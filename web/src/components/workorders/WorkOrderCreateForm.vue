<script setup lang="ts">
import { reactive, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import { workOrderApi } from '../../api/workOrderApi'
import type { WorkOrder, CreateWorkOrderPayload } from '../../types/asset'

const { t } = useI18n()
const props = defineProps<{ assetId: string }>()
const emit = defineEmits<{
  (e: 'created', wo: WorkOrder): void
  (e: 'cancel'): void
}>()

const form = reactive<CreateWorkOrderPayload>({
  assetId: props.assetId,
  title: '',
  priority: 'Medium',
  description: '',
  dueDate: undefined,
})

const loading = ref(false)
const error = ref<string | null>(null)

async function submit() {
  error.value = null
  loading.value = true
  try {
    const wo = await workOrderApi.create(form)
    emit('created', wo)
  } catch (e: unknown) {
    error.value = e instanceof Error ? e.message : t('form.errorUnknown')
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <form class="wo-form" @submit.prevent="submit">
    <h2>{{ t('wo.createTitle') }}</h2>

    <div v-if="error" class="alert alert--danger" role="alert">{{ error }}</div>

    <div class="form-group">
      <label for="wo-title">{{ t('wo.title') }} *</label>
      <input id="wo-title" v-model="form.title" type="text" required />
    </div>

    <div class="form-group">
      <label for="wo-priority">{{ t('wo.priority') }} *</label>
      <select id="wo-priority" v-model="form.priority" required>
        <option value="Low">{{ t('wo.priorityLow') }}</option>
        <option value="Medium">{{ t('wo.priorityMedium') }}</option>
        <option value="High">{{ t('wo.priorityHigh') }}</option>
      </select>
    </div>

    <details class="form-details">
      <summary>{{ t('form.details') }}</summary>
      <div class="form-group">
        <label for="wo-description">{{ t('form.description') }}</label>
        <textarea id="wo-description" v-model="form.description" rows="3" />
      </div>
      <div class="form-group">
        <label for="wo-duedate">{{ t('wo.dueDate') }}</label>
        <input id="wo-duedate" v-model="form.dueDate" type="date" />
      </div>
    </details>

    <div class="wo-form__actions">
      <button type="submit" class="btn btn--primary" :disabled="loading">
        {{ loading ? t('form.submitting') : t('wo.submitBtn') }}
      </button>
      <button type="button" class="btn btn--secondary" @click="emit('cancel')">
        {{ t('assets.cancel') }}
      </button>
    </div>
  </form>
</template>

<style scoped>
.wo-form {
  display: flex;
  flex-direction: column;
  gap: 16px;
  max-width: 480px;
}
.wo-form__actions {
  display: flex;
  gap: 12px;
}
.form-details summary {
  cursor: pointer;
  font-weight: 600;
  color: #6b7280;
  font-size: 0.875rem;
  padding: 4px 0;
}
.form-details .form-group {
  margin-top: 12px;
}
</style>
