import { computed } from 'vue';
import { RouterLink } from 'vue-router';

export default {
  name: 'HomeView',
  props: {
    stories: {
      type: Array,
      default: () => []
    },
    loading: {
      type: Boolean,
      default: false
    },
    reload: {
      type: Function,
      required: true
    }
  },
  setup(props) {
    return {
      sortedStories: computed(() => [...props.stories].sort((a, b) => a.id - b.id))
    };
  },
  components: {
    RouterLink
  },
  template: `
    <section>
      <div v-if="loading && !stories.length" class="placeholder loading">Loading stories…</div>
      <div v-else-if="!stories.length" class="placeholder">
        No stories yet. <button class="refresh" type="button" @click="reload">Try again</button>
      </div>
      <div v-else class="story-list">
        <article v-for="story in sortedStories" :key="story.id" class="story-card">
          <h2>{{ story.name }}</h2>
          <p>{{ story.description || 'No description provided.' }}</p>
          <div class="story-meta">
            <span>Status: {{ story.status }}</span>
            <span>Priority: {{ story.priority }}</span>
            <span>Attachments: {{ story.attachmentCount }}</span>
            <span>Comments: {{ story.commentCount }}</span>
          </div>
          <div class="story-actions">
            <RouterLink :to="{ name: 'story-detail', params: { id: story.id } }">
              View details →
            </RouterLink>
          </div>
        </article>
      </div>
    </section>
  `
};
