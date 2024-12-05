<template>
    <div class="container">
        <h1 class="my-4">Register</h1>
        <p v-if="error" class="text-danger">{{ error }}</p>
        <form @submit.prevent="register">
            <div class="form-group">
                <label for="email">Email</label>
                <input class="form-control" v-model="email" type="email" id="email" required />
                <span v-if="emailError">{{ emailError }}</span>
            </div>
            <div class="form-group">
                <label for="password">Password</label>
                <input class="form-control" v-model="password" type="password" id="password" required />
                <small class="text-muted">Password must be at least 8 characters long and contain at least one uppercase
                    letter, one lowercase letter, one number, and one special character.</small>
            </div>
            <button class="btn btn-primary mt-2" type="submit">Register</button>
            <p class="mt-5">Already have an account? <router-link to="/Debt-Tracker/login">Login</router-link></p>
        </form>
    </div>
</template>

<script>
import axios from 'axios';
import { ACCOUNT_CONTROLLER } from '../constants.js';

export default {
    data() {
        return {
            email: '',
            password: '',
            error: null,
            emailError: null,
        };
    },
    created() {
        this.checkIfLoggedIn();
    },
    methods: {
        async register() {
            try {
                await axios.post(`${ACCOUNT_CONTROLLER}/Register`, {
                    email: this.email,
                    password: this.password,
                });
                this.$router.push('/Debt-Tracker/login?registered=true');
            } catch (error) {
                this.error = error.response?.data?.message || 'An error occurred.';
                document.querySelector('button[type="submit"]').disabled = false;
                document.querySelector('button[type="submit"]').textContent = 'Register';
                setTimeout(() => {
                    this.error = null;
                }, 5000);
            }
        },
        async checkIfLoggedIn() {
            try {
                const token = localStorage.getItem('token');
                if (token) {
                    this.$router.push('/Debt-Tracker/');
                }
            } catch (error) {
                console.log(error);
            }
        },
    },
};
</script>