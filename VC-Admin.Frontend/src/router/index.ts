import { useAuthStore } from '@/stores/authStore'
import { createRouter, createWebHistory, type RouteRecordRaw } from 'vue-router'

const routes: RouteRecordRaw[] = [
  {
    path: '/',
    redirect: '/login'
  },
  {
    path: '/login',
    name: 'login',
    component: () => import('@/pages/auth/Login.vue')
  },
  {
    path: '/dashboard',
    name: 'dashboard',
    meta: { requiresAuth: true },
    component: () => import('@/pages/dashboard/Dashboard.vue')
  },
  {
    path: '/register',
    name: 'register',
    component: () => import('@/pages/auth/Register.vue')
  },
  // Deixar essa rota por último. Trataiva de 404.
  {
    path: '/:pathWatch(.*)*',
    name: 'notFound',
    component: () => import('@/pages/shared/NotFound.vue')
  }
]

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: routes,
})

// Guarda de rota
router.beforeEach((to, _, next) => {
  const auth = useAuthStore()
  if (to.meta.requiresAuth && ! auth.isAuthenticated) {
    auth.logout()
    next('/login')
  } else {
    next()
  }
})

export default router
