<template>
  <nav class="navbar navbar-expand-lg">
    <div class="container">
      <router-link class="navbar-brand" to="/Debt-Tracker/">Debt Tracker</router-link>
      <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav"
        aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
        <span class="navbar-toggler-icon"></span>
      </button>
      <div class="collapse navbar-collapse" id="navbarNav">
        <ul class="navbar-nav flex-column flex-lg-row">
          <li class="nav-item">
            <router-link class="nav-link" to="/Debt-Tracker/">Home</router-link>
          </li>
          <li class="nav-item">
            <router-link class="nav-link" to="/Debt-Tracker/add-debt">Add</router-link>
          </li>
          <li class="nav-item">
            <router-link class="nav-link" to="/Debt-Tracker/graphs">Graphs</router-link>
          </li>
          <li class="nav-item">
            <router-link class="nav-link" to="/Debt-Tracker/logging">Logs</router-link>
          </li>
        </ul>
        <ul class="navbar-nav ms-auto flex-column flex-lg-row">
          <div v-if="!isLoggedIn" class="d-flex flex-column flex-lg-row">
            <li class="nav-item">
              <router-link class="nav-link" to="/Debt-Tracker/login">Login</router-link>
            </li>
            <li class="nav-item">
              <router-link class="nav-link" to="/Debt-Tracker/register">Register</router-link>
            </li>
          </div>
          <div v-else class="d-flex flex-column flex-lg-row">
            <li class="nav-item">
              <button class="btn text-light nav-link" @click="logout">Logout</button>
            </li>
          </div>
        </ul>
      </div>
    </div>
  </nav>
</template>

<script>
export default {
  data() {
    return {
      isLoggedIn: !!localStorage.getItem('token'),
    };
  },
  name: 'NavbarView',
  watch: {
    '$route'() {
      this.isLoggedIn = !!localStorage.getItem('token');
    },
  },
  methods: {
    logout() {
      localStorage.removeItem('token');
      this.isLoggedIn = false;
      this.$router.push('/Debt-Tracker/login');
    },
  },
};
</script>
