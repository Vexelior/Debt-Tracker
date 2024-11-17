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
                <div class="mb-3">
                    <label for="notes" class="form-label">Notes</label>
                    <textarea class="form-control" id="notes" v-model="debt.notes"></textarea>
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
import { EventBus } from '../EventBus';

export default {
    data() {
        return {
            debt: null,
            originalAmount: null,  // Store original amount to calculate percentage change
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
                this.originalAmount = this.debt.amount;  // Store original debt amount
            } catch (error) {
                console.error("Error fetching debt details:", error);
            }
        },
        async updateDebt() {
            try {
                const debtId = this.$route.params.id;
                // Calculate percentage change
                const percentageChange = this.calculatePercentageChange(this.originalAmount, this.debt.amount);
                this.debt.percentageChange = percentageChange;  // Store percentage change in debt object

                // Update debt in the database
                await axios.put(`http://localhost:5000/api/debts/${debtId}`, this.debt);

                // Use EventBus to show success message and reset URL without query parameters
                EventBus.successMessage = 'Debt updated successfully!';
                this.$router.push({ path: '/' });

            } catch (error) {
                EventBus.errorMessage = 'Failed to update debt! - ' + error.message;
                console.error("Error updating debt:", error);
                this.$router.push({ path: '/' });
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

                // Show success message and reset URL
                EventBus.successMessage = 'Debt deleted successfully!';
                document.querySelector('.modal-backdrop').remove();
                this.$router.push({ path: '/' });

            } catch (error) {
                console.error("Error deleting debt!");
                EventBus.errorMessage = 'Failed to delete debt! - ' + error.message;
                this.$router.push({ path: '/' });
            }
        },

        calculatePercentageChange(originalAmount, newAmount) {
            if (originalAmount === 0) return 0;
            return ((newAmount - originalAmount) / originalAmount) * 100;
        },
    },
};
</script>
