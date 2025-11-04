import { createRouter, createWebHistory } from 'vue-router';
import Home from './views/Home.js';
import IssueDetail from './views/IssueDetail.js';

const routes = [
  {
    path: '/',
    name: 'home',
    component: Home
  },
  {
    path: '/stories/:id',
    name: 'story-detail',
    component: IssueDetail,
    props: route => ({ id: Number(route.params.id) })
  }
];

export default createRouter({
  history: createWebHistory(),
  routes
});
