<script setup lang="ts">
import { ref, onMounted, onUnmounted } from 'vue'
import { useI18n } from 'vue-i18n'
import { assetApi } from '../../api/assetApi'

const { t } = useI18n()
const props = defineProps<{ assetId: string; assetName: string }>()

const qrUrl = ref<string | null>(null)
const loading = ref(false)
const error = ref<string | null>(null)

onMounted(async () => {
  loading.value = true
  try {
    qrUrl.value = await assetApi.downloadQrCode(props.assetId)
  } catch (e: unknown) {
    error.value = e instanceof Error ? e.message : t('qr.errorLoading')
  } finally {
    loading.value = false
  }
})

onUnmounted(() => {
  if (qrUrl.value) URL.revokeObjectURL(qrUrl.value)
})

function downloadPng() {
  if (!qrUrl.value) return
  const a = document.createElement('a')
  a.href = qrUrl.value
  a.download = `asset-${props.assetId}.png`
  a.click()
}

function printQr() {
  window.print()
}
</script>

<template>
  <div class="qr-panel">
    <div v-if="loading">{{ t('assets.loading') }}</div>
    <div v-else-if="error" class="alert alert--danger">{{ error }}</div>
    <template v-else-if="qrUrl">
      <div class="qr-panel__image-wrap print-area">
        <img :src="qrUrl" :alt="`QR ${assetName}`" class="qr-panel__image" />
        <p class="qr-panel__label">{{ assetName }}</p>
      </div>
      <div class="qr-panel__actions no-print">
        <button class="btn btn--primary" @click="downloadPng">
          {{ t('qr.downloadPng') }}
        </button>
        <button class="btn btn--secondary" @click="printQr">
          {{ t('qr.print') }}
        </button>
      </div>
    </template>
  </div>
</template>

<style scoped>
.qr-panel {
  display: flex;
  flex-direction: column;
  gap: 16px;
  align-items: flex-start;
}

.qr-panel__image-wrap {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 8px;
  padding: 16px;
  border: 1px solid var(--color-border);
  background: #fff;
}

.qr-panel__image {
  width: 200px;
  height: 200px;
  image-rendering: pixelated;
}

.qr-panel__label {
  font-size: 0.875rem;
  font-weight: 600;
  color: #374151;
  margin: 0;
}

.qr-panel__actions {
  display: flex;
  gap: 12px;
}

@media print {
  .no-print { display: none !important; }
  .print-area { border: none; padding: 0; }
}
</style>
