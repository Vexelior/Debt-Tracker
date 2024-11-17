<template>
  <div id="app">
    <!-- Navbar will be displayed on every page -->
    <Navbar />
    
    <!-- Dynamic content will be injected here based on the current route -->
    <router-view />
  </div>
</template>

<script>
import Navbar from './components/Navbar.vue'; // Import Navbar component

export default {
  name: 'App',
  components: {
    Navbar, // Register Navbar component
  },
};
</script>
