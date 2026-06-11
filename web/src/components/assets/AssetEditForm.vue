<script setup lang="ts">
import { reactive, ref, watch } from 'vue'
import { useI18n } from 'vue-i18n'
import { assetApi } from '../../api/assetApi'
import type { Asset, UpdateAssetPayload } from '../../types/asset'

const { t } = useI18n()
const props = defineProps<{ asset: Asset }>()
const emit = defineEmits<{
  (e: 'updated', asset: Asset): void
  (e: 'cancel'): void
}>()

const form = reactive<UpdateAssetPayload>({
  assetId: props.asset.assetId,
  name: props.asset.name,
  type: props.asset.type,
  location: props.asset.location,
  description: props.asset.description ?? '',
})

watch(() => props.asset, (a) => {
  form.assetId = a.assetId
  form.name = a.name
  form.type = a.type
  form.location = a.location
  form.description = a.description ?? ''
})

const loading = ref(false)
const error = ref<string | null>(null)

async function submit() {
  error.value = null
  loading.value = true
  try {
    const updated = await assetApi.update(form.assetId, form)
    emit('updated', updated)
  } catch (e: unknown) {
    error.value = e instanceof Error ? e.message : t('form.errorUnknown')
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <form class="asset-form" @submit.prevent="submit">
    <h2>{{ t('form.editTitle') }}</h2>

    <div v-if="error" class="alert alert--danger" role="alert">{{ error }}</div>

    <div class="form-group">
      <label for="edit-name">{{ t('form.name') }} *</label>
      <input id="edit-name" v-model="form.name" type="text" required />
    </div>

    <div class="form-group">
      <label for="edit-type">{{ t('form.type') }} *</label>
      <select id="edit-type" v-model="form.type" required>
        <option value="" disabled>{{ t('form.typeSelect') }}</option>
        <option value="Maschine">{{ t('form.typeMachine') }}</option>
        <option value="Anlage">{{ t('form.typePlant') }}</option>
        <option value="Fahrzeug">{{ t('form.typeVehicle') }}</option>
        <option value="Gebäude">{{ t('form.typeBuilding') }}</option>
      </select>
    </div>

    <div class="form-group">
      <label for="edit-location">{{ t('form.location') }} *</label>
      <input id="edit-location" v-model="form.location" type="text" required />
    </div>

    <details class="form-details">
      <summary>{{ t('form.details') }}</summary>
      <div class="form-group">
        <label for="edit-description">{{ t('form.description') }}</label>
        <textarea id="edit-description" v-model="form.description" rows="3" />
      </div>
    </details>

    <div class="asset-form__actions">
      <button type="submit" class="btn btn--primary" :disabled="loading">
        {{ loading ? t('form.submitting') : t('form.save') }}
      </button>
      <button type="button" class="btn btn--secondary" @click="emit('cancel')">
        {{ t('assets.cancel') }}
      </button>
    </div>
  </form>
</template>

<style scoped>
.asset-form {
  display: flex;
  flex-direction: column;
  gap: 16px;
  max-width: 480px;
}

.asset-form__actions {
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
