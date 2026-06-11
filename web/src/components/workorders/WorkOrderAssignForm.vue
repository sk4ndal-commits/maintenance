<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import { technicianApi } from '../../api/technicianApi'
import { workOrderApi } from '../../api/workOrderApi'
import type { WorkOrder, Technician } from '../../types/asset'

const { t } = useI18n()
const props = defineProps<{ workOrder: WorkOrder }>()
const emit = defineEmits<{
  (e: 'assigned', wo: WorkOrder): void
  (e: 'cancel'): void
}>()

const technicians = ref<Technician[]>([])
const selectedTechId = ref(props.workOrder.assignedTechnicianId ?? '')
const loading = ref(false)
const error = ref<string | null>(null)

onMounted(async () => {
  try {
    technicians.value = await technicianApi.getAll()
  } catch {
    error.value = t('form.errorUnknown')
  }
})

async function submit() {
  if (!selectedTechId.value) return
  loading.value = true
  error.value = null
  try {
    const updated = await workOrderApi.assign(props.workOrder.workOrderId, {
      workOrderId: props.workOrder.workOrderId,
      technicianId: selectedTechId.value,
    })
    emit('assigned', updated)
  } catch (e: unknown) {
    error.value = e instanceof Error ? e.message : t('form.errorUnknown')
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <form class="assign-form" @submit.prevent="submit">
    <h3>{{ t('wo.assign') }}</h3>

    <div v-if="error" class="alert alert--danger" role="alert">{{ error }}</div>

    <div class="form-group">
      <label for="assign-tech">{{ t('wo.technician') }}</label>
      <select id="assign-tech" v-model="selectedTechId" required>
        <option value="" disabled>{{ t('wo.selectTechnician') }}</option>
        <option v-for="tech in technicians" :key="tech.technicianId" :value="tech.technicianId">
          {{ tech.name }}
        </option>
      </select>
    </div>

    <div class="assign-form__actions">
      <button type="submit" class="btn btn--primary" :disabled="loading || !selectedTechId">
        {{ loading ? t('form.submitting') : t('wo.assignBtn') }}
      </button>
      <button type="button" class="btn btn--secondary" @click="emit('cancel')">
        {{ t('assets.cancel') }}
      </button>
    </div>
  </form>
</template>

<style scoped>
.assign-form {
  display: flex;
  flex-direction: column;
  gap: 16px;
  max-width: 400px;
}
.assign-form__actions {
  display: flex;
  gap: 12px;
}
</style>
