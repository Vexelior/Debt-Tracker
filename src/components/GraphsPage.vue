<template>
  <div class="container">
    <div v-if="debts.length === 0" class="alert alert-info mt-2" role="alert">
      No debts found.
    </div>
    <div class="row mt-5">
      <div class="col-md-6">
        <h3 class="mb-2 text-center text-underline"><u>Debt Distribution by Creditor</u></h3>
        <canvas id="pieChart" width="400" height="400"></canvas>
      </div>
      <div class="col-md-6">
        <h3 class="mb-2 text-center"><u>Debt Amounts by Creditor</u></h3>
        <canvas id="barChart" width="400" height="400"></canvas>
      </div>
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
      successMessage: null,
      errorMessage: null,
    };
  },
  async created() {
    try {
      const response = await axios.get(DEBT_CONTROLLER);
      this.debts = response.data;
      this.renderCharts();
    } catch (error) {
      this.errorMessage = "Failed to load debts.";
      console.error("Error fetching debts:", error);
    }
  },
  methods: {
    renderCharts() {
      const pieChartData = {
        labels: this.debts.map(debt => debt.creditor),
        datasets: [{
          data: this.debts.map(debt => debt.amount),
          backgroundColor: ['#FF6384', '#36A2EB', '#FFCE56', '#FF5733', '#C70039'], // Customize colors
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