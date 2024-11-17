<template>
    <div class="container">
      <h2 class="my-4">Edit Debt</h2>
      <div v-if="debt">
        <form @submit.prevent="updateDebt">
          <div class="mb-3">
            <label for="creditor" class="form-label">Creditor</label>
            <input type="text" class="form-control" id="creditor" v-model="debt.creditor" required />
          </div>
          <div class="mb-3">
            <label for="amount" class="form-label">Amount</label>
            <input type="number" class="form-control" id="amount" v-model="debt.amount" required />
          </div>
          <button type="submit" class="btn btn-primary">Save Changes</button>
          <router-link to="/" class="btn btn-secondary ms-2">Cancel</router-link>
        </form>
      </div>
      <div v-else>
        <p>Loading debt information...</p>
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
      async updateDebt() {
        try {
          const debtId = this.$route.params.id;
          await axios.put(`http://localhost:5000/api/debts/${debtId}`, this.debt);
          this.$router.push("/");
        } catch (error) {
          console.error("Error updating debt:", error);
        }
      },
    },
  };
  </script>
  