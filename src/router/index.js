import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap/dist/js/bootstrap.bundle.min.js';

import { createRouter, createWebHistory } from 'vue-router';
import DebtList from '../components/DebtList.vue';
import AddDebt from '../components/AddDebt.vue';
import DebtDetail from '../components/DebtDetail.vue';
import EditDebt from '../components/EditDebt.vue';
import GraphsPage from '../components/GraphsPage.vue';
import NotFound from '../components/NotFound.vue';
import LoggingPage from '../components/LoggingPage.vue';
import RegisterPage from '../components/RegisterAccount.vue';
import LoginPage from '../components/LoginAccount.vue';
import AddPayment from '../components/AddPayment.vue';

const routes = [
  {
    path: '/',
    name: 'home',
    component: DebtList,
    meta: { title: 'Home', requiresAuth: true }
  },
  {
    path: '/add-debt',
    name: 'add-debt',
    component: AddDebt,
    meta: { title: 'Add', requiresAuth: true  }
  },
  {
    path: '/debt/:id', 
    name: 'debt-detail',
    component: DebtDetail,
    props: true, 
    meta: { title: 'Detail', requiresAuth: true  }
  },
  {
    path: '/edit-debt/:id',
    name: 'edit-debt',
    component: EditDebt,
    props: true,
    meta: { title: 'Edit', requiresAuth: true },
  },
  {
    path: '/graphs',
    name: 'GraphsPage',
    component: GraphsPage,
    meta: { title: 'Graph', requiresAuth: true },
  },
  {
    path: '/logging',
    name: 'LoggingPage',
    component: LoggingPage,
    meta: { title: 'Logging', requiresAuth: true },
  },
  {
    path: '/register',
    name: 'RegisterPage',
    component: RegisterPage,
    meta: { title: 'Register' },
  },
  {
    path: '/login',
    name: 'LoginPage',
    component: LoginPage,
    meta: { title: 'Login' },
  },
  {
    path: '/add-payment/:id',
    name: 'add-payment',
    component: AddPayment,
    props: true,
    meta: { title: 'Add Payment', requiresAuth: true },
  },
  {
    path: '/:catchAll(.*)',
    component: NotFound,
    meta: { title: '404 Not Found' },
  }
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

router.beforeEach((to, from, next) => {
  const isAuthenticated = localStorage.getItem('token');
  if (to.meta.requiresAuth && !isAuthenticated) {
    next({ path: '/login', query: { redirect: to.fullPath } });
  }
  document.title = to.meta.title;
  next();
});

export default router;
