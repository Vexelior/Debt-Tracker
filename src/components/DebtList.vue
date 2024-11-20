<template>
  <div class="container">
    <div v-if="debts.length > 0">
      <div class="row">
        <div v-for="debt in debts" :key="debt.id" class="col-md-4 mb-4 mt-4">
          <router-link :to="`/debt/${debt.id}`">
            <div class="card h-100" title="Click to view details">
              <div class="card-body">
                <h5 class="card-title">{{ debt.creditor }}</h5>
              </div>
            </div>
          </router-link>
        </div>
      </div>
    </div>
    <div v-else>
      <div class="loader text-center mt-5 col-md-6 m-auto">
        <div class="spinner-border text-primary" role="status">
          <span class="visually-hidden">Loading...</span>
        </div>
        <p>Loading...</p>
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
