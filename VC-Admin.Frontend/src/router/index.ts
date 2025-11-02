import NotFound from '@/pages/shared/NotFound.vue'
import { createRouter, createWebHistory, type RouteRecordRaw } from 'vue-router'

const routes: RouteRecordRaw[] = [
  // Deixar essa rota por último. Trataiva de 404.
  {
    path: '/:pathWatch(.*)*',
    name: 'notFound',
    component: NotFound
  }
]

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: routes,
})

export default router
