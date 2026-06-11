<script setup lang="ts">
import { useI18n } from 'vue-i18n'
import { useRouter } from 'vue-router'
import type { Asset } from '../../types/asset'

const { t, locale } = useI18n()
const router = useRouter()
const props = defineProps<{ asset: Asset }>()
const emit = defineEmits<{ (e: 'edit', asset: Asset): void }>()
</script>

<template>
  <div class="asset-card" @click="router.push(`/assets/${props.asset.assetId}`)">
    <div class="asset-card__header">
      <span class="asset-card__type badge">{{ asset.type }}</span>
      <span class="asset-card__date">{{ new Date(asset.createdAt).toLocaleDateString(locale) }}</span>
    </div>
    <h3 class="asset-card__name">{{ asset.name }}</h3>
    <p class="asset-card__location">📍 {{ asset.location }}</p>
    <p v-if="asset.description" class="asset-card__description">{{ asset.description }}</p>
    <code class="asset-card__qr">{{ t('card.qrCode') }}: {{ asset.qrCodePayload }}</code>
    <div class="asset-card__footer">
      <button class="btn btn--secondary asset-card__edit" @click.stop="emit('edit', asset)">
        {{ t('card.edit') }}
      </button>
    </div>
  </div>
</template>

<style scoped>
.asset-card {
  background: #fff;
  border: 1px solid #e5e7eb;
  border-radius: 0;
  padding: 16px;
  display: flex;
  flex-direction: column;
  gap: 8px;
  cursor: pointer;
  transition: box-shadow 0.15s;
}

.asset-card:hover {
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.08);
}

.asset-card__header {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.asset-card__date {
  font-size: 0.75rem;
  color: #9ca3af;
}

.asset-card__name {
  margin: 0;
  font-size: 1.1rem;
  font-weight: 700;
  color: #111827;
}

.asset-card__location {
  margin: 0;
  font-size: 0.875rem;
  color: #6b7280;
}

.asset-card__description {
  margin: 0;
  font-size: 0.875rem;
  color: #374151;
}

.asset-card__qr {
  font-size: 0.75rem;
  color: #9ca3af;
  background: #f9fafb;
  padding: 4px 8px;
  border-radius: 0;
  align-self: flex-start;
}
</style>
