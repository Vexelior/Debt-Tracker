<template>
    <div v-if="loading">
    <div class="text-center mt-5 col-md-6 m-auto">
      <div class="spinner-border text-primary" role="status">
        <span class="visually-hidden">Loading...</span>
      </div>
      <p>Loading...</p>
    </div>
  </div>
  <div v-else class="container">
    <h1>Logs</h1>
    <div v-if="logs.length > 0">
      <div class="row scrollable-list-large">
        <div class="col-md-12 mb-4 mt-4">
          <div class="table-responsive">
            <table class="table table-bordered">
              <thead>
                <tr>
                  <th>Level</th>
                  <th>Timestamp</th>
                  <th>Message</th>
                  <th>Trace</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="log in logs" :key="log.id">
                  <td>{{ log.logLevel }}</td>
                  <td>{{ new Date(log.timestamp).toLocaleDateString() }}</td>
                  <td>{{ log.message }}</td>
                  <td>{{ log.stackTrace }}</td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
      </div>
    </div>
    <div v-else-if="logs.length === 0">
        <div class="mt-5">
          No logs found.
        </div>
      </div>
  </div>
</template>

<script>
import axios from 'axios';
import { LOGGING_CONTROLLER } from '../constants.js';

export default {
  data() {
    return {
      logs: [],
    };
  },
  computed: {
  },
  async created() {
    await this.fetchLogs();
  },
  methods: {
    async fetchLogs() {
      try {
        const response = await axios.get(`${LOGGING_CONTROLLER}`);
        this.logs = response.data;
      } catch (error) {
        console.error('Error fetching logs:', error);
      }
    },
  },
};
</script>