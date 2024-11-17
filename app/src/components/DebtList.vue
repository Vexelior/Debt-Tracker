<template>
  <div class="container">
    <h2 class="my-4">Debt List</h2>
    <router-link to="/add-debt" class="btn btn-primary mb-2">Add New Debt</router-link>
    <ul class="list-group">
      <li v-for="debt in debts" :key="debt._id" class="list-group-item d-flex justify-content-between align-items-center">
        <span>{{ debt.creditor }}: {{ debt.amount | currency }}</span>
        <button @click="deleteDebt(debt._id)" class="btn btn-danger btn-sm">Delete</button>
      </li>
    </ul>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  data() {
    return {
      debts: [],
    };
  },
  async created() {
    await this.fetchDebts();
  },
  methods: {
    async fetchDebts() {
      const response = await axios.get('http://localhost:5000/api/debts');
      this.debts = response.data;
    },
    async deleteDebt(id) {
      await axios.delete(`http://localhost:5000/api/debts/${id}`);
      await this.fetchDebts(); // Refresh the list after deletion
    },
  },
  filters: {
    currency(value) {
      return `$${parseFloat(value).toFixed(2)}`;
    },
  },
};
</script>

<style scoped>
.container {
  margin-top: 20px;
}
</style>
