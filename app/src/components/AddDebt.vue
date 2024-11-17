<template>
  <div class="container">
    <div v-if="errorMessage" class="alert alert-danger alert-dismissible fade show mt-2" role="alert">
      {{ errorMessage }}
      <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close" @click="clearMessage"></button>
    </div>
    <h2 class="my-4">Add Debt</h2>
    <form @submit.prevent="addDebt">
      <div class="mb-3">
        <label for="creditor" class="form-label">Creditor</label>
        <input id="creditor" v-model="creditor" type="text" class="form-control" placeholder="Creditor Name" required />
      </div>
      <div class="mb-3">
        <label for="amount" class="form-label">Amount</label>
        <input id="amount" v-model.number="amount" type="number" class="form-control" placeholder="Amount" required />
      </div>
      <div class="mb-3">
        <label for="notes" class="form-label">Notes</label>
        <textarea id="notes" v-model="notes" class="form-control" placeholder="Notes"></textarea>
      </div>
      <button type="submit" class="btn btn-primary">Add Debt</button>
    </form>
    <router-link to="/" class="btn btn-secondary mt-2">Go Back</router-link>
  </div>
</template>

<script>
import axios from 'axios';
import { EventBus } from '../EventBus';

export default {
  data() {
    return {
      creditor: '',
      amount: null,
      notes: '',
      errorMessage: null,
    };
  },
  methods: {
    async addDebt() {
      try {
        const response = await axios.post('http://localhost:5000/api/debts', {
          creditor: this.creditor,
          amount: this.amount,
          notes: this.notes,
        });
        console.log(response);
        if (response.status === 201) {
          this.$emit('debtAdded', response.data);
          EventBus.successMessage = 'Debt added successfully!';
          this.$router.push('/');
        } else {
          this.errorMessage = 'Failed to add debt!';
          this.clearMessage();
        }
      } catch (error) {
        this.errorMessage = 'Failed to add debt! Please try again. ' + error.message;
        this.clearMessage();
      }
    },
    clearMessage() {
      setTimeout(() => {
        this.errorMessage = null;
      }, 5000);
    }
  },
};
</script>