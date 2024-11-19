<template>
  <div class="container">
    <h1>Logs</h1>
    <div v-if="logs.length > 0">
      <div class="row">
        <div class="col-md-12 mb-4 mt-4">
          <div class="table-rsponsive">
            <table class="table table-bordered">
              <thead>
                <tr>
                  <th>Id</th>
                  <th>Level</th>
                  <th>Timestamp</th>
                  <th>Message</th>
                  <th>Trace</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="log in logs" :key="log.id">
                  <td>{{ log.id }}</td>
                  <td>{{ log.logLevel }}</td>
                  <td>{{ log.timestamp }}</td>
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
      <p>No logs to display.</p>
    </div>
    <div v-else>
      <p>Loading log information...</p>
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
