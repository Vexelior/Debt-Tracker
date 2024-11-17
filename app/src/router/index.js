import { createRouter, createWebHistory } from 'vue-router';
import DebtList from '../components/DebtList.vue';
import AddDebt from '../components/AddDebt.vue';

const routes = [
  {
    path: '/',
    name: 'home',
    component: DebtList,
  },
  {
    path: '/add-debt',
    name: 'add-debt',
    component: AddDebt,
  },
];

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL), // This is for Vite projects
  routes,
});

export default router;
