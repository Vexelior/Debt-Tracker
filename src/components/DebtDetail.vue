<template>
  <div class="container">
    <h2 class="my-4">Debt Details</h2>
    <div v-if="debt" class="card">
      <div class="card-body">
        <h5 class="card-title">{{ debt.creditor }}</h5>
        <p class="card-text">
          <strong>Amount:</strong> {{ formattedAmount }}<br>
        </p>
        <p class="card-text">
          <strong>Notes:</strong> {{ debt.notes ? debt.notes : 'No current note.' }}
        </p>
        <p class="mb-3">
          <strong>Date Added:</strong> {{ formattedDateAdded }}
        </p>
        <p class="mb-3">
          <strong>Last Edit:</strong> {{ formattedDateAdded }}
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
        const response = await axios.get(`https://localhost:7164/Debt/${debtId}`);
        this.debt = response.data;
      } catch (error) {
        console.error("Error fetching debt details:", error);
      }
    },
  },
  computed: {
    formattedAmount() {
      return new Intl.NumberFormat('en-US', {
        style: 'currency',
        currency: 'USD',
      }).format(this.debt.amount);
    },
    formattedDateAdded() {
      return new Date(this.debt.dateAdded).toLocaleDateString();
    },
    formattedDateLastEdit() {
      return new Date(this.debt.dateLastEdit).toLocaleDateString();
    },
  },
};
</script>