<template>
  <div class="container">
    <h1 class="my-4">Log in</h1>
    <p v-if="message" class="text-success">{{ message }}</p>
    <p v-if="error" class="text-danger">{{ error }}</p>
    <form @submit.prevent="login">
      <div class="form-group">
        <label for="email">Email</label>
        <input class="form-control" v-model="email" type="email" id="email" required />
      </div>
      <div class="form-group">
        <label for="password">Password</label>
        <input class="form-control" v-model="password" type="password" id="password" required />
      </div>
      <button class="btn btn-primary mt-2" type="submit">Log in</button>
      <p class="mt-5">Don't have an account? <router-link to="/Debt-Tracker/register">Register</router-link></p>
    </form>
  </div>
</template>

<script>
import axios from 'axios';
import { ACCOUNT_CONTROLLER } from '../constants.js';

export default {
  data() {
    return {
      email: '',
      password: '',
      error: null,
      message: null,
    };
  },
  watch: {
    '$route.query': {
      handler: 'checkRouteQuery',
      immediate: true,
    },
  },
  created() {
    this.checkRouteQuery();
  },
  beforeRouteUpdate(to, from, next) {
    this.checkRouteQuery();
    next();
  },
  methods: {
    async login() {
      try {
        const response = await axios.post(`${ACCOUNT_CONTROLLER}/Login`, {
          email: this.email,
          password: this.password,
        });
        localStorage.setItem('token', response.data.token);
        this.$router.push(this.$route.query.redirect || '/Debt-Tracker/');
      } catch (error) {
        this.error = error.response?.data?.message || 'An error occurred.';
        document.querySelector('button[type="submit"]').disabled = false;
        document.querySelector('button[type="submit"]').textContent = 'Register';
        setTimeout(() => {
          this.error = null;
        }, 5000);
      }
    },
    checkRouteQuery() {
      if (this.$route.query.registered) {
        this.message = 'Registration successful. You can now log in.';
        setTimeout(() => {
          this.message = null;
        }, 5000);
      } else {
        this.message = null;
      }
      if (this.$route.query.redirect) {
        this.error = 'You must be logged in to view that page.';
      } else {
        this.error = null;
      }
    },
  },
};
</script>