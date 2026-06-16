<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useI18n } from 'vue-i18n'
import { assetApi } from '../api/assetApi'
import type { Asset, WorkOrder, WorkOrderStatus } from '../types/asset'
import QrCodePanel from '../components/assets/QrCodePanel.vue'
import WorkOrderCreateForm from '../components/workorders/WorkOrderCreateForm.vue'
import WorkOrderAssignForm from '../components/workorders/WorkOrderAssignForm.vue'
import WorkOrderCard from '../components/workorders/WorkOrderCard.vue'
import AssetEditForm from '../components/assets/AssetEditForm.vue'
import AssetHistoryTimeline from '../components/assets/AssetHistoryTimeline.vue'
import AuditLogTable from '../components/common/AuditLogTable.vue'
import { auditApi } from '../api/auditApi'
import type { AuditLog } from '../types/audit'

const { t, locale } = useI18n()
const route = useRoute()
const router = useRouter()

const asset = ref<Asset | null>(null)
const workOrders = ref<WorkOrder[]>([])
const loading = ref(false)
const error = ref<string | null>(null)
const showWoForm = ref(false)
const showEditForm = ref(false)
const assigningWoId = ref<string | null>(null)
const auditLogs = ref<AuditLog[]>([])
const activeTab = ref('overview')

