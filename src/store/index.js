import { createStore } from 'vuex';
import axios from 'axios';

const store = createStore({
    state: {
        token: localStorage.getItem('token') || null,
        user: null,
    },
    mutations: {
        setToken(state, token) {
            state.token = token;
            if (token) {
                localStorage.setItem('token', token);
            } else {
                localStorage.removeItem('token');
            }
        },
        setUser(state, user) {
            state.user = user;
        },
    },
    actions: {
        async checkToken({ commit }) {
            const token = localStorage.getItem('token');
            if (token) {
                try {
                    const response = await axios.get('/auth/validate-token', {
                        headers: { Authorization: `Bearer ${token}` },
                    });
                    commit('setToken', token);
                    commit('setUser', response.data.user);
                } catch (error) {
                    commit('setToken', null);
                }
            } else {
                commit('setToken', null);
            }
        },
    },
    getters: {
        isAuthenticated(state) {
            return !!state.token;
        },
    },
});

export default store;
