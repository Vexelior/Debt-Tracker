<template>
    <div class="container">
        <h1 class="my-4">Submit Payment</h1>
        <form @submit.prevent="submitPayment">
            <div class="mb-3">
                <label for="amount" class="form-label">Amount</label>
                <input type="number" class="form-control" id="amount" step=".01" v-model="amount" required
                    placeholder="Enter amount" />
            </div>
            <div class="mb-3">
                <label for="date" class="form-label">Date</label>
                <input type="date" class="form-control" id="date" v-model="date" required />
            </div>
            <button type="submit" class="btn btn-primary">Submit</button>
            <router-link :to="'/Debt-Tracker/debt/' + debtId" class="btn btn-secondary ms-2">Cancel</router-link>
        </form>
    </div>
</template>

<script>
import axios from 'axios';
import { PAYMENT_CONTROLLER } from '../constants';

export default {
    name: 'SubmitPayment',
    data() {
        return {
            amount: '',
            date: '',
            debtId: '',
        };
    },
    created() {
        this.debtId = this.$route.params.id;
    },
    methods: {
        async submitPayment() {
            try {
                const payload = {
                    amount: this.amount,
                    date: this.date,
                    debtId: this.debtId,
                };
                await axios.post(PAYMENT_CONTROLLER, payload);
                this.$router.push({ path: `/Debt-Tracker/debt/${this.debtId}` });
            } catch (error) {
                console.error('Error submitting payment:', error);
            }
        },
        resetForm() {
            this.payment = {
                amount: '',
                date: '',
                method: '',
            };
        },
    },
};
</script>
