<script setup lang="ts">
import { RouterView, RouterLink, useRouter, useRoute } from 'vue-router'
import { useI18n } from 'vue-i18n'
import { computed } from 'vue'

const { t } = useI18n()
const router = useRouter()
const route = useRoute()

function getTokenRole(): string | null {
  const token = localStorage.getItem('jwt')
  if (!token) return null
  try {
    const payload = JSON.parse(atob(token.split('.')[1]))
    return payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] ?? null
  } catch { return null }
}

// Re-evaluate on every route change so nav updates after login/logout
const isAdmin = computed(() => { void route.path; return getTokenRole() === 'Admin' })
const isLoggedIn = computed(() => { void route.path; return !!localStorage.getItem('jwt') })

function logout() {
  localStorage.removeItem('jwt')
  router.push('/login')
}
</script>

<template>
  <div id="app">
    <nav class="nav">
      <span class="nav__brand">{{ t('nav.brand') }}</span>
      <RouterLink class="nav__link" to="/assets">{{ t('nav.assets') }}</RouterLink>
      <RouterLink v-if="isAdmin" class="nav__link" to="/users">Benutzer</RouterLink>
      <button v-if="isLoggedIn" class="nav__logout" @click="logout">Abmelden</button>
    </nav>
    <main>
      <RouterView />
    </main>
  </div>
</template>

<style>
.nav {
  background: var(--color-primary);
  color: #fff;
  padding: 0 32px;
  height: 56px;
  display: flex;
  align-items: center;
  gap: 32px;
}

.nav__brand {
  font-weight: 700;
  font-size: 1.1rem;
}

.nav__link {
  color: #93c5fd;
  text-decoration: none;
  font-size: 0.95rem;
  font-weight: 500;
}

.nav__link:hover,
.nav__link.router-link-active {
  color: #fff;
}

.nav__logout {
  margin-left: auto;
  background: transparent;
  border: 1px solid rgba(255,255,255,0.4);
  color: #fff;
  padding: 4px 14px;
  border-radius: 4px;
  cursor: pointer;
  font-size: 0.9rem;
}

.nav__logout:hover {
  background: rgba(255,255,255,0.15);
}
</style>
