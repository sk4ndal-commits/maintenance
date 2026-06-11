<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import { workOrderApi } from '../../api/workOrderApi'
import type { ChecklistStep } from '../../types/asset'

const { t } = useI18n()
const props = defineProps<{ workOrderId: string }>()

const steps = ref<ChecklistStep[]>([])
const newLabel = ref('')
const newMandatory = ref(false)
const newRequiresPhoto = ref(false)
const loading = ref(false)
const error = ref<string | null>(null)

onMounted(async () => {
  steps.value = await workOrderApi.getChecklist(props.workOrderId)
})

async function addStep() {
  if (!newLabel.value.trim()) return
  loading.value = true
  error.value = null
  try {
    const step = await workOrderApi.addChecklistStep(props.workOrderId, {
      workOrderId: props.workOrderId,
      label: newLabel.value.trim(),
      isMandatory: newMandatory.value,
      requiresPhoto: newRequiresPhoto.value,
    })
    steps.value.push(step)
    newLabel.value = ''
    newMandatory.value = false
    newRequiresPhoto.value = false
  } catch (e: unknown) {
    error.value = e instanceof Error ? e.message : t('form.errorUnknown')
  } finally {
    loading.value = false
  }
}

async function toggleStep(step: ChecklistStep) {
  try {
    const updated = await workOrderApi.toggleChecklistStep(props.workOrderId, step.id, {
      workOrderId: props.workOrderId,
      stepId: step.id,
      isCompleted: !step.isCompleted,
    })
    const idx = steps.value.findIndex(s => s.id === updated.id)
    if (idx !== -1) steps.value[idx] = updated
  } catch (e: unknown) {
    error.value = e instanceof Error ? e.message : t('form.errorUnknown')
  }
}
</script>

<template>
  <div class="checklist-form">
    <h4 class="checklist-form__title">{{ t('wo.checklist') }}</h4>
    <div v-if="error" class="alert alert--danger">{{ error }}</div>

    <ul v-if="steps.length" class="checklist-form__list">
      <li v-for="step in steps" :key="step.id" class="checklist-form__item">
        <label class="checklist-form__label">
          <input type="checkbox" :checked="step.isCompleted" @change="toggleStep(step)" />
          <span :class="{ 'checklist-form__text--done': step.isCompleted }">{{ step.label }}</span>
          <span v-if="step.isMandatory" class="badge badge--priority-high checklist-form__mandatory">
            {{ t('wo.checklistMandatory') }}
          </span>
        </label>
      </li>
    </ul>
    <p v-else class="checklist-form__empty">{{ t('wo.checklistEmpty') }}</p>

    <form class="checklist-form__add" @submit.prevent="addStep">
      <input
        v-model="newLabel"
        class="checklist-form__input"
        :placeholder="t('wo.checklistStep')"
        required
      />
      <label class="checklist-form__mandatory-toggle">
        <input type="checkbox" v-model="newMandatory" />
        {{ t('wo.checklistMandatory') }}
      </label>
      <label class="checklist-form__mandatory-toggle">
        <input type="checkbox" v-model="newRequiresPhoto" />
        {{ t('wo.checklistRequiresPhoto') }}
      </label>
      <button type="submit" class="btn btn--secondary" :disabled="loading || !newLabel.trim()">
        {{ t('wo.checklistAdd') }}
      </button>
    </form>
  </div>
</template>

<style scoped>
.checklist-form { margin-top: 12px; }
.checklist-form__title { font-size: 14px; font-weight: 600; margin-bottom: 8px; color: #374151; }
.checklist-form__list { list-style: none; padding: 0; margin: 0 0 8px; }
.checklist-form__item { padding: 4px 0; border-bottom: 1px solid #f3f4f6; }
.checklist-form__label { display: flex; align-items: center; gap: 8px; cursor: pointer; font-size: 14px; }
.checklist-form__text--done { text-decoration: line-through; color: #9ca3af; }
.checklist-form__mandatory { font-size: 10px; padding: 1px 6px; }
.checklist-form__empty { font-size: 13px; color: #9ca3af; margin-bottom: 8px; }
.checklist-form__add { display: flex; gap: 8px; align-items: center; flex-wrap: wrap; margin-top: 8px; }
.checklist-form__input { flex: 1; min-width: 160px; padding: 6px 10px; border: 1px solid #d1d5db; font-size: 13px; }
.checklist-form__mandatory-toggle { display: flex; align-items: center; gap: 4px; font-size: 13px; white-space: nowrap; }
</style>
