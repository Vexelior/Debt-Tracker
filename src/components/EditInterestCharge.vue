<template>
    <div class="container">
        <h1 class="my-4">Edit Interest Charge</h1>
        <form v-if="charge" @submit.prevent="updateInterestCharge">
            <input type="hidden" v-model="charge.debtId" />
            <input type="hidden" v-model="charge.id" />
            <div class="mb-3">
                <label for="creditor" class="form-label">Creditor</label>
                <input type="text" class="form-control" id="creditor" v-model="charge.creditor" required
                    placeholder="Enter creditor" />
            </div>
            <div class="mb-3">
                <label for="amount" class="form-label">Amount</label>
                <input type="number" class="form-control" id="amount" step=".01" v-model="charge.amount" required
                    placeholder="Enter amount" />
            </div>
            <div class="mb-3">
                <label for="date" class="form-label">Date</label>
                <input type="date" class="form-control" id="date" v-model="charge.date" required />
            </div>
            <div class="mb-3">
                <label for="description" class="form-label">Description</label>
                <select class="form-select" id="description" v-model="charge.description" required>
                    <option value="" disabled>Select Description</option>
                    <option value="Interest Charge">Interest Charge</option>
                    <option value="Late Fee">Late Fee</option>
                    <option value="Other">Other</option>
                </select>
            </div>
            <button type="submit" class="btn btn-primary">Submit</button>
            <router-link :to="`/Debt-Tracker/debt/${charge.debtId}`" class="btn btn-secondary ms-2">Cancel</router-link>
            <button type="button" class="btn btn-danger ms-2 float-end" @click="showDeleteModal">Delete</button>
        </form>
        <div v-else>
            <div class="loader text-center mt-5 col-md-6 m-auto">
                <div class="spinner-border text-primary" role="status">
                    <span class="visually-hidden">Loading...</span>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="deleteModal" tabindex="-1" aria-labelledby="deleteInterestChargeModalLabel"
        aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="deleteInterestChargeModalLabel">Delete</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    Are you sure you want to delete this interest charge?
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                    <button type="button" class="btn btn-danger" @click="deleteInterestChargeConfirmed">Delete</button>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import axios from 'axios';
import { Modal } from 'bootstrap';
import { INTEREST_CHARGE_CONTROLLER } from '../constants';

export default {
    name: 'EditInterestCharge',
    props: {
        id: {
            type: String,
            required: true,
        },
    },
    data() {
        return {
            charge: null,
        };
    },
    async created() {
        await this.fetchInterestCharge(this.id); 
    },
    methods: {
        async fetchInterestCharge(chargeId) {
            try {
                const response = await axios.get(`${INTEREST_CHARGE_CONTROLLER}/GetById/${chargeId}`);
                this.charge = response.data;
                this.charge.date = this.charge.date.split('T')[0];
            } catch (error) {
                console.error('Error fetching interest charge:', error);
            }
        },
        async updateInterestCharge() {
            try {
                await axios.put(`${INTEREST_CHARGE_CONTROLLER}/${this.charge.id}`, this.charge);
                this.$router.push({ path: `/Debt-Tracker/debt/${this.charge.debtId}` });
            } catch (error) {
                console.error("Error updating interest charge:", error);
            }
        },
        showDeleteModal() {
            const modalElement = document.getElementById('deleteModal');
            const modal = new Modal(modalElement);
            modal.show();
        },
        async deleteInterestChargeConfirmed() {
            try {
                await axios.delete(`${INTEREST_CHARGE_CONTROLLER}/${this.id}`);

                const modalElement = document.getElementById('deleteModal');
                const modal = Modal.getInstance(modalElement);
                modal.hide();
                
                this.$router.push({ path: `/Debt-Tracker/debt/${this.charge.debtId}` });
            } catch (error) {
                console.error('Error deleting interest charge:', error);
            }
        },
    },
};
</script>