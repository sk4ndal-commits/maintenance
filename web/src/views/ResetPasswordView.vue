<script setup lang="ts">
import { ref } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { apiClient } from '../api/apiClient'
import { useI18n } from 'vue-i18n'

const { t } = useI18n()
const route = useRoute()
const router = useRouter()
const token = route.query.token as string
const newPassword = ref('')
const error = ref('')
const loading = ref(false)

async function handleResetPassword() {
  if (!token) {
    error.value = t('auth.resetPassword.error')
    return
  }
  error.value = ''
  loading.value = true
  try {
    await apiClient.post('/auth/reset-password', { token, newPassword: newPassword.value })
    router.push('/login')
  } catch {
    error.value = t('auth.resetPassword.error')
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <div class="login-page">
    <div class="login-card">
      <h1 class="login-title">{{ t('auth.resetPassword.title') }}</h1>
      <p class="login-subtitle">{{ t('auth.resetPassword.subtitle') }}</p>

      <form @submit.prevent="handleResetPassword" class="login-form">
        <div class="form-group">
          <label for="password">{{ t('auth.resetPassword.password') }}</label>
          <input
            id="password"
            v-model="newPassword"
            type="password"
            placeholder="••••••••"
            required
          />
        </div>

        <p v-if="error" class="error-message">{{ error }}</p>

        <button type="submit" class="btn-primary" :disabled="loading">
          {{ loading ? t('auth.resetPassword.saving') : t('auth.resetPassword.save') }}
        </button>
      </form>
    </div>
  </div>
</template>

<style scoped>
.login-page {
  min-height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
  background: #f5f5f5;
}

.login-card {
  background: #fff;
  padding: 48px;
  width: 400px;
  max-width: 100%;
  box-shadow: 0 2px 16px rgba(0, 0, 0, 0.1);
}

.login-title {
  font-size: 1.5rem;
  font-weight: 700;
  margin: 0 0 4px;
}

.login-subtitle {
  color: #666;
  margin: 0 0 32px;
}

.login-form {
  display: flex;
  flex-direction: column;
  gap: 16px;
}

.form-group {
  display: flex;
  flex-direction: column;
  gap: 6px;
}

.form-group label {
  font-size: 0.875rem;
  font-weight: 500;
}

.form-group input {
  padding: 10px 12px;
  border: 1px solid #ddd;
  font-size: 1rem;
  outline: none;
  transition: border-color 0.2s;
}

.form-group input:focus {
  border-color: #2563eb;
}

.error-message {
  color: #dc2626;
  font-size: 0.875rem;
  margin: 0;
}

.btn-primary {
  padding: 12px;
  background: #2563eb;
  color: #fff;
  border: none;
  font-size: 1rem;
  font-weight: 600;
  cursor: pointer;
  transition: background 0.2s;
}

.btn-primary:hover:not(:disabled) {
  background: #1d4ed8;
}

.btn-primary:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}
</style>
