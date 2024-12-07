<template>
  <div class="container">
    <h1 class="my-4">New Debt</h1>
    <!--ERROR MESSAGES-->
    <div class="alert alert-warning alert-dismissible" role="alert" v-if="error">
      {{ error }}
      <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
    </div>
    <div class="alert alert-success alert-dismissible" role="alert" v-if="success">
      {{ success }}
      <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
    </div>
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
        <input id="interest" v-model.number="interestRate" type="number" class="form-control" placeholder="Interest"
          step=".01" required />
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

      <p class="text-muted">Select an image from the dropdown menu or upload a new image by clicking the button below.</p>

      <div class="mb-3 d-flex justify-content-start">
        <button type="button" class="btn btn-secondary btn-sm" @click="toggleFileInput">
          {{ showFileInput ? 'Hide' : 'Show' }} File Upload
        </button>
      </div>

      <div v-if="showFileInput == false" class="mb-3">
        <label for="type" class="form-label">Image</label>
        <select class="form-select" v-model="image" id="image" required>
          <option value="" disabled selected>-- Select Type --</option>
          <option v-for="image in allImages" :key="image.id" :value="image.source">{{ image.name }}</option>
        </select>
      </div>

      <div v-if="showFileInput" class="d-flex justify-content-start mb-3">
        <button type="button" class="btn btn-primary btn-sm me-2" @click="addImage">Upload</button>
        <div class="form-file w-50">
          <input id="image" ref="image" type="file" class="form-file-input w-100" accept="image/*" required />
        </div>
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
import { DEBT_CONTROLLER, IMAGE_CONTROLLER } from '../constants.js';

export default {
  data() {
    return {
      creditor: '',
      amount: null,
      notes: '',
      type: '',
      image: null,
      interestRate: null,
      imageFile: null,
      allImages: [],
      error: null,
      success: null,
      showFileInput: false,
    };
  },
  created() {
    this.getImages();
  },
  methods: {
    async addDebt() {
      try {
        const image = this.allImages.find((img) => img.source === this.image);
        if (image === undefined) {
          alert('Please select an image to upload.');
          return;
        }
        const payload = {
          creditor: this.creditor,
          amount: this.amount,
          interestRate: this.interestRate,
          notes: this.notes,
          type: this.type,
          image: image.name,
        };
        const response = await axios.post(DEBT_CONTROLLER, payload);
        this.$router.push(`/Debt-Tracker/debt/${response.data.id}`);
      } catch (error) {
        console.error('Error adding debt:', error);
      }
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
  },
};
</script>