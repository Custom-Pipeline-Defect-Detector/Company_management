import { defineStore } from 'pinia';
import axios from 'axios';

const api = axios.create({
  baseURL: '/api',
  headers: {
    'Content-Type': 'application/json'
  }
});

export const useStoryStore = defineStore('stories', {
  state: () => ({
    stories: []
  }),
  actions: {
    async fetchStories() {
      const { data } = await api.get('/stories');
      this.stories = Array.isArray(data) ? data : [];
    }
  }
});
