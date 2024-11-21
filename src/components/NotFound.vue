<template>
    <div class="not-found d-flex flex-column align-items-center  text-center">
      <h1 class="display-1 text-primary">404</h1>
      <p class="lead text-secondary">Oops! The page you're looking for doesn't exist.</p>
      <p id="counter"></p>
    </div>
  </template>
  
  <script>
  export default {
    name: "NotFound",
    mounted() {
      let count = 5;
      const counter = document.getElementById("counter");
      const interval = setInterval(() => {
        counter.textContent = `Redirecting in ${count} second(s)...`;
        count--;
        if (count < 0) {
          clearInterval(interval);
          this.$router.push("/");
        }
      }, 1000);
    },
  };
  </script>
  
  <style scoped>
  .not-found {
    height: 100vh;
    background-color: var(--tertiary);
    color: var(--primary);
  }
  
  h1 {
    font-weight: bold;
    font-size: 8rem;
  }
  
  p {
    font-size: 1.25rem;
  }
  </style>
  