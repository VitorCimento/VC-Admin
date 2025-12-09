<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import type { LoginForm } from '@/types/auth'
import { useAuthStore } from '@/stores/authStore'

const form = ref<LoginForm>({
  email: '',
  password: ''
})

const auth = useAuthStore()
const router = useRouter()

const handleLogin = async () => {
  try {
    await auth.login(form.value)
    router.push('/dashboard')
  } catch (err) {

  }
}
</script>

<template>
  <div class="flex flex-col min-h-screen items-center justify-center">
    <UCard class="w-full max-w-sm shadow-2xl">
      <template #header>
        <div class="flex justify-between items-center">
          <h1 class="text-lg font-semibold text-sky-700 dark:text-sky-300">VC-Admin</h1>
          <UColorModeButton />
        </div>
      </template>

      <UForm @submit.prevent="handleLogin" id="loginForm" class="space-y-2">
        <UFormField name="email">
          <UInput 
            v-model="form.email"
            type="email"
            placeholder="Informe seu e-mail"
            required
            trailing-icon="ph:at"
            class="w-full"
            color="secondary"
          />
        </UFormField>

        <UFormField name="password">
          <UInput
            v-model="form.password"
            type="password"
            placeholder="Informe sua senha"
            required
            trailing-icon="ph:password-duotone"
            class="w-full"
            color="secondary"
          />
        </UFormField>

        <p v-if="auth.error" class="text-red-500 text-sm mt-3 text-center">{{ auth.error }}</p>

        <UButton type="submit" block :loading="auth.loading" color="secondary">
          Entrar
        </UButton>

        <RouterLink to="/register">
          <UButton color="secondary" variant="outline" block :disabled="auth.loading" class="cursor-pointer">
            Não tem uma conta? Registre-se!
          </UButton>
        </RouterLink>
      </UForm>
    </UCard>
  </div>
</template>
