<template>
  <div>
    <AddDebt @debtAdded="refreshList" />
    <DebtList ref="debtList" />
  </div>
</template>

<script>
import DebtList from '../components/DebtList.vue';

export default {
  components: {
    DebtList
  },
  methods: {
    refreshList() {
      this.$refs.debtList.fetchDebts(); // Call the fetchDebts method directly after adding a debt
    },
  },
};
</script>
