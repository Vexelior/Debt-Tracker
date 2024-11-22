<template>
  <div class="container">
    <h1 class="my-4">Graphs</h1>
    <div v-if="loading">
      <div class="text-center mt-5 col-md-6 m-auto">
        <div class="spinner-border text-primary" role="status">
          <span class="visually-hidden">Loading...</span>
        </div>
        <p>Loading...</p>
      </div>
    </div>
    <div v-if="debts.length > 0">
      <div class="row">
        <div class="col-md-6">
          <h3 class="mb-2 text-center text-underline">Debt Distribution by Creditor</h3>
          <canvas id="pieChart" width="400" height="400"></canvas>
        </div>
        <div class="col-md-6 mb-5">
          <h3 class="mb-2 text-center">Debt Amounts by Creditor</h3>
          <canvas id="barChart" width="400" height="400"></canvas>
        </div>
      </div>
    </div>
    <div v-else class="mt-5">
      No debts found to display graphs.
    </div>
  </div>
</template>

<script>
import axios from 'axios';
import { Chart } from 'chart.js/auto';
import { DEBT_CONTROLLER } from '../constants.js';

export default {
  data() {
    return {
      debts: [],
      loading: true,
    };
  },
  async created() {
    try {
      const response = await axios.get(DEBT_CONTROLLER);
      this.debts = response.data;
      await this.$nextTick();
    } catch (error) {
      console.error("Error fetching debts:", error);
    } finally {
      this.loading = false;
      await this.renderCharts();
    }
  },
  methods: {
    async renderCharts() {
      const pieChartData = {
        labels: this.debts.map(debt => debt.creditor),
        datasets: [{
          data: this.debts.map(debt => debt.amount),
          backgroundColor: ['#FF6384', '#36A2EB', '#FFCE56', '#FF5733', '#C70039'],
        }],
      };
      const barChartData = {
        labels: this.debts.map(debt => debt.creditor),
        datasets: [{
          label: 'Debt Amount (' + this.debts.reduce((total, debt) => total + debt.amount, 0).toLocaleString('en-US', { style: 'currency', currency: 'USD' }) + ')',
          data: this.debts.map(debt => debt.amount),
          backgroundColor: '#36A2EB',
          borderColor: '#1e90ff',
          borderWidth: 1,
        }],
      };
      new Chart(document.getElementById('pieChart'), {
        type: 'pie',
        data: pieChartData,
        options: {
          responsive: true,
          plugins: {
            legend: {
              position: 'top',
            },
            tooltip: {
              callbacks: {
                label: function (tooltipItem) {
                  return `${tooltipItem.label}: ${tooltipItem.raw.toLocaleString('en-US', { style: 'currency', currency: 'USD' })}`;
                },
              },
            },
          },
        },
      });
      new Chart(document.getElementById('barChart'), {
        type: 'bar',
        data: barChartData,
        options: {
          responsive: true,
          scales: {
            x: {
              beginAtZero: true,
            },
            y: {
              beginAtZero: true,
            },
          },
          plugins: {
            tooltip: {
              callbacks: {
                label: function (tooltipItem) {
                  return `${tooltipItem.raw.toLocaleString('en-US', { style: 'currency', currency: 'USD' })}`;
                },
              },
            },
          },
        },
      });
    },
  },
};
</script>