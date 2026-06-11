<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import { assetApi } from '../api/assetApi'
import type { Asset } from '../types/asset'
import AssetCreateForm from '../components/assets/AssetCreateForm.vue'
import AssetCard from '../components/assets/AssetCard.vue'

const { t } = useI18n()

const assets = ref<Asset[]>([])
const total = ref(0)
const page = ref(1)
const pageSize = 20
const loading = ref(false)
const error = ref<string | null>(null)
const createdAsset = ref<Asset | null>(null)
const showForm = ref(false)

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
      <button class="btn btn--primary" @click="showForm = !showForm">
        {{ showForm ? t('assets.cancel') : t('assets.create') }}
      </button>
    </div>

    <div v-if="createdAsset" class="alert alert--success">
      ✅ <strong>{{ t('assets.created', { name: createdAsset.name }) }}</strong>
      {{ t('assets.qrCode') }}: <code>{{ createdAsset.qrCodePayload }}</code>
    </div>

    <div v-if="showForm" class="assets-view__form-panel">
      <AssetCreateForm @created="onCreated" />
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
</style>
