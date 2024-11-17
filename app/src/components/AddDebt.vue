<template>
  <div class="container">
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
    };
  },
  methods: {
    async addDebt() {
      const response = await axios.post('http://localhost:5000/api/debts', {
        creditor: this.creditor,
        amount: this.amount,
      });
      this.$emit('debtAdded', response.data); // Emit the event after adding a debt
      this.creditor = '';
      this.amount = null;
      EventBus.successMessage = 'Debt added successfully!';
      this.$router.push('/');
    },
  },
};
</script>
