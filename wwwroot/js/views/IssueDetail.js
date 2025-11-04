import { RouterLink } from 'vue-router';

export default {
  name: 'IssueDetailView',
  props: {
    stories: {
      type: Array,
      default: () => []
    },
    id: {
      type: [Number, String],
      required: false
    }
  },
  computed: {
    story() {
      const numericId = Number(this.id);
      return this.stories.find(story => story.id === numericId);
    },
    assignedTo() {
      return this.story?.assignedTo?.name ?? 'Unassigned';
    }
  },
  template: `
    <section>
      <div v-if="!story" class="placeholder">
        <p v-if="stories.length">Select a story to see its details.</p>
        <p v-else>No stories available yet.</p>
        <RouterLink v-if="$route.name === 'story-detail'" :to="{ name: 'home' }">← Back to list</RouterLink>
      </div>
      <article v-else class="detail-card">
        <RouterLink class="story-actions" :to="{ name: 'home' }">← Back to stories</RouterLink>
        <h2>{{ story.name }}</h2>
        <p>{{ story.description || 'No description provided.' }}</p>
        <div class="detail-meta">
          <div><strong>Status:</strong> {{ story.status }}</div>
          <div><strong>Priority:</strong> {{ story.priority }}</div>
          <div><strong>Assigned to:</strong> {{ assignedTo }}</div>
          <div><strong>Attachments:</strong> {{ story.attachmentCount }}</div>
          <div><strong>Comments:</strong> {{ story.commentCount }}</div>
        </div>
      </article>
    </section>
  `,
  components: {
    RouterLink
  }
};
