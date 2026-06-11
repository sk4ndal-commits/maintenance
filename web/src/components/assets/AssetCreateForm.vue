<script setup lang="ts">
import { reactive, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import { assetApi } from '../../api/assetApi'
import type { Asset, CreateAssetPayload } from '../../types/asset'

const { t } = useI18n()
const emit = defineEmits<{ (e: 'created', asset: Asset): void }>()

const form = reactive<CreateAssetPayload>({
  name: '',
  type: '',
  location: '',
  description: '',
})

const loading = ref(false)
const error = ref<string | null>(null)

async function submit() {
  error.value = null
  loading.value = true
  try {
    const payload: CreateAssetPayload = {
      name: form.name,
      type: form.type,
      location: form.location,
      description: form.description || undefined,
    }
    const asset = await assetApi.create(payload)
    emit('created', asset)
    form.name = ''
    form.type = ''
    form.location = ''
    form.description = ''
  } catch (e: unknown) {
    error.value = e instanceof Error ? e.message : t('form.errorUnknown')
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <form class="asset-form" @submit.prevent="submit">
    <h2>{{ t('form.title') }}</h2>

    <div v-if="error" class="alert alert--danger" role="alert">
      {{ error }}
    </div>

    <div class="form-group">
      <label for="name">{{ t('form.name') }} *</label>
      <input id="name" v-model="form.name" type="text" required :placeholder="t('form.namePlaceholder')" />
    </div>

    <div class="form-group">
      <label for="type">{{ t('form.type') }} *</label>
      <select id="type" v-model="form.type" required>
        <option value="" disabled>{{ t('form.typeSelect') }}</option>
        <option value="Maschine">{{ t('form.typeMachine') }}</option>
        <option value="Anlage">{{ t('form.typePlant') }}</option>
        <option value="Fahrzeug">{{ t('form.typeVehicle') }}</option>
        <option value="Gebäude">{{ t('form.typeBuilding') }}</option>
      </select>
    </div>

    <div class="form-group">
      <label for="location">{{ t('form.location') }} *</label>
      <input id="location" v-model="form.location" type="text" required :placeholder="t('form.locationPlaceholder')" />
    </div>

    <details class="form-details">
      <summary>{{ t('form.details') }}</summary>
      <div class="form-group">
        <label for="description">{{ t('form.description') }}</label>
        <textarea id="description" v-model="form.description" rows="3" :placeholder="t('form.descriptionPlaceholder')" />
      </div>
    </details>

    <button type="submit" class="btn btn--primary" :disabled="loading">
      {{ loading ? t('form.submitting') : t('form.submit') }}
    </button>
  </form>
</template>

<style scoped>
.asset-form {
  display: flex;
  flex-direction: column;
  gap: 16px;
  max-width: 480px;
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

.btn {
  align-self: flex-start;
}
</style>
