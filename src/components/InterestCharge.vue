<template>
    <div class="container">
        <h1 class="my-4">Add Interest Charge</h1>
        <form @submit.prevent="submitCharge">
            <div class="mb-3">
                <label for="amount" class="form-label">Creditor</label>
                <input type="text" class="form-control" id="creditor" v-model="creditor" readonly />
            </div>
            <div class="mb-3">
                <label for="amount" class="form-label">Amount</label>
                <input type="number" class="form-control" id="amount" step=".01" v-model="amount" required
                    placeholder="Enter amount" />
            </div>
            <div class="mb-3">
                <label for="date" class="form-label">Date</label>
                <input type="date" class="form-control" id="date" v-model="date" required />
            </div>
            <div class="mb-3">
                <label for="debtId" class="form-label">Description</label>
                <select class="form-select" id="description" v-model="description" required>
                    <option value="" disabled>Select description</option>
                    <option value="Interest Charge">Interest Charge</option>
                    <option value="Late Fee">Late Fee</option>
                    <option value="Other">Other</option>
                </select>
            </div>
            <button type="submit" class="btn btn-primary">Submit</button>
            <router-link :to="'/Debt-Tracker/debt/' + debtId" class="btn btn-secondary ms-2">Cancel</router-link>
        </form>
    </div>
</template>

<script>
import axios from 'axios';
import { INTEREST_CHARGE_CONTROLLER, DEBT_CONTROLLER } from '../constants';

export default {
    name: 'InterestCharge',
    data() {
        return {
            amount: '',
            date: '',
            debtId: '',
            description: '',
            creditor: '',
            debt: [],
        };
    },
    created() {
        this.debtId = this.$route.params.id;
        this.fetchDebt(this.debtId);
    },
    methods: {
        async submitCharge() {
            try {
                const payload = {
                    amount: this.amount,
                    date: this.date,
                    debtId: this.debtId,
                    description: this.description,
                    creditor: this.creditor,
                };
                console.log('payload:', payload);
                await axios.post(INTEREST_CHARGE_CONTROLLER, payload);
                this.$router.push({ path: `/Debt-Tracker/debt/${this.debtId}` });
            } catch (error) {
                console.error('Error submitting interest charge:', error);
            }
        },
        async fetchDebt(debtId) {
            try {
                const response = await axios.get(`${DEBT_CONTROLLER}/${debtId}`);
                this.debt = response.data;
                this.creditor = this.debt.creditor;
            } catch (error) {
                console.error('Error fetching debt:', error);
            }
        },
    },
};
</script>
