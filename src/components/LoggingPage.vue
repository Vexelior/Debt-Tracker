<template>
  <div class="container">
    <h1>Logs</h1>
    <div v-if="logs">
      <div class="row">
        <div class="col-md-12 mb-4 mt-4">
          <div class="table-responsive">
            <table class="table table-bordered">
              <thead>
                <tr>
                  <th>Id</th>
                  <th>Level</th>
                  <th>Timestamp</th>
                  <th>Message</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="log in logs" :key="log.id">
                  <td>{{ log.id }}</td>
                  <td>{{ log.logLevel }}</td>
                  <td>{{ new Date(log.timestamp).toLocaleDateString() }}</td>
                  <td>{{ log.message }}</td>
                </tr>
              </tbody>
            </table>
          </div>
          <nav aria-label="Page navigation">
            <ul class="pagination">
              <li class="page-item" :class="{ disabled : currentPage === 1 }">
                <a class="page-link" @click="changePage(currentPage - 1)" aria-label="Previous">
                  <span aria-hidden="true">&laquo;</span>
                </a>
              </li>
              <li class="page-item" v-for="page in totalPages" :key="page" :class="{ active :
                currentPage === page }">
                <a class="page-link" @click="changePage(page)">{{ page }}</a>
              </li>
              <li class="page-item" :class="{ disabled : currentPage === totalPages }">
                <a class="page-link" @click="changePage(currentPage + 1)" aria-label="Next">
                  <span aria-hidden="true">&raquo;</span>
                </a>
              </li>
            </ul>
          </nav>
        </div>
      </div>
    </div>
    <div v-else>
      <p>No logs to display.</p>
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
      currentPage: 1,
      pageSize: 10,
      totalLogs: 0,
    };
  },
  computed: {
    totalPages() {
      return Math.ceil(this.totalLogs / this.pageSize);
    },
  },
  async created() {
    await this.fetchTotalLogs();
    await this.fetchLogs();
  },
  methods: {
    async fetchTotalLogs() {
      try {
        const response = await axios.get(`${LOGGING_CONTROLLER}`);
        this.totalLogs = response.data.length;
      } catch (error) {
        console.error('Error fetching total logs:', error);
      }
    },
    async fetchLogs() {
      try {
        const response = await axios.get(`${LOGGING_CONTROLLER}?page=${this.currentPage}&pageSize=${this.pageSize}`);
        this.logs = response.data;
      } catch (error) {
        console.error('Error fetching logs:', error);
      }
    },
    async changePage(page) {
      if (page > 0 && page <= this.totalPages) {
        this.currentPage = page;
        await this.fetchLogs();
      }
    },
  },
};
</script>