<template>
  <div class="container">
    <div v-if="successMessage" class="alert alert-success alert-dismissible fade show mt-2" role="alert">
      {{ successMessage }}
      <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close" @click="clearSuccessMessage"></button>
    </div>

    <div v-if="errorMessage" class="alert alert-danger alert-dismissible fade show mt-2" role="alert">
      {{ errorMessage }}
      <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close" @click="clearMessage"></button>
    </div>

    <h2 class="my-4">Debt List</h2>

    <div class="row">
      <div v-for="debt in debts" :key="debt._id" class="col-md-4 mb-4">
        <div class="card h-100">
          <div class="card-body">
            <h5 class="card-title">{{ debt.creditor }}</h5>
            <p class="card-text">
              <strong>Amount:</strong> {{ formattedAmount(debt.amount) }}
              <span v-if="debt.percentageChange !== null && !isNaN(debt.percentageChange)" :class="{'text-success': debt.percentageChange > 0, 'text-danger': debt.percentageChange < 0}">
                {{ formattedPercentageChange(debt.percentageChange) }}
              </span>
            </p>
          </div>
          <div class="card-footer text-end">
            <router-link :to="`/debt/${debt._id}`" class="btn btn-secondary btn-sm me-2">View</router-link>
            <router-link :to="`/edit-debt/${debt._id}`" class="btn btn-primary btn-sm me-2">Edit</router-link>
          </div>
        </div>
      </div>
    </div>
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
      errorMessage: null,
    };
  },
  async created() {
    await this.fetchDebts();
    // Handling success and error messages passed via EventBus
    this.successMessage = EventBus.successMessage;
    this.errorMessage = EventBus.errorMessage;
    if (this.successMessage || this.errorMessage) {
      setTimeout(() => {
        this.clearMessage();
      }, 5000);
    }
  },
  methods: {
    async fetchDebts() {
      try {
        const response = await axios.get('http://localhost:5000/api/debts');
        this.debts = response.data;
      } catch (error) {
        console.error('Error fetching debts:', error);
      }
    },
    clearMessage() {
      this.successMessage = null;
      this.errorMessage = null;
    },
  },
  computed: {
    formattedAmount() {
      return function(value) {
        return `$${parseFloat(value).toFixed(2)}`;
      };
    },
    formattedPercentageChange() {
      return function(value) {
        if (value !== null && !isNaN(value)) {
          return `(${value.toFixed(2)}%)`; // Ensure value is a valid number before formatting
        }
        return ''; // Return an empty string if the value is not valid
      };
    },
  },
};
</script>
