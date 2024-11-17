<template>
  <div id="app">
    <!-- Navbar will be displayed on every page -->
    <Navbar />
    
    <!-- Dynamic content will be injected here based on the current route -->
    <router-view />

    <!-- Footer will be displayed on every page -->
     <FooterComponent />
  </div>
</template>

<script>
import Navbar from './components/Navbar.vue'; // Import Navbar component
import FooterComponent from './components/FooterComponent.vue'; // Import Footer component

export default {
  name: 'App',
  components: {
    Navbar, // Register Navbar component
    FooterComponent, // Register Footer component
  },
};
</script>
