<template>
  <div class="container">
    <h1 class="my-4">New Debt</h1>
    <form @submit.prevent="addDebt">
      <div class="mb-3">
        <label for="creditor" class="form-label">Creditor</label>
        <input id="creditor" v-model="creditor" type="text" class="form-control" placeholder="Creditor" required />
      </div>
      <div class="mb-3">
        <label for="amount" class="form-label">Amount</label>
        <input id="amount" v-model.number="amount" type="number" class="form-control" placeholder="Amount" step=".01"
          required />
      </div>
      <div class="mb-3">
        <label for="interest" class="form-label">Interest</label>
        <input id="interest" v-model.number="interestRate" type="number" class="form-control" placeholder="Interest" step=".01"
          required />
      </div>
      <div class="mb-3">
        <label for="type" class="form-label">Debt Type</label>
        <select id="type" v-model="type" class="form-select" required>
          <option value="" disabled>-- Select Type --</option>
          <option value="Credit Card">Credit Card</option>
          <option value="Loan">Loan</option>
          <option value="Other">Other</option>
        </select>
      </div>
      <div class="mb-3">
        <label for="notes" class="form-label">Notes</label>
        <textarea id="notes" v-model="notes" class="form-control" placeholder="Notes"></textarea>
      </div>
      <div class="mb-3">
        <label for="image" class="form-label">Image</label>
        <input id="image" type="file" ref="image" class="form-control" accept="image/*" />
      </div>
      <div class="d-flex justify-content-start">
        <button type="submit" class="btn btn-primary me-2">Submit</button>
        <router-link to="/Debt-Tracker/" class="btn btn-secondary">Back</router-link>
      </div>
    </form>
  </div>
</template>

<script>
import axios from 'axios';
import { DEBT_CONTROLLER } from '../constants.js';

export default {
  data() {
    return {
      creditor: '',
      amount: null,
      notes: '',
      type: '',
      image: null,
      interestRate: null,
    };
  },
  methods: {
    async addDebt() {
      try {
        let imageFile = null;
        if (this.$refs.image.files[0]) {
          imageFile = await this.convertImageToBase64(this.$refs.image.files[0]);
        }
        const payload = {
          creditor: this.creditor,
          amount: this.amount,
          interestRate: this.interestRate,
          notes: this.notes,
          type: this.type,
          image: imageFile,
        };
        const response = await axios.post(DEBT_CONTROLLER, payload);
        this.$router.push(`/Debt-Tracker/debt/${response.data.id}`);
      } catch (error) {
        console.error('Error adding debt:', error);
      }
    },
    convertImageToBase64(imageFile) {
      return new Promise((resolve, reject) => {
        const reader = new FileReader();
        reader.onload = () => resolve(reader.result);
        reader.onerror = error => reject(error);
        reader.readAsDataURL(imageFile);
      });
    },
  },
};
</script>