import { computed, onMounted, ref, watch } from 'https://unpkg.com/vue@3.3.4/dist/vue.esm-browser.js';
import axios from 'https://cdn.jsdelivr.net/npm/axios@1.6.7/dist/axios.esm.min.js';
import { useStore } from '../store.js';

export default {
  name: 'IssueDetailView',
  props: {
    id: {
      type: String,
      required: true,
    },
  },
  setup(props) {
    const store = useStore();
    const isLoading = ref(false);
    const error = ref(null);
    const story = ref(null);

    const loadStory = async (identifier) => {
      const numericId = Number(identifier);
      if (Number.isNaN(numericId)) {
        error.value = 'Invalid story id';
        story.value = null;
        return;
      }

      const existing = store.getStoryById(numericId);
      if (existing) {
        story.value = existing;
      }

      isLoading.value = true;
      error.value = null;

      try {
        const response = await axios.get(`/api/stories/${numericId}`);
        story.value = response.data;
      } catch (err) {
        error.value = err instanceof Error ? err.message : 'Unable to load story';
      } finally {
        isLoading.value = false;
      }
    };

    onMounted(() => {
      loadStory(props.id);
    });

    watch(
      () => props.id,
      (newId) => {
        if (newId) {
          loadStory(newId);
        }
      }
    );

    const attachments = computed(() => story.value?.attachments ?? []);
    const comments = computed(() => story.value?.comments ?? []);
    const tasks = computed(() => story.value?.tasks ?? []);

    const formatDate = (value) => {
      if (!value) return '';
      const date = new Date(value);
      if (Number.isNaN(date.getTime())) return '';
      return date.toLocaleString();
    };

    return {
      story,
      attachments,
      comments,
      tasks,
      isLoading,
      error,
      formatDate,
    };
  },
  template: `
    <section v-if="story" class="detail-view">
      <header class="detail-header">
        <h1>{{ story.name }}</h1>
        <p>{{ story.description }}</p>
        <div class="inline-meta">
          <span>Status: <strong>{{ story.status || 'Unspecified' }}</strong></span>
          <span v-if="story.priority">Priority: <strong>{{ story.priority }}</strong></span>
          <span v-if="story.assignedTo">Owner: <strong>{{ story.assignedTo.name }}</strong></span>
        </div>
      </header>

      <div class="detail-grid">
        <article class="panel">
          <h2>Overview</h2>
          <p>{{ story.longDescription || story.description }}</p>
          <div class="inline-meta">
            <span v-if="story.createdAt">Created: <strong>{{ formatDate(story.createdAt) }}</strong></span>
            <span v-if="story.updatedAt">Updated: <strong>{{ formatDate(story.updatedAt) }}</strong></span>
          </div>
        </article>

        <section class="panel">
          <h2>Attachments</h2>
          <p v-if="!attachments.length" class="muted">No attachments uploaded.</p>
          <ul v-else class="panel__list">
            <li v-for="attachment in attachments" :key="attachment.id" class="list-item">
              <strong>{{ attachment.fileName }}</strong>
              <a :href="attachment.filePath" target="_blank" rel="noopener">View file</a>
            </li>
          </ul>
        </section>

        <section class="panel">
          <h2>Comments</h2>
          <p v-if="!comments.length" class="muted">No comments yet.</p>
          <ul v-else class="panel__list">
            <li v-for="comment in comments" :key="comment.id" class="list-item">
              <strong>{{ comment.user?.name ?? 'Unknown User' }}</strong>
              <p>{{ comment.text }}</p>
              <span class="muted">{{ formatDate(comment.createdAt) }}</span>
            </li>
          </ul>
        </section>

        <section class="panel">
          <h2>Tasks</h2>
          <p v-if="!tasks.length" class="muted">No tasks linked to this story.</p>
          <ul v-else class="panel__list">
            <li v-for="task in tasks" :key="task.id" class="list-item">
              <strong>{{ task.title }}</strong>
              <p v-if="task.description" class="muted">{{ task.description }}</p>
              <ul v-if="task.attachments && task.attachments.length" class="panel__list">
                <li v-for="attachment in task.attachments" :key="attachment.id" class="list-item">
                  <a :href="attachment.filePath" target="_blank" rel="noopener">{{ attachment.fileName }}</a>
                </li>
              </ul>
            </li>
          </ul>
        </section>
      </div>

      <router-link class="back-link" :to="{ name: 'stories' }">Back to all stories</router-link>
    </section>
    <section v-else class="empty-state detail-view">
      <p v-if="isLoading">Loading story…</p>
      <p v-else-if="error" class="error">{{ error }}</p>
      <p v-else>Story not found.</p>
      <router-link class="back-link" :to="{ name: 'stories' }">Back to all stories</router-link>
    </section>
  `,
};
