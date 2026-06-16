<template>
  <div class="login-page">
    <div class="login-card">
      <h1 class="login-title">Passwort vergessen</h1>
      <p class="login-subtitle">Geben Sie Ihre E-Mail-Adresse ein, um einen Reset-Link zu erhalten.</p>

      <form @submit.prevent="handleForgotPassword" class="login-form">
        <div class="form-group">
          <label for="email">E-Mail</label>
          <input
            id="email"
            v-model="email"
            type="email"
            placeholder="name@example.com"
            required
            autocomplete="email"
          />
        </div>

        <p v-if="message" class="info-message">{{ message }}</p>
        <p v-if="error" class="error-message">{{ error }}</p>

        <button type="submit" class="btn-primary" :disabled="loading">
          {{ loading ? 'Senden...' : 'Reset-Link senden' }}
        </button>
      </form>

      <p class="login-link">
        <RouterLink to="/login">Zurück zum Login</RouterLink>
      </p>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { apiClient } from '../api/apiClient'

const email = ref('')
const message = ref('')
const error = ref('')
const loading = ref(false)

async function handleForgotPassword() {
  error.value = ''
  message.value = ''
  loading.value = true
  try {
    // Calling API directly since auth is separate and doesn't require JWT
    const { token } = await apiClient.post<{ token: string }>('/auth/forgot-password', { email: email.value })
    message.value = 'Falls ein Benutzer mit dieser E-Mail existiert, wurde eine E-Mail gesendet. (Token zur Simulation: ' + token + ')'
  } catch {
    error.value = 'Ein Fehler ist aufgetreten. Bitte versuchen Sie es später erneut.'
  } finally {
    loading.value = false
  }
}
</script>

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

.info-message {
  color: #16a34a;
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

.login-link {
  margin-top: 24px;
  text-align: center;
  font-size: 0.875rem;
  color: #666;
}

.login-link a {
  color: #2563eb;
  text-decoration: none;
  font-weight: 500;
}

.login-link a:hover {
  text-decoration: underline;
}
</style>
