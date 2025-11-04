import { useStore } from '../store.js';

if (!window.Vue) {
  throw new Error('Vue failed to load.');
}

const { computed, onMounted } = window.Vue;

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

    const getStatusBadgeClass = (status) => {
      if (!status) return '';

      const normalized = status.toLowerCase();
      if (['closed', 'done', 'completed', 'resolved'].includes(normalized)) {
        return 'badge--status-closed';
      }

      return 'badge--status-open';
    };

    return {
      stories,
      isLoading,
      lastError,
      getStatusBadgeClass,
    };
  },
  template: `
    <section>
      <header>
        <h1 class="page-title">Story Workspace</h1>
        <p class="page-subtitle">Monitor, triage, and collaborate on company initiatives in one view.</p>
      </header>

      <div class="story-toolbar" v-if="!isLoading && !lastError && stories.length">
        <div class="filter-group">
          <button class="filter filter--active">All</button>
          <button class="filter">Active</button>
          <button class="filter">Waiting</button>
          <button class="filter">Closed</button>
        </div>
        <button class="pill">+ New Story</button>
      </div>

      <div v-if="isLoading" class="empty-state">Loading stories…</div>
      <div v-else-if="lastError" class="empty-state error">Failed to load stories: {{ lastError }}</div>
      <div v-else-if="!stories.length" class="empty-state">There are no stories yet. Start by creating a new story.</div>
      <ul v-else class="story-list">
        <li v-for="story in stories" :key="story.id">
          <router-link class="story-card" :to="{ name: 'issue-detail', params: { id: story.id } }">
            <div>
              <h2>{{ story.name }}</h2>
              <p class="story-card__description">{{ story.description }}</p>
            </div>
            <div class="story-meta">
              <span class="badge" :class="getStatusBadgeClass(story.status)">
                <span>●</span>
                {{ story.status || 'No status' }}
              </span>
              <span class="badge" v-if="story.assignedTo">
                👤 {{ story.assignedTo.name }}
              </span>
              <span class="badge" v-if="story.priority">
                ⭐ {{ story.priority }}
              </span>
              <span class="badge" v-if="story.commentCount">
                💬 {{ story.commentCount }}
              </span>
              <span class="badge" v-if="story.attachmentCount">
                📎 {{ story.attachmentCount }}
              </span>
            </div>
          </router-link>
        </li>
      </ul>
    </section>
  `,
};
