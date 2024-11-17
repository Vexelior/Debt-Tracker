import { createRouter, createWebHistory } from 'vue-router';
import DebtList from '../components/DebtList.vue';
import AddDebt from '../components/AddDebt.vue';
import DebtDetail from '../components/DebtDetail.vue';

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
  {
    path: '/debt/:id', 
    name: 'debt-detail',
    component: DebtDetail,
    props: true, 
  }
];

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes,
});

export default router;
