<template>
  <table class="audit-table">
    <thead>
      <tr>
        <th>Timestamp</th>
        <th>Action</th>
        <th>User ID</th>
        <th>Details</th>
      </tr>
    </thead>
    <tbody>
      <tr v-for="log in logs" :key="log.id">
        <td>{{ new Date(log.createdAt).toLocaleString() }}</td>
        <td>{{ log.action }}</td>
        <td>{{ log.userId }}</td>
        <td>{{ log.details }}</td>
      </tr>
      <tr v-if="logs.length === 0">
        <td colspan="4">No logs found</td>
      </tr>
    </tbody>
  </table>
</template>

<script setup lang="ts">
import type { AuditLog } from '../../types/audit'

defineProps<{
  logs: AuditLog[]
}>()
</script>

<style scoped>
.audit-table {
  width: 100%;
  border-collapse: collapse;
}
.audit-table th, .audit-table td {
  padding: 8px;
  border: 1px solid #ddd;
  text-align: left;
}
</style>
