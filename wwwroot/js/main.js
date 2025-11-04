import { createApp } from 'https://unpkg.com/vue@3.3.4/dist/vue.esm-browser.js';
import { createPinia } from 'https://unpkg.com/pinia@2.1.7/dist/pinia.esm-browser.js';
import router from './router.js';

const App = {
  template: `
    <div class="app-frame">
      <header class="app-header">
        <div class="app-header__branding">
          <button class="app-header__menu" aria-label="Toggle navigation">
            <span></span>
            <span></span>
            <span></span>
          </button>
          <router-link class="app-header__title" :to="{ name: 'stories' }">
            Company Management CRM
          </router-link>
        </div>
        <div class="app-header__actions">
          <button class="pill">Create Story</button>
          <button class="pill pill--secondary">Log Call</button>
          <button class="avatar" aria-label="View profile">CM</button>
        </div>
      </header>

      <div class="app-layout">
        <aside class="sidebar">
          <nav class="sidebar__nav">
            <p class="sidebar__section">Workspace</p>
            <router-link class="sidebar__link" active-class="sidebar__link--active" :to="{ name: 'stories' }">
              <span class="sidebar__icon">📋</span>
              Stories
            </router-link>
            <a class="sidebar__link" href="#">
              <span class="sidebar__icon">🗂️</span>
              Projects
            </a>
            <a class="sidebar__link" href="#">
              <span class="sidebar__icon">👥</span>
              Teams
            </a>

            <p class="sidebar__section">Insights</p>
            <a class="sidebar__link" href="#">
              <span class="sidebar__icon">📊</span>
              Dashboards
            </a>
            <a class="sidebar__link" href="#">
              <span class="sidebar__icon">⏱️</span>
              Activities
            </a>
          </nav>

          <div class="sidebar__footer">
            <p class="sidebar__section">Shortcuts</p>
            <button class="shortcut">+ New Comment</button>
            <button class="shortcut">+ New Attachment</button>
          </div>
        </aside>

        <main class="main-content">
          <router-view></router-view>
        </main>
      </div>
    </div>
  `,
};

const app = createApp(App);
app.use(createPinia());
app.use(router);
app.mount('#app');
