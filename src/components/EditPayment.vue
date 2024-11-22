<template>
    <div class="container">
        <h1 class="my-4">Edit Payment</h1>
        <form @submit.prevent="editPayment">
            <div class="mb-3">
                <label for="amount" class="form-label">Amount</label>
                <input type="number" class="form-control" id="amount" step=".01" v-model="payment.amount" required
                    placeholder="Enter amount" />
            </div>
            <div class="mb-3">
                <label for="date" class="form-label">Date</label>
                <input type="date" class="form-control" id="date" v-model="payment.date" required />
            </div>
            <button type="submit" class="btn btn-primary">Submit</button>
            <router-link :to="`/Debt-Tracker/debt/${payment.debtId}`" class="btn btn-secondary ms-2">Cancel</router-link>
            <button type="button" class="btn btn-danger ms-2 float-end" @click="showDeleteModal">Delete</button>
        </form>
    </div>

    <div class="modal fade" id="deleteModal" tabindex="-1" aria-labelledby="deletePaymentModalLabel"
        aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="deletePaymentModalLabel">Delete Payment</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    Are you sure you want to delete this payment?
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                    <button type="button" class="btn btn-danger" @click="deletePaymentConfirmed">Delete</button>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import axios from 'axios';
import { Modal } from 'bootstrap';
import { PAYMENT_CONTROLLER, DEBT_CONTROLLER } from '../constants';

export default {
    name: 'EditPayment',
    data() {
        return {
            payment: null,
            debt: null,
            debtId: '',
        };
    },
    async created() {
        this.paymentId = this.$route.params.id;
        await this.getPayment();
    },
    methods: {
        async editPayment() {
            try {
                await axios.put(PAYMENT_CONTROLLER, this.payment);
                this.$router.push({ path: `/Debt-Tracker/debt/${this.payment.debtId}` });
            } catch (error) {
                console.error('Error submitting payment:', error);
            }
        },
        async getPayment() {
            try {
                const response = await axios.get(`${PAYMENT_CONTROLLER}/${this.$route.params.id}`);
                this.payment = response.data;
                this.payment.date = this.formatDate(this.payment.date);
                this.debt = await this.getDebt(this.payment.debtId);
            } catch (error) {
                console.error('Error getting payment:', error);
            }
        },
        async getDebt(id) {
            try {
                const response = await axios.get(`${DEBT_CONTROLLER}/${id}`);
                return response.data;
            } catch (error) {
                console.error('Error getting debt:', error);
            }
        },
        formatDate(dateString) {
            const date = new Date(dateString);
            const year = date.getFullYear();
            const month = String(date.getMonth() + 1).padStart(2, '0');
            const day = String(date.getDate()).padStart(2, '0');
            return `${year}-${month}-${day}`;
        },
        showDeleteModal() {
            const modalElement = document.getElementById('deleteModal');
            const modal = new Modal(modalElement);
            modal.show();
        },
        async deletePaymentConfirmed() {
            try {
                await axios.delete(`${PAYMENT_CONTROLLER}/${this.paymentId}`);
                const modalElement = document.getElementById('deleteModal');
                const modal = Modal.getInstance(modalElement);
                modal.hide();
                this.$router.push({ path: `/Debt-Tracker/debt/${this.payment.debtId}` });
            } catch (error) {
                console.error('Error deleting payment:', error);
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