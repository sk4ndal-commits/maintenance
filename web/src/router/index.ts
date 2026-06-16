import { createRouter, createWebHistory } from 'vue-router'
import AssetsView from '../views/AssetsView.vue'

function getTokenRole(): string | null {
  const token = localStorage.getItem('jwt')
  if (!token) return null
  try {
    const payload = JSON.parse(atob(token.split('.')[1]))
    return payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] ?? null
  } catch {
    return null
  }
}

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      redirect: '/assets',
    },
    {
      path: '/login',
      name: 'login',
      component: () => import('../views/LoginView.vue'),
    },
    {
      path: '/register',
      name: 'register',
      component: () => import('../views/RegisterView.vue'),
    },
    {
      path: '/assets',
      name: 'assets',
      component: AssetsView,
      meta: { requiresAuth: true },
    },
    {
      path: '/assets/:id',
      name: 'asset-detail',
      component: () => import('../views/AssetDetailView.vue'),
      meta: { requiresAuth: true },
    },
    {
      path: '/users',
      name: 'users',
      component: () => import('../views/UsersView.vue'),
      meta: { requiresAuth: true, requiresRole: ['Admin'] },
    },
    {
      path: '/audit',
      name: 'audit',
      component: () => import('../views/AdminAuditLogsView.vue'),
      meta: { requiresAuth: true, requiresRole: ['Admin'] },
    },
  ],
})

router.beforeEach((to) => {
  const role = getTokenRole()
  if (to.meta.requiresAuth && !role) {
    return { name: 'login' }
  }
  if (to.meta.requiresRole && !(to.meta.requiresRole as string[]).includes(role!)) {
    return { name: 'assets' }
  }
})

export default router
