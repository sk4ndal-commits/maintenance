<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useI18n } from 'vue-i18n'
import { assetApi } from '../api/assetApi'
import type { Asset, WorkOrder, WorkOrderStatus } from '../types/asset'
import QrCodePanel from '../components/assets/QrCodePanel.vue'
import WorkOrderCreateForm from '../components/workorders/WorkOrderCreateForm.vue'

const { t, locale } = useI18n()
const route = useRoute()
const router = useRouter()

const asset = ref<Asset | null>(null)
const workOrders = ref<WorkOrder[]>([])
const loading = ref(false)
const error = ref<string | null>(null)
const showWoForm = ref(false)

onMounted(async () => {
  loading.value = true
  try {
    const id = route.params.id as string
    asset.value = await assetApi.getById(id)
    workOrders.value = await assetApi.getWorkOrders(id)
  } catch (e: unknown) {
    error.value = e instanceof Error ? e.message : t('assets.errorLoading')
  } finally {
    loading.value = false
  }
})

async function onWoCreated(wo: WorkOrder) {
  showWoForm.value = false
  workOrders.value = [wo, ...workOrders.value]
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
  <div class="asset-detail">
    <button class="btn btn--secondary asset-detail__back" @click="router.back()">
      ← {{ t('detail.back') }}
    </button>

    <div v-if="loading" class="asset-detail__loading">{{ t('assets.loading') }}</div>
    <div v-if="error" class="alert alert--danger">{{ error }}</div>

    <template v-if="asset">
      <div class="asset-detail__header">
        <div>
          <h1 class="asset-detail__name">{{ asset.name }}</h1>
          <span class="badge">{{ asset.type }}</span>
        </div>
      </div>

      <div class="asset-detail__card">
        <div class="asset-detail__row">
          <span class="asset-detail__label">{{ t('detail.location') }}</span>
          <span>📍 {{ asset.location }}</span>
        </div>
        <div v-if="asset.description" class="asset-detail__row">
          <span class="asset-detail__label">{{ t('detail.description') }}</span>
          <span>{{ asset.description }}</span>
        </div>
        <div class="asset-detail__row">
          <span class="asset-detail__label">{{ t('detail.created') }}</span>
          <span>{{ new Date(asset.createdAt).toLocaleDateString(locale) }}</span>
        </div>
        <div class="asset-detail__row">
          <span class="asset-detail__label">{{ t('detail.qrCode') }}</span>
          <code>{{ asset.qrCodePayload }}</code>
        </div>
      </div>

      <h2 class="asset-detail__section-title">{{ t('qr.title') }}</h2>
      <QrCodePanel :asset-id="asset.assetId" :asset-name="asset.name" class="asset-detail__qr" />

      <h2 class="asset-detail__section-title">{{ t('detail.workOrders') }}</h2>

      <div class="asset-detail__wo-actions">
        <button class="btn btn--primary" @click="showWoForm = !showWoForm">
          {{ showWoForm ? t('assets.cancel') : t('wo.create') }}
        </button>
      </div>

      <div v-if="showWoForm" class="asset-detail__wo-form">
        <WorkOrderCreateForm
          :asset-id="asset.assetId"
          @created="onWoCreated"
          @cancel="showWoForm = false"
        />
      </div>

      <p v-if="workOrders.length === 0" class="asset-detail__empty">
        {{ t('detail.noWorkOrders') }}
      </p>

      <div v-else class="asset-detail__wo-list">
        <div v-for="wo in workOrders" :key="wo.workOrderId" class="asset-detail__wo-card">
          <div class="asset-detail__wo-header">
            <span :class="['badge', statusClass(wo.status)]">{{ wo.status }}</span>
            <span class="asset-detail__wo-date">
              {{ new Date(wo.createdAt).toLocaleDateString(locale) }}
            </span>
          </div>
          <strong class="asset-detail__wo-title">{{ wo.title }}</strong>
          <span class="asset-detail__wo-priority">{{ wo.priority }}</span>
        </div>
      </div>
    </template>
  </div>
</template>

<style scoped>
.asset-detail {
  padding: 32px;
  max-width: 800px;
  margin: 0 auto;
}

.asset-detail__back {
  margin-bottom: 24px;
}

.asset-detail__loading {
  color: #6b7280;
}

.asset-detail__header {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  margin-bottom: 24px;
}

.asset-detail__name {
  margin: 0 0 8px 0;
  font-size: 1.75rem;
  font-weight: 700;
}

.asset-detail__card {
  background: #fff;
  border: 1px solid #e5e7eb;
  padding: 20px;
  margin-bottom: 32px;
  display: flex;
  flex-direction: column;
  gap: 12px;
}

.asset-detail__row {
  display: flex;
  gap: 16px;
}

.asset-detail__label {
  font-weight: 600;
  font-size: 0.875rem;
  color: #6b7280;
  min-width: 120px;
}

.asset-detail__section-title {
  font-size: 1.25rem;
  font-weight: 700;
  margin-bottom: 16px;
}

.asset-detail__qr {
  margin-bottom: 32px;
}

.asset-detail__empty {
  color: #6b7280;
  font-size: 0.95rem;
}

.asset-detail__wo-list {
  display: flex;
  flex-direction: column;
  gap: 12px;
}

.asset-detail__wo-card {
  background: #fff;
  border: 1px solid #e5e7eb;
  padding: 16px;
  display: flex;
  flex-direction: column;
  gap: 6px;
}

.asset-detail__wo-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.asset-detail__wo-date {
  font-size: 0.75rem;
  color: #9ca3af;
}

.asset-detail__wo-title {
  font-size: 1rem;
  color: #111827;
}

.asset-detail__wo-priority {
  font-size: 0.875rem;
  color: #6b7280;
}

.asset-detail__wo-actions {
  margin-bottom: 16px;
}

.asset-detail__wo-form {
  margin-bottom: 24px;
  padding: 24px;
  background: #f9fafb;
  border: 1px solid #e5e7eb;
}
</style>
