import StoryListView from './views/StoryListView.js';
import IssueDetailView from './views/IssueDetailView.js';

if (!window.VueRouter) {
  throw new Error('Vue Router failed to load.');
}

const { createRouter, createWebHistory } = window.VueRouter;

const routes = [
  { path: '/', name: 'stories', component: StoryListView },
  { path: '/issue/:id', name: 'issue-detail', component: IssueDetailView, props: true },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;
