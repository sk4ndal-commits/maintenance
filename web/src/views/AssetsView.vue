<script setup lang="ts">
import { ref, onMounted, computed } from 'vue'
import { useI18n } from 'vue-i18n'
import { assetApi } from '../api/assetApi'
import type { Asset } from '../types/asset'
import AssetCreateForm from '../components/assets/AssetCreateForm.vue'
import AssetEditForm from '../components/assets/AssetEditForm.vue'
import AssetCard from '../components/assets/AssetCard.vue'

const { t } = useI18n()

function getTokenRole(): string | null {
  const token = localStorage.getItem('jwt')
  if (!token) return null
  try {
    const payload = JSON.parse(atob(token.split('.')[1]))
    return payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] ?? null
  } catch {
    return null
  }
}

const role = computed(() => getTokenRole())
const canEdit = computed(() => role.value === 'Admin' || role.value === 'Planner')

const assets = ref<Asset[]>([])
const total = ref(0)
const page = ref(1)
const pageSize = 20
const loading = ref(false)
const error = ref<string | null>(null)
const createdAsset = ref<Asset | null>(null)
const showForm = ref(false)
const editingAsset = ref<Asset | null>(null)

async function loadAssets() {
  loading.value = true
  error.value = null
  try {
    const res = await assetApi.getAll(page.value, pageSize)
    assets.value = res.data
    total.value = res.total
  } catch (e: unknown) {
    error.value = e instanceof Error ? e.message : t('assets.errorLoading')
  } finally {
    loading.value = false
  }
}

function onCreated(asset: Asset) {
  createdAsset.value = asset
  showForm.value = false
  loadAssets()
}


function onUpdated(asset: Asset) {
  editingAsset.value = null
  createdAsset.value = asset
  loadAssets()
}

function changePage(newPage: number) {
  page.value = newPage
  loadAssets()
}

const totalPages = () => Math.ceil(total.value / pageSize)

onMounted(loadAssets)
</script>

<template>
  <div class="assets-view">
    <div class="assets-view__header">
      <h1>{{ t('assets.title') }}</h1>
      <button v-if="canEdit" class="btn btn--primary" @click="showForm = true">
        {{ t('assets.create') }}
      </button>
    </div>

    <div v-if="createdAsset" class="alert alert--success">
      ✅ <strong>{{ t('assets.created', { name: createdAsset.name }) }}</strong>
      {{ t('assets.qrCode') }}: <code>{{ createdAsset.qrCodePayload }}</code>
    </div>

    <div v-if="editingAsset" class="assets-view__form-panel">
      <AssetEditForm :asset="editingAsset" @updated="onUpdated" @cancel="editingAsset = null" />
    </div>

    <!-- Asset Create Modal -->
    <div v-if="showForm" class="modal-overlay" @click.self="showForm = false">
      <div class="modal">
        <AssetCreateForm @created="onCreated" @cancel="showForm = false" />
      </div>
    </div>

    <div v-if="error" class="alert alert--danger">{{ error }}</div>

    <div v-if="loading" class="assets-view__loading">{{ t('assets.loading') }}</div>

    <div v-else class="assets-view__grid">
      <AssetCard
        v-for="asset in assets"
        :key="asset.assetId"
        :asset="asset"
      />
      <p v-if="assets.length === 0" class="assets-view__empty">{{ t('assets.empty') }}</p>
    </div>

    <div v-if="totalPages() > 1" class="assets-view__pagination">
      <button :disabled="page <= 1" @click="changePage(page - 1)">{{ t('assets.pagination.prev') }}</button>
      <span>{{ t('assets.pagination.page', { page, total: totalPages() }) }}</span>
      <button :disabled="page >= totalPages()" @click="changePage(page + 1)">{{ t('assets.pagination.next') }}</button>
    </div>
  </div>
</template>

<style scoped>
.assets-view {
  padding: 32px;
  max-width: 1200px;
  margin: 0 auto;
}

.assets-view__header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 24px;
}

.assets-view__header h1 {
  margin: 0;
  font-size: 1.75rem;
  font-weight: 700;
}

.assets-view__form-panel {
  background: #f9fafb;
  border: 1px solid #e5e7eb;
  border-radius: 0;
  padding: 24px;
  margin-bottom: 24px;
}

.assets-view__grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(280px, 1fr));
  gap: 16px;
}

.assets-view__loading,
.assets-view__empty {
  color: #6b7280;
  font-size: 0.95rem;
}

.assets-view__pagination {
  display: flex;
  align-items: center;
  gap: 16px;
  margin-top: 24px;
}

.assets-view__pagination button {
  padding: 6px 14px;
  border: 1px solid #d1d5db;
  border-radius: 0;
  background: #fff;
  cursor: pointer;
}

.assets-view__pagination button:disabled {
  opacity: 0.4;
  cursor: not-allowed;
}
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
</style>
