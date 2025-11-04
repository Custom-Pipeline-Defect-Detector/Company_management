import { computed, onMounted } from 'https://unpkg.com/vue@3.3.4/dist/vue.esm-browser.js';
import { useStore } from '../store.js';

export default {
  name: 'StoryListView',
  setup() {
    const store = useStore();
    const stories = computed(() => store.stories);
    const isLoading = computed(() => store.isLoading);
    const lastError = computed(() => store.lastError);

    onMounted(() => {
      if (!store.stories.length && !store.isLoading) {
        store.fetchStories().catch(() => {
          /* error state handled via lastError */
        });
      }
    });

    return {
      stories,
      isLoading,
      lastError,
    };
  },
  template: `
    <section>
      <h1 class="page-title">Stories</h1>
      <p v-if="isLoading" class="muted">Loading stories…</p>
      <p v-else-if="lastError" class="error">Failed to load stories: {{ lastError }}</p>
      <p v-else-if="!stories.length" class="muted">There are no stories yet.</p>
      <ul v-else class="story-list">
        <li v-for="story in stories" :key="story.id" class="story-list__item">
          <router-link :to="{ name: 'issue-detail', params: { id: story.id } }">
            <h2>{{ story.name }}</h2>
            <p class="muted">{{ story.description }}</p>
            <span class="badge">Status: {{ story.status }}</span>
            <span class="badge" v-if="story.assignedTo">Assigned to: {{ story.assignedTo.name }}</span>
            <span class="badge" v-if="story.commentCount">💬 {{ story.commentCount }}</span>
            <span class="badge" v-if="story.attachmentCount">📎 {{ story.attachmentCount }}</span>
          </router-link>
        </li>
      </ul>
    </section>
  `,
};
