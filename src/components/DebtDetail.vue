<template>
  <div class="container">
    <router-link to="/Debt-Tracker/" class="btn btn-secondary mt-3">Back</router-link>
    <div v-if="isDebt">
      <ul class="nav nav-tabs mt-3" id="debtTabs" role="tablist">
        <li class="nav-item" role="presentation">
          <button class="nav-link active" id="details-tab" data-bs-toggle="tab" data-bs-target="#details" type="button"
            role="tab" aria-controls="details" aria-selected="true">Details</button>
        </li>
        <li class="nav-item" role="presentation">
          <button class="nav-link" id="payments-tab" data-bs-toggle="tab" data-bs-target="#payments" type="button"
            role="tab" aria-controls="payments" aria-selected="true">Payments</button>
        </li>
        <li class="nav-item" role="presentation">
          <button class="nav-link" id="charges-tab" data-bs-toggle="tab" data-bs-target="#charges" type="button"
            role="tab" aria-controls="charges" aria-selected="true">Charges</button>
        </li>
        <li class="nav-item" role="presentation">
          <button class="nav-link" id="previous-amounts-tab" data-bs-toggle="tab" data-bs-target="#previous-amounts"
            type="button" role="tab" aria-controls="previous-amounts" aria-selected="false">Previous Amounts</button>
        </li>
        <li class="nav-item" role="presentation">
          <button class="nav-link" id="previous-percentages-tab" data-bs-toggle="tab"
            data-bs-target="#previous-percentages" type="button" role="tab" aria-controls="previous-percentages"
            aria-selected="false">Previous Percentages</button>
        </li>
        <li class="nav-item" role="presentation">
          <button class="nav-link" id="history-tab" data-bs-toggle="tab" data-bs-target="#history" type="button"
            role="tab" aria-controls="history" aria-selected="false">History</button>
        </li>
      </ul>
      <div class="tab-content debt-list-container" id="debtTabsContent">
        <div class="tab-pane fade show active" id="details" role="tabpanel" aria-labelledby="details-tab">
          <div class="row">
            <div class="col">
              <div v-if="debt" class="card">
                <h1 class="card-header">Details</h1>
                <div class="d-flex d-inline">
                  <div class="dropdown pe-4 ps-2 pt-3">
                    <button class="btn btn-secondary dropdown-toggle" type="button" id="dropdownMenuButton"
                      data-bs-toggle="dropdown" aria-expanded="false">
                      Options
                    </button>
                    <ul class="dropdown-menu" aria-labelledby="dropdownMenuButton">
                      <li>
                        <router-link :to="`/Debt-Tracker/edit-debt/${debt.id}`" class="dropdown-item">Edit</router-link>
                      </li>
                      <li>
                        <router-link :to="`/Debt-Tracker/add-payment/${debt.id}`" class="dropdown-item">Add
                          Payment</router-link>
                      </li>
                      <li>
                        <router-link :to="`/Debt-Tracker/add-interest-charge/${debt.id}`" class="dropdown-item">Add
                          Interest Charge</router-link>
                      </li>
                    </ul>
                  </div>
                  <p class="text-muted mt-3">Progress:</p>
                  <div class="progress w-100 mt-4 me-2" role="progressbar" aria-label="Default striped example"
                    aria-valuenow="0" aria-valuemin="0" aria-valuemax="100">
                    <div class="progress-bar progress-bar-striped progress-bar-animated">
                      <span class="progressCompletionPercentage">
                        0%
                      </span>
                    </div>
                  </div>
                </div>
                <div class="row">
                  <div class="col-md-6">
                    <div class="card-body">
                      <div class="debt-detail d-flex align-items-center">
                        <h5 class="card-title me-2">{{ debt.creditor }}</h5>
                        <span v-if="debt.percentageChange === 0" class="badge bg-secondary">{{ debt.percentageChange
                          }}%</span>
                        <span v-else-if="debt.percentageChange > 0" class="badge bg-danger">+{{ debt.percentageChange
                          }}%</span>
                        <span v-else class="badge bg-success">{{ debt.percentageChange }}%</span>
                      </div>
                      <table class="table table-hover mt-3 debt-details-table">
                        <tbody>
                          <tr>
                            <th>Original Balance:</th>
                            <td>{{ debt.amount.toLocaleString('en-US', { style: 'currency', currency: 'USD' }) }}</td>
                          </tr>
                          <tr>
                            <th>Remaining Balance:</th>
                            <td>{{ debt.remainingAmount.toLocaleString('en-US', { style: 'currency', currency: 'USD' })
                              }}</td>
                          </tr>
                          <tr>
                            <th>Interest:</th>
                            <td>{{ debt.interestRate }}%</td>
                          </tr>
                          <tr>
                            <th>Last Payment:</th>
                            <td>{{ debt.lastPayment.toLocaleString('en-US', { style: 'currency', currency: 'USD' }) }}
                            </td>
                          </tr>
                          <tr>
                            <th>Last Payment Date:</th>
                            <td>{{ formattedDate(debt.lastPaymentDate) }}</td>
                          </tr>
                          <tr>
                            <th>Type:</th>
                            <td>{{ debt.type }}</td>
                          </tr>
                          <tr>
                            <th>Notes:</th>
                            <td>{{ debt.notes ? debt.notes : 'No current note.' }}</td>
                          </tr>
                          <tr>
                            <th>Date Added:</th>
                            <td>{{ formattedDate(debt.dateAdded) }}</td>
                          </tr>
                          <tr>
                            <th>Last Edit:</th>
                            <td>{{ formattedDate(debt.dateEdited) }}</td>
                          </tr>
                        </tbody>
                      </table>
                    </div>
                  </div>
                  <div class="col-md-6">
                    <div class="card-body">
                      <img v-if="debt.image" :src="debt.imageSource" alt="Debt Image" class="img img-fluid">
                      <div v-else class="text-center">
                        <p>No image available.</p>
                      </div>
                    </div>
                  </div>
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
                          <span>{{ formattedAmount(amount.amount) }}</span>
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
        <div class="tab-pane fade" id="payments" role="tabpanel" aria-labelledby="payments-tab">
          <div class="row">
            <div class="col">
              <div class="card">
                <h1 class="card-header">Payments</h1>
                <div class="card-body">
                  <div v-if="payments.length === 0">
                    <span class="list-group-item">No payments available.</span>
                  </div>
                  <div v-else>
                    <ul class="list-group scrollable-list">
                      <li v-for="payment in payments" :key="payment.id" class="list-group-item">
                        <div class="d-flex justify-content-between">
                          <span>{{ formattedDate(payment.date) }}</span>
                          <span>{{ formattedAmount(payment.amount) }}</span>
                          <router-link :to="`/Debt-Tracker/edit-payment/${payment.id}`"
                            class="btn btn-primary btn-sm">Edit</router-link>
                        </div>
                      </li>
                    </ul>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
        <div class="tab-pane fade" id="previous-percentages" role="tabpanel" aria-labelledby="previous-percentages-tab">
          <div class="row">
            <div class="col">
              <div v-if="previousPercentages.length > 0" class="card">
                <h1 class="card-header">Previous Percentages</h1>
                <div class="card-body">
                  <canvas id="percentageChart"></canvas>
                </div>
              </div>
              <div v-else>
                <p>No previous percentages available.</p>
              </div>
            </div>
          </div>
        </div>
        <div class="tab-pane fade" id="charges" role="tabpanel" aria-labelledby="charges-tab">
          <div class="row">
            <div class="col">
              <div class="card">
                <h1 class="card-header">Charges</h1>
                <div class="card-body">
                  <div v-if="charges.length === 0">
                    <span class="list-group-item">No charges available.</span>
                  </div>
                  <div class="scrollable-list" v-else>
                    <table class="table table-hover mt-3">
                      <thead>
                        <tr>
                          <th scope="col">Description</th>
                          <th scope="col">Date</th>
                          <th scope="col">Amount</th>
                          <th scope="col">Options</th>
                        </tr>
                      </thead>
                      <tbody>
                        <tr v-for="charge in charges" :key="charge.id">
                          <td>{{ charge.description }}</td>
                          <td>{{ formattedDate(charge.date) }}</td>
                          <td>{{ formattedAmount(charge.amount) }}</td>
                          <td>
                            <router-link :to="`/Debt-Tracker/edit-interest-charge/${charge.id}`"
                              class="btn btn-primary btn-sm">Edit</router-link>
                          </td>
                        </tr>
                      </tbody>
                    </table>
                  </div>
                </div>
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
import {
  DEBT_CONTROLLER,
  DEBT_HISTORY_CONTROLLER,
  DEBT_PREVIOUS_AMOUNT,
  PAYMENT_CONTROLLER,
  PREVIOUS_PERCENTAGE_CONTROLLER,
  INTEREST_CHARGE_CONTROLLER
} from '../constants.js';
import { Chart } from 'chart.js';

