<template>
    <div class="container">
      <h2 class="my-4">Edit Debt</h2>
      <div v-if="debt">
        <form @submit.prevent="updateDebt">
          <div class="mb-3">
            <label for="creditor" class="form-label">Creditor</label>
            <input type="text" class="form-control" id="creditor" v-model="debt.creditor" required />
          </div>
          <div class="mb-3">
            <label for="amount" class="form-label">Amount</label>
            <input type="number" class="form-control" id="amount" v-model="debt.amount" required />
          </div>
          <button type="submit" class="btn btn-primary">Save Changes</button>
          <router-link to="/" class="btn btn-secondary ms-2">Cancel</router-link>
        </form>
  
        <button class="btn btn-danger mt-4" @click="showDeleteModal">Delete Debt</button>
  
        <div class="modal fade" id="deleteModal" tabindex="-1" aria-labelledby="deleteModalLabel" aria-hidden="true">
          <div class="modal-dialog">
            <div class="modal-content">
              <div class="modal-header">
                <h5 class="modal-title" id="deleteModalLabel">Confirm Deletion</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
              </div>
              <div class="modal-body">
                Are you sure you want to delete this debt? This action cannot be undone.
              </div>
              <div class="modal-footer">
                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                <button type="button" class="btn btn-danger" @click="deleteDebt">Delete</button>
              </div>
            </div>
          </div>
        </div>
      </div>
      <div v-else>
        <p>Loading debt information...</p>
      </div>
    </div>
  </template>
  
  <script>
  import axios from 'axios';
  import { Modal } from 'bootstrap';
  
  export default {
    data() {
      return {
        debt: null,
      };
    },
    async created() {
      const debtId = this.$route.params.id;
      await this.fetchDebtDetails(debtId);
    },
    methods: {
      async fetchDebtDetails(debtId) {
        try {
          const response = await axios.get(`http://localhost:5000/api/debts/${debtId}`);
          this.debt = response.data;
        } catch (error) {
          console.error("Error fetching debt details:", error);
        }
      },
      async updateDebt() {
        try {
          const debtId = this.$route.params.id;
          await axios.put(`http://localhost:5000/api/debts/${debtId}`, this.debt);
          this.$router.push("/"); 
        } catch (error) {
          console.error("Error updating debt:", error);
        }
      },
      showDeleteModal() {
        const modalElement = document.getElementById('deleteModal');
        const modal = new Modal(modalElement);
        modal.show();
      },
      async deleteDebt() {
        try {
          const debtId = this.$route.params.id;
          await axios.delete(`http://localhost:5000/api/debts/${debtId}`);
          document.querySelector('.modal-backdrop').remove();
          this.$router.push("/");
        } catch (error) {
          console.error("Error deleting debt:", error);
        }
      },
    },
  };
  </script>
  