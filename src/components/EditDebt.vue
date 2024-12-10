<template>
    <div class="container">
        <h1 class="my-4">Edit Debt</h1>
        <!--ERROR MESSAGES-->
        <div class="alert alert-warning alert-dismissible" role="alert" v-if="error">
            {{ error }}
            <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
        </div>
        <div class="alert alert-success alert-dismissible" role="alert" v-if="success">
            {{ success }}
            <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
        </div>
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
                        <option value="" disabled>-- Select Type --</option>
                        <option value="Credit Card">Credit Card</option>
                        <option value="Loan">Loan</option>
                        <option value="Other">Other</option>
                    </select>
                </div>
                <div class="mb-3">
                    <label for="notes" class="form-label">Notes</label>
                    <textarea class="form-control" id="notes" v-model="debt.notes"></textarea>
                </div>
                <p class="text-muted">Select an image from the dropdown menu or upload a new image by clicking the
                    button below.</p>
                <div class="mb-3 d-flex justify-content-start">
                    <button type="button" class="btn btn-secondary btn-sm" @click="toggleFileInput">
                        {{ showFileInput ? 'Hide' : 'Show' }} File Upload
                    </button>
                </div>
                <div v-if="showFileInput == false" class="mb-3">
                    <label for="type" class="form-label">Image</label>
                    <select class="form-select" v-model="image" id="image" required @change="onImageChange">
                        <option value="" disabled selected>-- Select Type --</option>
                        <option v-for="image in allImages" :key="image.id" :value="image.source">{{ image.name }}
                        </option>
                    </select>
                </div>
                <div v-if="showFileInput" class="d-flex justify-content-start mb-3">
                    <button type="button" class="btn btn-primary btn-sm me-2" @click="addImage">Upload</button>
                    <div class="form-file w-50">
                        <input id="image" ref="image" type="file" class="form-file-input w-100" accept="image/*"
                            required />
                    </div>
                </div>
                <div class="mb-3">
                    <img v-if="image" :src="image" alt="Selected Image" class="img img-fluid" width="250px" />
                </div>
                <button type="submit" class="btn btn-primary">Save</button>
                <router-link :to="'/Debt-Tracker/debt/' + debt.id" class="btn btn-secondary ms-2">Cancel</router-link>
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
import { DEBT_CONTROLLER, IMAGE_CONTROLLER } from '../constants.js';

export default {
    data() {
        return {
            debt: null,
            image: null,
            allImages: [],
            showFileInput: false,
            error: null,
            success: null,
        };
    },
    async created() {
        const debtId = this.$route.params.id;
        this.fetchDebtDetails(debtId);
        this.getImages();
    },
    methods: {
        async fetchDebtDetails(debtId) {
            try {
                const response = await axios.get(`${DEBT_CONTROLLER}/${debtId}`);
                this.debt = response.data;
                this.image = this.debt.imageSource;
            } catch (error) {
                console.error("Error fetching debt details:", error);
            }
        },
        async updateDebt() {
            try {
                const image = this.allImages.find((img) => img.source === this.image);
                if (image === undefined) {
                    alert('Please select an image to upload.');
                    return;
                }
                this.debt.image = image.name;
                await axios.put(`${DEBT_CONTROLLER}/${this.debt.id}`, this.debt);
                this.$router.push({ path: `/Debt-Tracker/debt/${this.debt.id}` });
            } catch (error) {
                console.error("Error updating debt:", error.message);
                this.$router.push({ path: '/' });
            }
        },
        async getImages() {
            try {
                const response = await axios.get(IMAGE_CONTROLLER);

                if (this.allImages.length > 0) {
                    this.allImages = [];
                }

                this.allImages = response.data;
            } catch (error) {
                console.error('Error getting images:', error);
            }
        },
        async onImageChange() {
            const image = this.allImages.find((img) => img.source === this.image);
            if (image === undefined) {
                return;
            }
            this.image = image.source;
        },
        async addImage() {
            try {
                const imageFile = this.$refs.image.files[0];
                if (imageFile === undefined) {
                    alert('Please select an image to upload.');
                    return;
                }
                const imageBase64 = await this.convertImageToBase64(imageFile);
                const payload = {
                    name: imageFile.name,
                    source: imageBase64,
                };
                await axios.post(IMAGE_CONTROLLER, payload);
                await this.getImages();
                this.showSuccess('Image uploaded successfully! You can now select the image from the dropdown menu.');
            } catch (error) {
                if (error.response.data.message) {
                    this.showError(error.response.data.message);
                    return;
                }
                console.error('Error adding image:', error);
            }
        },
        showError(error) {
            this.error = error;
            setTimeout(() => {
                this.error = null;
            }, 5000);
        },
        showSuccess(success) {
            this.success = success;
            setTimeout(() => {
                this.success = null;
            }, 5000);
        },
        toggleFileInput() {
            this.showFileInput = !this.showFileInput;
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
