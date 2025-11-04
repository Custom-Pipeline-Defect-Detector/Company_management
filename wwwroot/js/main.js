import { createApp } from 'https://unpkg.com/vue@3.3.4/dist/vue.esm-browser.js';
import { createPinia } from 'https://unpkg.com/pinia@2.1.7/dist/pinia.esm-browser.js';
import router from './router.js';

const App = {
  template: `
    <div>
      <nav class="top-bar">
        <router-link class="brand" :to="{ name: 'stories' }">Company Management</router-link>
      </nav>
      <main class="content">
        <router-view></router-view>
      </main>
    </div>
  `,
};

const app = createApp(App);
app.use(createPinia());
app.use(router);
app.mount('#app');
