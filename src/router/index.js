import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap/dist/js/bootstrap.bundle.min.js';

import { createRouter, createWebHistory } from 'vue-router';
import DebtList from '../components/DebtList.vue';
import AddDebt from '../components/AddDebt.vue';
import DebtDetail from '../components/DebtDetail.vue';
import EditDebt from '../components/EditDebt.vue';
import GraphsPage from '../components/GraphsPage.vue';
import NotFound from '../components/NotFound.vue';

const routes = [
  {
    path: '/',
    name: 'home',
    component: DebtList,
    meta: { title: 'Home' }
  },
  {
    path: '/add-debt',
    name: 'add-debt',
    component: AddDebt,
    meta: { title: 'Add' }
  },
  {
    path: '/debt/:id', 
    name: 'debt-detail',
    component: DebtDetail,
    props: true, 
    meta: { title: 'Detail' }
  },
  {
    path: '/edit-debt/:id',
    name: 'edit-debt',
    component: EditDebt,
    props: true,
    meta: { title: 'Edit'},
  },
  {
    path: '/graphs',
    name: 'GraphsPage',
    component: GraphsPage,
    meta: {
      title: 'Graph',
    },
  },
  {
    path: '/*',
    component: NotFound,
  }
];

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes,
});

router.beforeEach((to, from, next) => {
  document.title = to.meta.title;
  next();
});

export default router;
