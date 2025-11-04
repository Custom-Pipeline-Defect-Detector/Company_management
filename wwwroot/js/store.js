import { defineStore } from 'https://unpkg.com/pinia@2.1.7/dist/pinia.esm-browser.js';
import axios from 'https://cdn.jsdelivr.net/npm/axios@1.6.7/dist/axios.esm.min.js';

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
        const response = await axios.get('/api/stories');
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
