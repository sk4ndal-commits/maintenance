<script setup lang="ts">
import { ref, watch, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import { assetApi, type HistoryEvent } from '../../api/assetApi'

const { t } = useI18n()
const props = defineProps<{ assetId: string }>()

const events = ref<HistoryEvent[]>([])
const total = ref(0)
const search = ref('')
const filterType = ref('')
const loading = ref(false)

const eventTypes = [
  'WorkOrderCreated', 'WorkOrderAssigned', 'WorkOrderCompleted',
  'ChecklistStepCompleted', 'DocumentUploaded',
]

async function load() {
  loading.value = true
  try {
    const res = await assetApi.getHistory(props.assetId, {
      search: search.value || undefined,
      eventType: filterType.value || undefined,
    })
    events.value = res.data
    total.value = res.total
  } finally {
    loading.value = false
  }
}

onMounted(load)
watch([search, filterType], load)

function iconFor(type: string): string {
  return ({
    WorkOrderCreated: '📋',
    WorkOrderAssigned: '👤',
    WorkOrderCompleted: '✅',
    ChecklistStepCompleted: '☑️',
    DocumentUploaded: '📎',
    StatusChanged: '🔄',
  } as Record<string, string>)[type] ?? '•'
}

function colorClass(type: string): string {
  return ({
    WorkOrderCreated: 'timeline__dot--primary',
    WorkOrderAssigned: 'timeline__dot--warning',
    WorkOrderCompleted: 'timeline__dot--success',
    ChecklistStepCompleted: 'timeline__dot--info',
    DocumentUploaded: 'timeline__dot--secondary',
  } as Record<string, string>)[type] ?? ''
}

function formatDate(ts: string): string {
  return new Date(ts).toLocaleString()
}
</script>

<template>
  <div class="asset-history">
    <div class="asset-history__filters">
      <input
        v-model="search"
        class="asset-history__search"
        :placeholder="t('history.search')"
        type="search"
      />
      <select v-model="filterType" class="asset-history__filter-select">
        <option value="">{{ t('history.allTypes') }}</option>
        <option v-for="type in eventTypes" :key="type" :value="type">
          {{ t(`history.type.${type}`) }}
        </option>
      </select>
    </div>

    <p v-if="loading" class="asset-history__empty">{{ t('history.loading') }}</p>
    <p v-else-if="!events.length" class="asset-history__empty">{{ t('history.empty') }}</p>

    <ol v-else class="timeline">
      <li v-for="event in events" :key="event.id" class="timeline__item">
        <span :class="['timeline__dot', colorClass(event.eventType)]">
          {{ iconFor(event.eventType) }}
        </span>
        <div class="timeline__content">
          <div class="timeline__header">
            <span class="timeline__title">{{ event.title }}</span>
            <span class="timeline__time">{{ formatDate(event.timestamp) }}</span>
          </div>
          <p v-if="event.details" class="timeline__details">{{ event.details }}</p>
          <div class="timeline__meta">
            <span v-if="event.workOrderTitle" class="timeline__wo-link">{{ event.workOrderTitle }}</span>
            <span v-if="event.actor" class="timeline__actor">{{ event.actor }}</span>
          </div>
        </div>
      </li>
    </ol>

    <p v-if="!loading && total > 0" class="asset-history__total">
      {{ t('history.total', { count: total }) }}
    </p>
  </div>
</template>

<style scoped>
.asset-history__filters { display: flex; gap: 12px; margin-bottom: 16px; flex-wrap: wrap; }
.asset-history__search { flex: 1; min-width: 200px; padding: 8px 12px; border: 1px solid #d1d5db; }
.asset-history__filter-select { padding: 8px 12px; border: 1px solid #d1d5db; background: #fff; }
.asset-history__empty { color: #9ca3af; font-size: 14px; }
.asset-history__total { font-size: 12px; color: #9ca3af; margin-top: 12px; }

.timeline { list-style: none; padding: 0; margin: 0; position: relative; }
.timeline::before { content: ''; position: absolute; left: 16px; top: 0; bottom: 0; width: 2px; background: #e5e7eb; }
.timeline__item { display: flex; gap: 16px; padding: 12px 0; position: relative; }
.timeline__dot { width: 32px; height: 32px; border-radius: 50%; display: flex; align-items: center; justify-content: center; font-size: 14px; flex-shrink: 0; background: #f3f4f6; z-index: 1; }
.timeline__dot--primary   { background: #dbeafe; }
.timeline__dot--success   { background: #dcfce7; }
.timeline__dot--warning   { background: #fef9c3; }
.timeline__dot--info      { background: #e0f2fe; }
.timeline__dot--secondary { background: #f3f4f6; }

.timeline__content { flex: 1; }
.timeline__header { display: flex; justify-content: space-between; align-items: baseline; gap: 8px; flex-wrap: wrap; }
.timeline__title { font-weight: 600; font-size: 14px; }
.timeline__time { font-size: 12px; color: #9ca3af; white-space: nowrap; }
.timeline__details { font-size: 13px; color: #6b7280; margin: 4px 0 0; }
.timeline__meta { display: flex; gap: 12px; margin-top: 4px; }
.timeline__wo-link { font-size: 12px; color: #1e3a5f; }
.timeline__actor { font-size: 12px; color: #9ca3af; }
</style>
