import { createRouter, createWebHistory } from 'vue-router';
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
