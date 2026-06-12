<template>
  <div class="login-page">
    <div class="login-card">
      <h1 class="login-title">Maintenance System</h1>
      <p class="login-subtitle">Neues Konto erstellen</p>

      <form @submit.prevent="handleRegister" class="login-form">
        <div class="form-group">
          <label for="name">Name</label>
          <input
            id="name"
            v-model="name"
            type="text"
            placeholder="Vor- und Nachname"
            required
            autocomplete="name"
          />
        </div>

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

        <div class="form-group">
          <label for="password">Passwort</label>
          <input
            id="password"
            v-model="password"
            type="password"
            placeholder="••••••••"
            required
            autocomplete="new-password"
            minlength="6"
          />
        </div>

        <p v-if="error" class="error-message">{{ error }}</p>
        <p v-if="success" class="success-message">{{ success }}</p>

        <button type="submit" class="btn-primary" :disabled="loading">
          {{ loading ? 'Registrieren...' : 'Registrieren' }}
        </button>
      </form>

      <p class="login-link">
        Bereits ein Konto?
        <RouterLink to="/login">Anmelden</RouterLink>
      </p>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { userApi } from '../api/userApi'

const router = useRouter()
const name = ref('')
const email = ref('')
const password = ref('')
const error = ref('')
const success = ref('')
const loading = ref(false)

async function handleRegister() {
  error.value = ''
  success.value = ''
  loading.value = true
  try {
    await userApi.register(name.value, email.value, password.value)
    success.value = 'Konto erfolgreich erstellt. Sie werden weitergeleitet…'
    setTimeout(() => router.push('/login'), 1500)
  } catch {
    error.value = 'Registrierung fehlgeschlagen. Bitte prüfen Sie Ihre Eingaben.'
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
  min-width: 400px;
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

.success-message {
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
