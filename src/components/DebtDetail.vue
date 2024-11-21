<template>
  <div class="container">
    <router-link to="/" class="btn btn-secondary mt-3">Back</router-link>
    <div v-if="isDebt">
      <ul class="nav nav-tabs mt-3" id="debtTabs" role="tablist">
        <li class="nav-item" role="presentation">
          <button class="nav-link active" id="details-tab" data-bs-toggle="tab" data-bs-target="#details" type="button" role="tab" aria-controls="details" aria-selected="true">Details</button>
        </li>
        <li class="nav-item" role="presentation">
          <button class="nav-link" id="previous-amounts-tab" data-bs-toggle="tab" data-bs-target="#previous-amounts" type="button" role="tab" aria-controls="previous-amounts" aria-selected="false">Previous Amounts</button>
        </li>
        <li class="nav-item" role="presentation">
          <button class="nav-link" id="history-tab" data-bs-toggle="tab" data-bs-target="#history" type="button" role="tab" aria-controls="history" aria-selected="false">History</button>
        </li>
      </ul>
      <div class="tab-content" id="debtTabsContent">
        <div class="tab-pane fade show active" id="details" role="tabpanel" aria-labelledby="details-tab">
          <div class="row">
            <div class="col">
              <div v-if="debt" class="card">
                <h1 class="card-header">Details</h1>
                <div class="card-body">
                  <h5 class="card-title">{{ debt.creditor }}</h5>
                  <p class="card-text">
                    <strong>Original Balance:</strong> {{ debt.amount.toLocaleString('en-US', { style: 'currency', currency: 'USD' }) }}<br>
                  </p>
                  <p class="card-text">
                    <strong>Remaining Balance:</strong> {{ debt.remainingAmount.toLocaleString('en-US', { style: 'currency', currency: 'USD' }) }}<br>
                  </p>
                  <p class="card-text">
                    <strong>Type:</strong> {{ debt.type }}
                  </p>
                  <p class="card-text">
                    <strong>Notes:</strong> {{ debt.notes ? debt.notes : 'No current note.' }}
                  </p>
                  <p class="mb-3">
                    <strong>Date Added:</strong> {{ formattedDate(debt.dateAdded) }}
                  </p>
                  <p class="mb-3">
                    <strong>Last Edit:</strong> {{ formattedDate(debt.dateEdited) }}
                  </p>
                  <router-link :to="`/edit-debt/${debt.id}`" class="btn btn-primary btn-sm me-2">Edit</router-link>
                </div>
              </div>
            </div>
          </div>
        </div>
        <div class="tab-pane fade" id="previous-amounts" role="tabpanel" aria-labelledby="previous-amounts-tab">
          <div class="row">
            <div class="col">
              <div v-if="debtPreviousAmounts" class="card">
                <h1 class="card-header">Previous Amounts</h1>
                <div class="card-body">
                  <div v-if="debtPreviousAmounts.length === 0">
                    <span class="list-group-item">No previous amounts available.</span>
                  </div>
                  <div v-else class="scrollable-list">
                    <ul class="list-group">
                      <li v-for="amount in debtPreviousAmounts" :key="amount.id" class="list-group-item">
                        <div class="d-flex justify-content-between">
                          <span>{{ formattedAmount(amount.previousAmount) }}</span>
                          <span>{{ formattedDate(amount.dateAdded) }}</span>
                          <span v-if="amount.percentageChange > 0" class="badge text-bg-danger">
                            {{ formattedPercentage(amount.percentageChange) }}
                          </span>
                          <span v-else-if="amount.percentageChange === 0" class="badge text-bg-secondary">
                            {{ formattedPercentage(amount.percentageChange) }}
                          </span>
                          <span v-else class="badge text-bg-success">
                            {{ formattedPercentage(amount.percentageChange) }}
                          </span>
                        </div>
                      </li>
                    </ul>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
        <div class="tab-pane fade" id="history" role="tabpanel" aria-labelledby="history-tab">
          <div class="row">
            <div class="col">
              <div v-if="debtHistory" class="card">
                <h1 class="card-header">History</h1>
                <div class="card-body">
                  <div v-if="debtHistory.length === 0">
                    <span class="list-group-item">No history available.</span>
                  </div>
                  <div v-else>
                    <ul class="list-group scrollable-list">
                      <li v-for="history in debtHistory" :key="history._id" class="list-group-item">
                        <div class="d-flex justify-content-between">
                          <span>[{{ formattedDate(history.timestamp) }}] {{ history.description }}</span>
                        </div>
                      </li>
                    </ul>
                  </div>
                </div>
              </div>
              <div v-else>
                <p>Loading debt history...</p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div v-else>
      <div class="text-center mt-5 col-md-6 m-auto">
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
import { DEBT_HISTORY_CONTROLLER, DEBT_PREVIOUS_AMOUNT } from '../constants.js';
export default {
  data() {
    return {
      debt: null,
      debtHistory: null,
      debtPreviousAmounts: null,
      isDebt: false,
    };
  },
  async created() {
    const debtId = this.$route.params.id;
    try {
      await this.fetchDebtDetails(debtId);
      await this.fetchDebtHistory(debtId);
      await this.fetchPreviousDebt(debtId);
      this.isDebt = true;
    }
    catch (error) {
      console.error("Error fetching debt details:", error);
    }
  },
  methods: {
    async fetchDebtDetails(debtId) {
      try {
        const response = await axios.get(`${DEBT_CONTROLLER}/${debtId}`);
        this.debt = response.data;
      } catch (error) {
        console.error("Error fetching debt details:", error);
      }
    },
    async fetchDebtHistory(debtId) {
      try {
        const response = await axios.get(`${DEBT_HISTORY_CONTROLLER}/${debtId}`);
        this.debtHistory = response.data;
      } catch (error) {
        this.debtHistory = [];
        console.error("Error fetching debt history:", error);
      }
    },
    async fetchPreviousDebt(debtId) {
      try {
        const response = await axios.get(`${DEBT_PREVIOUS_AMOUNT}/${debtId}`);
        this.debtPreviousAmounts = response.data;
      } catch (error) {
        console.error("Error fetching previous debt amount:", error);
      }
    },
    async renameProperties(debtHistory) {
      const renamedHistory = debtHistory.map((history) => {
        return {
          _id: history._id,
          description: history.description,
          percentageChange: history.percentageChange
        };
      });
      return renamedHistory;
    }
  },
  computed: {
    formattedDate() {
      return function (date) {
        return new Date(date).toLocaleDateString();
      };
    },
    formattedAmount() {
      return function (value) {
        return `$${parseFloat(value).toFixed(2)}`;
      };
    },
    formattedPercentage() {
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