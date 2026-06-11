<script setup lang="ts">
import { RouterView, RouterLink } from 'vue-router'
import { useI18n } from 'vue-i18n'

const { t } = useI18n()
</script>

<template>
  <div id="app">
    <nav class="nav">
      <span class="nav__brand">{{ t('nav.brand') }}</span>
      <RouterLink class="nav__link" to="/assets">{{ t('nav.assets') }}</RouterLink>
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
</style>
