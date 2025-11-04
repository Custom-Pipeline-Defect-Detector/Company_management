if (!window.Pinia || !window.axios) {
  throw new Error('Pinia or Axios failed to load.');
}

const { defineStore } = window.Pinia;
const http = window.axios;

export const useStore = defineStore('main', {
  state: () => ({
    stories: [],
    isLoading: false,
    lastError: null,
  }),
  actions: {
    async fetchStories() {
      if (this.isLoading) return;

      this.isLoading = true;
      this.lastError = null;

      try {
        const response = await http.get('/api/stories');
        this.stories = response.data;
      } catch (error) {
        const message = error instanceof Error ? error.message : 'Unknown error';
        this.lastError = message;
        throw error;
      } finally {
        this.isLoading = false;
      }
    },
    getStoryById(id) {
      return this.stories.find((story) => story.id === Number(id));
    },
  },
});
