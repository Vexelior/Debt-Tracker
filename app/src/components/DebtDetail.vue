<template>
    <div class="container">
      <h2 class="my-4">Debt Details</h2>
      <div v-if="debt" class="card">
        <div class="card-body">
          <h5 class="card-title">{{ debt.creditor }}</h5>
          <p class="card-text">
            <strong>Amount:</strong> {{ formattedAmount }}<br>
            <strong>Notes:</strong> {{ debt.notes }}
          </p>
          <router-link to="/" class="btn btn-secondary">Back</router-link>
        </div>
      </div>
      <div v-else>
        <p>Loading details...</p>
      </div>
    </div>
  </template>
  
  <script>
  import axios from 'axios';
  
  export default {
    data() {
      return {
        debt: null,
      };
    },
    async created() {
      const debtId = this.$route.params.id;
      await this.fetchDebtDetails(debtId);
    },
    methods: {
      async fetchDebtDetails(debtId) {
        try {
          const response = await axios.get(`http://localhost:5000/api/debts/${debtId}`);
          this.debt = response.data;
        } catch (error) {
          console.error("Error fetching debt details:", error);
        }
      },
    },
    computed: {
      formattedAmount() {
        return `$${parseFloat(this.debt.amount).toFixed(2)}`;
      },
    },
  };
  </script>
  