export default {
  data() {
    return {
      debt: null,
      debtHistory: null,
      debtPreviousAmounts: null,
      payments: [],
      isDebt: false,
      previousPercentages: [],
      charges: [],
    };
  },
  async created() {
    const debtId = this.$route.params.id;
    try {
      await this.fetchDebtDetails(debtId);
      await this.fetchDebtHistory(debtId);
      await this.fetchPreviousDebt(debtId);
      await this.fetchPayments(debtId);
      await this.fetchPreviousPercentages(debtId);
      await this.fetchCharges(debtId);
      this.isDebt = true;
      await this.$nextTick();
    } catch (error) {
      console.error("Error fetching debt details:", error);
    } finally {
      await this.calculateProgressPercentage();
      await this.renderChart();
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
    async fetchPayments(debtId) {
      try {
        const response = await axios.get(`${PAYMENT_CONTROLLER}/GetPayments/${debtId}`);
        this.payments = response.data;
      } catch (error) {
        console.error("Error fetching payments:", error);
      }
    },
    async fetchPreviousPercentages(debtId) {
      try {
        const response = await axios.get(`${PREVIOUS_PERCENTAGE_CONTROLLER}/${debtId}`);
        this.previousPercentages = response.data;
      } catch (error) {
        console.error("Error fetching previous percentages:", error);
      }
    },
    async fetchCharges(debtId) {
      try {
        const response = await axios.get(`${INTEREST_CHARGE_CONTROLLER}/${debtId}`);
        this.charges = response.data;
      } catch (error) {
        console.error("Error fetching charges:", error);
      }
    },
    async calculateProgressPercentage() {
      if (this.debt.remainingAmount === this.debt.amount) {
        return 0;
      }

      const percentage = Math.ceil((this.debt.remainingAmount / this.debt.amount) * 100);
      const progressBar = document.querySelector('.progress-bar');
      const progressCompletionPercentage = document.querySelector('.progressCompletionPercentage');
      if (progressBar && progressCompletionPercentage) {
        progressBar.style.width = `${percentage}%`;
        progressBar.setAttribute('aria-valuenow', percentage);
        progressCompletionPercentage.textContent = `${percentage}%`;

        switch (true) {
          case percentage === 100:
            progressBar.classList.remove('bg-warning');
            progressBar.classList.add('bg-success');
            break;
          case percentage > 75:
            progressBar.classList.remove('bg-warning');
            progressBar.classList.add('bg-success');
            break;
          case percentage > 50:
            progressBar.classList.remove('bg-warning');
            progressBar.classList.add('bg-info');
            break;
          case percentage > 25:
            progressBar.classList.remove('bg-warning');
            progressBar.classList.add('bg-warning');
            break;
          default:
            progressBar.classList.remove('bg-warning');
            progressBar.classList.add('bg-danger');
            break;
        }

      } else {
        console.error('Progress bar not found or text element not found');
      }
    },
    async renderChart() {
      const ctx = document.getElementById('percentageChart');
      if (!ctx) {
        console.error('Canvas element not found');
        return;
      }

      const labels = this.previousPercentages.map(p => this.formattedDate(p.date)).reverse();
      const percentageData = this.previousPercentages.map(p => p.percentage).reverse();

      const data = {
        labels: labels,
        datasets: [{
          label: 'Percentage Change',
          data: percentageData,
          fill: false,
          borderColor: 'rgb(75, 192, 192)',
          tension: 0.1,
        }],
      };
      new Chart(ctx, {
        type: 'line',
        data: data,
      });
    }
  },
  computed: {
    formattedDate() {
      return function (date) {
        return new Date(date).toLocaleDateString('en-US');
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
    roudPercentageToWholeNumber(percentage) {
      return Math.ceil(percentage);
    },
  },
};
</script>
