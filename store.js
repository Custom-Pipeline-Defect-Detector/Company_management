import { defineStore } from 'pinia';
import axios from 'axios';

export const useStore = defineStore('main', {
  state: () => ({
    stories: [],
    tasks: [],
    comments: [],
  }),
  actions: {
    async fetchStories() {
      const response = await axios.get('/api/stories');
      this.stories = response.data;
    },
  },
});
