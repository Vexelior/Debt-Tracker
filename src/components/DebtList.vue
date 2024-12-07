<template>
  <div v-if="loading">
    <div class="text-center mt-5 col-md-6 m-auto">
      <div class="spinner-border text-primary" role="status">
        <span class="visually-hidden">Loading...</span>
      </div>
      <p>Loading...</p>
    </div>
  </div>
  <div v-else>
    <div class="container">
      <div class="d-flex justify-content-between align-items-center">
        <h1 class="my-4">Debts</h1>
        <router-link class="btn btn-primary" to="/Debt-Tracker/add-debt">
          Add
        </router-link>
      </div>
      <div v-if="debts.length > 0" class="debt-list-container scrollable">
        <div class="row">
          <div v-for="debt in debts" :key="debt.id" class="col-md-4 mb-4">
            <router-link :to="`/Debt-Tracker/debt/${debt.id}`">
              <div class="card h-100">
                <div class="card-body">
                  <h5 class="card-title">{{ debt.creditor }}</h5>
                  <div v-if="debt.imageSource != null">
                    <img :src="debt.imageSource" class="card-img-top" alt="Creditor logo" />
                  </div>
                  <p class="card-text mt-2">
                    <strong>Balance:</strong> {{ debt.remainingAmount.toLocaleString('en-US', { style: 'currency', currency: 'USD' }) }}<br>
                  </p>
                </div>
              </div>
            </router-link>
          </div>
        </div>
      </div>
      <div v-else-if="debts.length === 0">
        <div class="mt-5">
          No debts found.
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import axios from 'axios';
import { DEBT_CONTROLLER } from '../constants.js';

export default {
  data() {
    return {
      debts: [],
      loading: true,
    };
  },
  async created() {
    await this.fetchDebts();
  },
  methods: {
    async fetchDebts() {
      try {
        const response = await axios.get(`${DEBT_CONTROLLER}`);
        this.debts = response.data;
      } catch (error) {
        console.error('Error fetching debts:', error);
      } finally {
        this.loading = false;
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
