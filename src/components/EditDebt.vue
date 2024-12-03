<template>
    <div class="container">
        <h1 class="my-4">Edit Debt</h1>
        <div v-if="debt">
            <form @submit.prevent="updateDebt">
                <div class="mb-3">
                    <label for="creditor" class="form-label">Creditor</label>
                    <input type="text" class="form-control" id="creditor" v-model="debt.creditor" required />
                </div>
                <div class="mb-3">
                    <label for="amount" class="form-label">Amount</label>
                    <input type="number" class="form-control" id="amount" step=".01" v-model="debt.remainingAmount"
                        required />
                </div>
                <div class="mb-3">
                    <label for="type" class="form-label">Type</label>
                    <select class="form-select" id="type" v-model="debt.type" required>
                        <option value="Credit Card">Credit Card</option>
                        <option value="Loan">Loan</option>
                        <option value="Other">Other</option>
                    </select>
                </div>
                <div class="mb-3">
                    <label for="notes" class="form-label">Notes</label>
                    <textarea class="form-control" id="notes" v-model="debt.notes"></textarea>
                </div>
                <div class="mb-3">
                    <label for="image" class="form-label">Image</label>
                    <input id="image" type="file" ref="image" class="form-control" accept="image/*" />
                </div>
                <button type="submit" class="btn btn-primary">Save</button>
                <router-link to="/Debt-Tracker/" class="btn btn-secondary ms-2">Cancel</router-link>
                <button type="button" class="btn btn-danger ms-2 float-end" @click="showDeleteModal">Delete</button>
            </form>


            <div class="modal fade" id="deleteModal" tabindex="-1" aria-labelledby="deleteModalLabel"
                aria-hidden="true">
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
            <div class="loader text-center mt-5 col-md-6 m-auto">
                <div class="spinner-border text-primary" role="status">
                    <span class="visually-hidden">Loading...</span>
                </div>
                <p>Loading...</p>
            </div>
        </div>
    </div>
</template>

<script>
import axios from 'axios';
import { Modal } from 'bootstrap';
import { DEBT_CONTROLLER } from '../constants.js';

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
                const response = await axios.get(`${DEBT_CONTROLLER}/${debtId}`);
                this.debt = response.data;
            } catch (error) {
                console.error("Error fetching debt details:", error);
            }
        },
        async updateDebt() {
            try {
                if (this.$refs.image.files[0]) {
                    this.debt.image = await this.convertImageToBase64(this.$refs.image.files[0]);
                }
                await axios.put(`${DEBT_CONTROLLER}/${this.debt.id}`, this.debt);
                this.$router.push({ path: `/Debt-Tracker/debt/${this.debt.id}` });
            } catch (error) {
                console.error("Error updating debt:", error.message);
                this.$router.push({ path: '/' });
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
        showDeleteModal() {
            const modalElement = document.getElementById('deleteModal');
            const modal = new Modal(modalElement);
            modal.show();
        },
        async deleteDebt() {
            try {
                const debtId = this.$route.params.id;
                await axios.delete(`https://localhost:7164/Debt/${debtId}`);

                document.querySelector('.modal-backdrop').remove();
                this.$router.push({ path: '/Debt-Tracker/' });

            } catch (error) {
                console.error("Error deleting debt!", error);
                this.$router.push({ path: '/Debt-Tracker/' });
            }
        },

        calculatePercentageChange(originalAmount, newAmount) {
            if (originalAmount === 0) return 0;
            return ((newAmount - originalAmount) / originalAmount) * 100;
        },
    },
};
</script>
