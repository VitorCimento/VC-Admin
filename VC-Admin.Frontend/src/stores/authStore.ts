import { loginUser } from "@/services/authService";
import type { LoginForm } from "@/types/auth";
import { defineStore } from "pinia";
import { computed, ref } from "vue";

export const useAuthStore = defineStore('auth', () => {
    const token = ref<string | null>(localStorage.getItem('jwt'))
    const expiresAt = ref<string | null>(localStorage.getItem('jwt_expires'))
    const userEmail = ref<string | null>(null)
    const loading = ref(false)
    const error = ref<string | null>(null)

    const isAuthenticated = computed(() => !!token.value && new Date(expiresAt.value || new Date(-2208988800000).toISOString()) > new Date())

    async function login(payload: LoginForm) {
        error.value = null
        loading.value = true

        try {
            const response = await loginUser(payload)

            token.value = response.token
            expiresAt.value = response.expiresAt
            userEmail.value = payload.email

            localStorage.setItem('jwt', response.token)
            localStorage.setItem('jwt_expires', response.expiresAt)
        } catch (err: any) {
            error.value = err.message
            throw err
        } finally {
            loading.value = false
        }
    }

    function logout() {
        token.value = null
        expiresAt.value = null
        userEmail.value = null
        localStorage.removeItem('jwt')
        localStorage.removeItem('jwt_expires')
    }

    return {
        token,
        expiresAt,
        userEmail,
        loading,
        error,
        isAuthenticated,
        login,
        logout
    }
})