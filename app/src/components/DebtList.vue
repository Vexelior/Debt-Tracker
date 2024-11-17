<!-- DebtList.vue -->
<template>
  <div class="container">
    <!-- Display Bootstrap success alert -->
    <div v-if="successMessage" class="alert alert-success alert-dismissible fade show mt-2" role="alert">
      {{ successMessage }}
      <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close" @click="clearSuccessMessage"></button>
    </div>

    <h2 class="my-4">Debt List</h2>
    <router-link to="/add-debt" class="btn btn-primary mb-2">Add New Debt</router-link>
    <ul class="list-group">
      <li v-for="debt in debts" :key="debt._id" class="list-group-item d-flex justify-content-between align-items-center">
        <span>{{ debt.creditor }}: {{ formattedAmount(debt.amount) }}</span>
        <button @click="deleteDebt(debt._id)" class="btn btn-danger btn-sm">Delete</button>
      </li>
    </ul>
  </div>
</template>

<script>
import axios from 'axios';
import { EventBus } from '../EventBus';

export default {
  data() {
    return {
      debts: [],
      successMessage: null,
    };
  },
  async created() {
    await this.fetchDebts();
    this.successMessage = EventBus.successMessage;
  },
  watch: {
    '$route'() {
      this.successMessage = EventBus.successMessage;
      if (this.successMessage) {
        setTimeout(() => {
          this.clearSuccessMessage();
        }, 5000);
      }
    },
  },
  methods: {
    async fetchDebts() {
      const response = await axios.get('http://localhost:5000/api/debts');
      this.debts = response.data;
    },
    async deleteDebt(id) {
      await axios.delete(`http://localhost:5000/api/debts/${id}`);
      await this.fetchDebts(); 
    },
    clearSuccessMessage() {
      this.successMessage = null;
      EventBus.successMessage = null;
    },
  },
  computed: {
    formattedAmount() {
      return (value) => `$${parseFloat(value).toFixed(2)}`;
    },
  },
};
</script>
