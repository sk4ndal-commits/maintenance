<script setup lang="ts">
import { ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { WorkOrder, WorkOrderStatus } from '../../types/asset'
import WorkOrderChecklistForm from './WorkOrderChecklistForm.vue'

const props = defineProps<{ workOrder: WorkOrder }>()
const emit = defineEmits(['assign', 'updated'])
const { t, locale } = useI18n()

const isExpanded = ref(false)

function priorityClass(priority: string): string {
  return {
    High: 'badge--priority-high',
    Medium: 'badge--priority-medium',
    Low: 'badge--priority-low',
  }[priority] ?? ''
}

function statusClass(status: WorkOrderStatus): string {
  return {
    Open: 'badge--warning',
    Assigned: 'badge--info',
    InProgress: 'badge--primary',
    Done: 'badge--success',
  }[status] ?? ''
}
</script>

<template>
  <div class="wo-card" :class="{ 'wo-card--expanded': isExpanded }">
    <div class="wo-card__header">
      <strong class="wo-card__title">{{ workOrder.title }}</strong>
      <div class="wo-card__status-group">
        <span :class="['badge', statusClass(workOrder.status)]">{{ t(`wo.status.${workOrder.status}`) }}</span>
        <span :class="['badge', 'badge--priority', priorityClass(workOrder.priority)]">{{ t(`wo.priority${workOrder.priority}`) }}</span>
      </div>
    </div>
    
    <div class="wo-card__meta">
      <span>{{ new Date(workOrder.createdAt).toLocaleDateString(locale) }}</span>
      <span v-if="workOrder.assignedTechnicianName">👤 {{ workOrder.assignedTechnicianName }}</span>
      <span v-else class="wo-card__unassigned">{{ t('wo.unassigned') }}</span>
    </div>

    <div v-if="isExpanded" class="wo-card__details">
      <div v-if="workOrder.status === 'Done'" class="wo-card__completion">
        <p>{{ t('wo.completedAt') }}: {{ workOrder.completedAt ? new Date(workOrder.completedAt).toLocaleDateString(locale) : '—' }}</p>
        <p v-if="workOrder.completionNotes">{{ workOrder.completionNotes }}</p>
      </div>
      <WorkOrderChecklistForm :work-order-id="workOrder.workOrderId" />
    </div>

    <div class="wo-card__actions">
      <button class="btn btn--secondary" @click="isExpanded = !isExpanded">
        {{ isExpanded ? t('common.collapse') : t('common.expand') }}
      </button>
      <button
        v-if="workOrder.status !== 'Done'"
        class="btn btn--primary"
        @click="emit('assign', workOrder.workOrderId)"
      >
        {{ workOrder.assignedTechnicianId ? t('wo.reassign') : t('wo.assignBtn') }}
      </button>
    </div>
  </div>
</template>

<style scoped>
.wo-card {
  background: #fff;
  border: 1px solid #e5e7eb;
  padding: 16px;
  display: flex;
  flex-direction: column;
  gap: 12px;
}

.wo-card__header {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  gap: 8px;
}

.wo-card__title {
  font-size: 1rem;
  color: #111827;
}

.wo-card__status-group {
  display: flex;
  gap: 8px;
}

.wo-card__meta {
  display: flex;
  gap: 16px;
  font-size: 0.875rem;
  color: #6b7280;
}

.wo-card__unassigned {
  color: #9ca3af;
  font-style: italic;
}

.wo-card__details {
  border-top: 1px solid #f3f4f6;
  padding-top: 12px;
  margin-top: 4px;
}

.wo-card__actions {
  display: flex;
  gap: 8px;
  margin-top: 8px;
}
</style>
