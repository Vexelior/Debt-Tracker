<template>
  <div class="container">
    <div v-if="successMessage" class="alert alert-success alert-dismissible fade show mt-2" role="alert">
      {{ successMessage }}
      <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"
        @click="clearSuccessMessage"></button>
    </div>

    <div v-if="errorMessage" class="alert alert-danger alert-dismissible fade show mt-2" role="alert">
      {{ errorMessage }}
      <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close" @click="clearMessage"></button>
    </div>

    <div v-if="debts.length > 0">
      <div class="row">
        <div v-for="debt in debts" :key="debt._id" class="col-md-4 mb-4 mt-4">
          <div class="card h-100">
            <div class="card-body">
              <h5 class="card-title">{{ debt.creditor }}</h5>
              <p class="card-text">
                <strong>Amount:</strong> {{ formattedAmount(debt.remainingAmount) }}
                <span v-if="debt.percentageChange > 0" class="badge text-bg-danger">
                  {{ formattedPercentageChange(debt.percentageChange) }}
                </span>
                <span v-else-if="debt.percentageChange === 0" class="badge text-bg-secondary">
                  {{ formattedPercentageChange(debt.percentageChange) }}
                </span>
                <span v-else class="badge text-bg-success">
                  {{ formattedPercentageChange(debt.percentageChange) }}
                </span>
              </p>
            </div>
            <div class="card-footer text-end">
              <router-link :to="`/debt/${debt.id}`" class="btn btn-secondary btn-sm me-2">View</router-link>
              <router-link :to="`/edit-debt/${debt.id}`" class="btn btn-primary btn-sm me-2">Edit</router-link>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div v-else-if="debts.length === 0">
      <p>No debts to display.</p>
    </div>
    <div v-else>
      <p>Loading debt information...</p>
    </div>
  </div>
</template>

<script>
import axios from 'axios';
import { EventBus } from '../EventBus';
import { DEBT_CONTROLLER } from '../constants.js';

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
        const response = await axios.get(`${DEBT_CONTROLLER}`);
        this.debts = response.data;
      } catch (error) {
        console.error('Error fetching debts:', error);
      }
    },
    clearMessage() {
      this.successMessage = null;
      this.errorMessage = null;
      var alert = document.querySelector('.alert');
      if (alert) {
        alert.remove();
      } 
    },
  },
  computed: {
    formattedAmount() {
      return function (value) {
        return `$${parseFloat(value).toFixed(2)}`;
      };
    },
    formattedPercentageChange() {
      return function (value) {
        if (value !== null && !isNaN(value)) {
          return `(${value.toFixed(2)}%)`;
        }
        return '';
      };
    },
  },
};
</script>
