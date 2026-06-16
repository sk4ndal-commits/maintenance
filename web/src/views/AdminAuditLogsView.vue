<template>
  <div class="admin-audit">
    <h1>System Audit Logs</h1>
    <AuditLogTable :logs="logs" />
    <button @click="loadNextPage" class="btn btn--secondary" :disabled="loading">
      {{ loading ? 'Loading...' : 'Load More' }}
    </button>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { auditApi } from '../api/auditApi'
import type { AuditLog } from '../types/audit'
import AuditLogTable from '../components/common/AuditLogTable.vue'

const logs = ref<AuditLog[]>([])
const page = ref(1)
const loading = ref(false)

async function load() {
  loading.value = true
  try {
    const newLogs = await auditApi.getAll(page.value)
    logs.value = [...logs.value, ...newLogs]
  } finally {
    loading.value = false
  }
}

function loadNextPage() {
  page.value++
  load()
}

onMounted(load)
</script>
