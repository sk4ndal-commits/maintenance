import { createRouter, createWebHistory } from 'vue-router'
import AssetsView from '../views/AssetsView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      redirect: '/assets',
    },
    {
      path: '/assets',
      name: 'assets',
      component: AssetsView,
    },
    {
      path: '/assets/:id',
      name: 'asset-detail',
      component: () => import('../views/AssetDetailView.vue'),
    },
  ],
})

export default router
