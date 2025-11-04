import { createRouter, createWebHistory } from 'https://unpkg.com/vue-router@4.2.5/dist/vue-router.esm-browser.js';
import StoryListView from './views/StoryListView.js';
import IssueDetailView from './views/IssueDetailView.js';

const routes = [
  { path: '/', name: 'stories', component: StoryListView },
  { path: '/issue/:id', name: 'issue-detail', component: IssueDetailView, props: true },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;
