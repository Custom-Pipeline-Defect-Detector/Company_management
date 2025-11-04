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

    return {
      story,
      attachments,
      comments,
      tasks,
      isLoading,
      error,
    };
  },
  template: `
    <section class="detail" v-if="story">
      <header class="detail__header">
        <h1>{{ story.name }}</h1>
        <p class="muted">Status: {{ story.status }}</p>
        <p class="muted">Priority: {{ story.priority }}</p>
        <p class="muted" v-if="story.assignedTo">Assigned to: {{ story.assignedTo.name }}</p>
      </header>
      <article>
        <h2>Description</h2>
        <p>{{ story.description }}</p>
      </article>
      <section>
        <h2>Attachments</h2>
        <p v-if="!attachments.length" class="muted">No attachments uploaded.</p>
        <ul v-else>
          <li v-for="attachment in attachments" :key="attachment.id">
            <a :href="attachment.filePath" target="_blank" rel="noopener">{{ attachment.fileName }}</a>
          </li>
        </ul>
      </section>
      <section>
        <h2>Comments</h2>
        <p v-if="!comments.length" class="muted">No comments yet.</p>
        <ul v-else class="comments">
          <li v-for="comment in comments" :key="comment.id">
            <header>
              <strong>{{ comment.user?.name ?? 'Unknown User' }}</strong>
              <span class="muted">{{ comment.createdAt ? new Date(comment.createdAt).toLocaleString() : '' }}</span>
            </header>
            <p>{{ comment.text }}</p>
          </li>
        </ul>
      </section>
      <section>
        <h2>Tasks</h2>
        <p v-if="!tasks.length" class="muted">No tasks linked to this story.</p>
        <ul v-else class="tasks">
          <li v-for="task in tasks" :key="task.id">
            <strong>{{ task.title }}</strong>
            <p class="muted" v-if="task.description">{{ task.description }}</p>
            <ul v-if="task.attachments && task.attachments.length">
              <li v-for="attachment in task.attachments" :key="attachment.id">
                <a :href="attachment.filePath" target="_blank" rel="noopener">{{ attachment.fileName }}</a>
              </li>
            </ul>
          </li>
        </ul>
      </section>
      <router-link class="back-link" :to="{ name: 'stories' }">← Back to all stories</router-link>
    </section>
    <section v-else class="detail">
      <p v-if="isLoading" class="muted">Loading story…</p>
      <p v-else-if="error" class="error">{{ error }}</p>
      <p v-else class="muted">Story not found.</p>
      <router-link class="back-link" :to="{ name: 'stories' }">← Back to all stories</router-link>
    </section>
  `,
};
