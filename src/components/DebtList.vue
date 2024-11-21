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
      <h1 class="my-4">Debts</h1>
      <div v-if="debts.length > 0">
        <div class="row">
          <div v-for="debt in debts" :key="debt.id" class="col-md-4">
            <router-link :to="`/debt/${debt.id}`">
              <div class="card h-100" title="Click to view details">
                <div class="card-body">
                  <h5 class="card-title">{{ debt.creditor }}</h5>
                  <div v-if="debt.image != null">
                    <img :src="debt.image" class="card-img-top" alt="Creditor logo" />
                  </div>
                  <p class="card-text mt-2">
                    <strong>Balance:</strong> {{ debt.previousAmount.toLocaleString('en-US', { style: 'currency', currency: 'USD' }) }}<br>
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
