<template>
    <div class="container">
      <h2 class="my-4">Debt Summary</h2>
  
      <!-- Success/Error messages -->
      <div v-if="successMessage" class="alert alert-success alert-dismissible fade show mt-2" role="alert">
        {{ successMessage }}
        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close" @click="clearMessage"></button>
      </div>
  
      <div v-if="errorMessage" class="alert alert-danger alert-dismissible fade show mt-2" role="alert">
        {{ errorMessage }}
        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close" @click="clearMessage"></button>
      </div>
  
      <!-- Pie Chart for Debt Distribution -->
      <div class="row">
        <div class="col-md-6">
          <h3>Debt Distribution by Creditor</h3>
          <canvas id="pieChart" width="400" height="400"></canvas>
        </div>
  
        <!-- Bar Chart for Debt by Amount -->
        <div class="col-md-6">
          <h3>Debt Amounts by Creditor</h3>
          <canvas id="barChart" width="400" height="400"></canvas>
        </div>
      </div>
    </div>
  </template>
  
  <script>
  // Import necessary Chart.js components
  import axios from 'axios';
  import { Chart } from 'chart.js/auto';
  
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
        // Fetch debts from the API
        const response = await axios.get('http://localhost:5000/api/debts');
        this.debts = response.data;
        this.renderCharts();  // Render the charts after fetching data
      } catch (error) {
        this.errorMessage = "Failed to load debts.";
        console.error("Error fetching debts:", error);
      }
  
      // Handling success and error messages passed via query parameters
      this.successMessage = this.$route.query.successMessage;
      this.errorMessage = this.$route.query.errorMessage;
      if (this.successMessage || this.errorMessage) {
        setTimeout(() => {
          this.clearMessage();
        }, 5000);
      }
    },
    methods: {
      renderCharts() {
        // Pie Chart Data
        const pieChartData = {
          labels: this.debts.map(debt => debt.creditor),
          datasets: [{
            data: this.debts.map(debt => debt.amount),
            backgroundColor: ['#FF6384', '#36A2EB', '#FFCE56', '#FF5733', '#C70039'], // Customize colors
          }],
        };
  
        // Bar Chart Data
        const barChartData = {
          labels: this.debts.map(debt => debt.creditor),
          datasets: [{
            label: 'Debt Amount',
            data: this.debts.map(debt => debt.amount),
            backgroundColor: '#36A2EB',
            borderColor: '#1e90ff',
            borderWidth: 1,
          }],
        };
  
        // Pie Chart Configuration
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
                  label: function(tooltipItem) {
                    return `${tooltipItem.label}: $${tooltipItem.raw}`;
                  },
                },
              },
            },
          },
        });
  
        // Bar Chart Configuration
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
                  label: function(tooltipItem) {
                    return `$${tooltipItem.raw}`;
                  },
                },
              },
            },
          },
        });
      },
  
      clearMessage() {
        this.successMessage = null;
        this.errorMessage = null;
      },
    },
  };
  </script>
  
  <style scoped>
  /* Custom styles for the chart container */
  canvas {
    max-width: 100%;
    height: auto;
  }
  </style>
  