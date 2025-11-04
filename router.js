import { createRouter, createWebHistory } from 'vue-router';
import IssueDetail from './components/IssueDetail.vue';

const routes = [
  { path: '/issue/:id', component: IssueDetail, props: true },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;
