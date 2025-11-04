import { computed, onMounted, ref } from 'vue';
import { useStoryStore } from '../store.js';

export default {
  name: 'AppRoot',
  setup() {
    const store = useStoryStore();
    const loading = ref(false);
    const error = ref('');

    const loadStories = async () => {
      loading.value = true;
      error.value = '';
      try {
        await store.fetchStories();
      } catch (err) {
        const message = err?.response?.data?.message ?? err?.message ?? 'Unable to load stories.';
        error.value = message;
      } finally {
        loading.value = false;
      }
    };

    onMounted(loadStories);

    return {
      stories: computed(() => store.stories),
      loading,
      error,
      loadStories
    };
  },
  template: `
    <div class="app">
      <header class="app__header">
        <h1>Company Management</h1>
        <button type="button" class="refresh" @click="loadStories" :disabled="loading">
          {{ loading ? 'Refreshing…' : 'Refresh stories' }}
        </button>
      </header>
      <section v-if="error" class="alert" role="alert">
        {{ error }}
      </section>
      <router-view v-slot="{ Component, route, props }">
        <component
          :is="Component"
          v-bind="props"
          :stories="stories"
          :loading="loading"
          :reload="loadStories"
        />
      </router-view>
    </div>
  `
};