onMounted(async () => {
  loading.value = true
  try {
    const id = route.params.id as string
    asset.value = await assetApi.getById(id)
    workOrders.value = await assetApi.getWorkOrders(id)
    auditLogs.value = await auditApi.getByEntity(id)
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

function onAssetSaved(updated: Asset) {
  asset.value = updated
  showEditForm.value = false
}

function onAssigned(wo: WorkOrder) {
  assigningWoId.value = null
  const idx = workOrders.value.findIndex(w => w.workOrderId === wo.workOrderId)
  if (idx !== -1) workOrders.value[idx] = wo
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
        <button class="btn btn--secondary" @click="showEditForm = true">{{ t('form.editTitle') }}</button>
      </div>

      <nav class="asset-detail__tabs">
        <button :class="['tab', { 'tab--active': activeTab === 'overview' }]" @click="activeTab = 'overview'">Overview</button>
        <button :class="['tab', { 'tab--active': activeTab === 'workorders' }]" @click="activeTab = 'workorders'">Work Orders</button>
        <button :class="['tab', { 'tab--active': activeTab === 'history' }]" @click="activeTab = 'history'">History</button>
        <button :class="['tab', { 'tab--active': activeTab === 'audit' }]" @click="activeTab = 'audit'">Audit</button>
      </nav>

      <div class="asset-detail__content">
        <!-- Overview Tab -->
        <div v-if="activeTab === 'overview'" class="asset-detail__layout">
          <div class="asset-detail__main">
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
          </div>
          <aside class="asset-detail__sidebar">
            <h3 class="asset-detail__sidebar-title">{{ t('qr.title') }}</h3>
            <QrCodePanel :asset-id="asset.assetId" :asset-name="asset.name" />
          </aside>
        </div>

        <!-- Work Orders Tab -->
        <div v-if="activeTab === 'workorders'">
          <section class="asset-detail__wo-section">
            <div class="asset-detail__section-header">
              <h2 class="asset-detail__section-title">{{ t('detail.workOrders') }}</h2>
              <button class="btn btn--primary" @click="showWoForm = true">
                {{ t('wo.create') }}
              </button>
            </div>

            <p v-if="workOrders.length === 0" class="asset-detail__empty">
              {{ t('detail.noWorkOrders') }}
            </p>

            <div v-else class="asset-detail__wo-list">
              <WorkOrderCard
                v-for="wo in workOrders"
                :key="wo.workOrderId"
                :work-order="wo"
                @assign="assigningWoId = $event"
              />
            </div>
          </section>
        </div>

        <!-- History Tab -->
        <div v-if="activeTab === 'history'">
          <section class="asset-detail__history-section">
            <h2 class="asset-detail__section-title">{{ t('history.title') }}</h2>
            <AssetHistoryTimeline :asset-id="asset.assetId" />
          </section>
        </div>

        <!-- Audit Tab -->
        <div v-if="activeTab === 'audit'">
          <div class="asset-detail__card">
            <h2 class="asset-detail__section-title">Audit Logs</h2>
            <AuditLogTable :logs="auditLogs" />
          </div>
        </div>
      </div>

      <!-- WO Create Modal -->
      <div v-if="showWoForm" class="modal-overlay" @click.self="showWoForm = false">
        <div class="modal">
          <WorkOrderCreateForm
            :asset-id="asset.assetId"
            @created="onWoCreated"
            @cancel="showWoForm = false"
          />
        </div>
      </div>

      <!-- WO Assign Modal -->
      <div v-if="assigningWoId" class="modal-overlay" @click.self="assigningWoId = null">
        <div class="modal">
          <WorkOrderAssignForm
            v-if="workOrders.find(w => w.workOrderId === assigningWoId)"
            :work-order="workOrders.find(w => w.workOrderId === assigningWoId)!"
            @assigned="onAssigned"
            @cancel="assigningWoId = null"
          />
        </div>
      </div>

      <!-- Asset Edit Modal -->
      <div v-if="showEditForm" class="modal-overlay" @click.self="showEditForm = false">
        <div class="modal">
          <AssetEditForm
            :asset="asset"
            @updated="onAssetSaved"
            @cancel="showEditForm = false"
          />
        </div>
      </div>
    </template>
  </div>
</template>

<style scoped>
.asset-detail {
  padding: 32px;
  max-width: 1100px;
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

.asset-detail__tabs {
  display: flex;
  gap: 16px;
  margin-bottom: 24px;
  border-bottom: 1px solid #e5e7eb;
}

.tab {
  padding: 8px 16px;
  cursor: pointer;
  background: none;
  border: none;
  border-bottom: 2px solid transparent;
  font-weight: 600;
  color: #6b7280;
}

.tab--active {
  color: #111827;
  border-bottom-color: #2563eb;
}

.asset-detail__content {
  display: flex;
  flex-direction: column;
  gap: 24px;
}

.asset-detail__card {
  background: #fff;
  border: 1px solid #e5e7eb;
  padding: 20px;
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

.asset-detail__sidebar {
  position: sticky;
  top: 24px;
  background: #fff;
  border: 1px solid #e5e7eb;
  padding: 20px;
}

.asset-detail__sidebar-title {
  font-size: 1rem;
  font-weight: 700;
  margin: 0 0 16px 0;
}

.asset-detail__section-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 16px;
}

.asset-detail__section-title {
  font-size: 1.25rem;
  font-weight: 700;
  margin: 0;
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

.asset-detail__wo-form {
  margin-bottom: 24px;
  padding: 24px;
  background: #f9fafb;
  border: 1px solid #e5e7eb;
}

.asset-detail__wo-meta {
  display: flex;
  gap: 16px;
  align-items: center;
  flex-wrap: wrap;
}

.asset-detail__wo-assignee {
  font-size: 0.875rem;
  color: #374151;
}

.asset-detail__wo-assignee--none {
  color: #9ca3af;
  font-style: italic;
}

.asset-detail__wo-assign-btn {
  align-self: flex-start;
  font-size: 0.875rem;
  padding: 6px 12px;
}

.asset-detail__assign-form {
  margin-top: 12px;
  padding: 16px;
  background: #f9fafb;
  border: 1px solid #e5e7eb;
}

.asset-detail__wo-completion {
  display: flex;
  flex-direction: column;
  gap: 4px;
  margin-top: 8px;
  font-size: 0.85rem;
  color: #6b7280;
}

.asset-detail__wo-completion-notes {
  font-style: italic;
}

.badge--priority-high   { background: #fee2e2; color: #b91c1c; }
.badge--priority-medium { background: #fef9c3; color: #a16207; }
.badge--priority-low    { background: #f3f4f6; color: #6b7280; }

.modal-overlay {
  position: fixed;
  inset: 0;
  background: rgba(0,0,0,0.4);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 100;
}

.modal {
  background: #fff;
  padding: 32px;
  min-width: 400px;
  max-width: 600px;
  width: 100%;
}

@media (max-width: 700px) {
  .asset-detail__layout {
    grid-template-columns: 1fr;
  }

  .asset-detail__sidebar {
    position: static;
  }
}
</style>
