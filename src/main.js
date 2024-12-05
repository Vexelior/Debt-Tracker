import './assets/main.css'
import 'bootstrap/dist/js/bootstrap';
import 'bootstrap/dist/css/bootstrap.css'
import './plugins/axios';
import store from './store';

import { createApp } from 'vue'
import App from './App.vue'
import router from './router'

document.addEventListener('click', function () {
    document.querySelector('.navbar-collapse').classList.remove('show');
});

document.addEventListener('click', function (e) {
    if (e.target.type === 'submit') {
        const btn = e.target;
        const originalText = btn.innerHTML;
        btn.form.onsubmit = function () {
            try {
                if (btn.form.checkValidity()) {
                    btn.innerHTML = `<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Loading...`;
                    btn.disabled = true;
                    return true;
                } else {
                    btn.innerHTML = originalText;
                    btn.disabled = false;
                    return false;
                }
            } catch (e) {
                alert('An error occurred. Please try again.');
                console.error(e);
                return false;
            }
        };
    }
});

const app = createApp(App)
app.use(router)
app.use(store);
app.mount('#app')
