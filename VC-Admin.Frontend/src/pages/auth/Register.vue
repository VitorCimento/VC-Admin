<script setup lang="ts">
import { registerUser } from '@/services/authService';
import type { RegisterForm } from '@/types/auth';
import { computed, reactive, ref } from 'vue';
import { useRouter } from 'vue-router';

const router = useRouter()
const toast = useToast()

const form = reactive<RegisterForm>({
  username: '',
  email: '',
  password: '',
  password_confirm: ''
})

// Password
const passwordFocused = ref(false)

const passwordRules = computed(() => ({
  minLength: form.password.length >= 8,
  lowercase: /[a-z]/.test(form.password),
  uppercase: /[A-Z]/.test(form.password),
  number: /\d/.test(form.password),
  symbol: /[^A-Za-z0-9]/.test(form.password)
}))

const passwordMatch = computed(() => {
  return form.password === form.password_confirm
})

const passwordValid = computed(() => {
  return Object.values(passwordRules.value).every(Boolean)
})

const ruleClass = (valid: boolean) => valid ? 'text-green-600 dark:text-green-400' : 'text-slate-400'


// Regra de submit
const canSubmit = computed(() => {
  return (
    form.username.trim() !== '' &&
    form.email.trim() !== '' &&
    passwordMatch.value &&
    passwordValid.value
  )
})

// Submit
const loading = ref(false)
const error = ref<string | null>(null)

const handleRegister = async () => {
  error.value = null
  loading.value = true

  try {
    await registerUser(form)
    
    toast.add({
      title: 'Registro concluído',
      description: 'Faça login para continuar',
      color: 'success',
      icon: 'ph:user-circle-check'
    })

    router.push('/login')

  } catch (err: unknown) {
    if (err instanceof Error) {
      error.value = err.message
    } else {
      error.value = 'Erro inesperado ao registrar usuário.'
    }
  } finally {
    loading.value = false
  }
}  
</script>

<template>
    <div class="flex flex-col min-h-screen items-center justify-center">
    <UCard class="w-full max-w-md shadow-2xl">
      <template #header>
        <div class="flex justify-between items-center">
          <h1 class="text-lg font-semibold text-sky-700 dark:text-sky-300">Registre-se na plataforma VC-Admin</h1>
          <UColorModeButton />
        </div>
      </template>

      <UForm @submit.prevent="handleRegister" id="registerForm" class="space-y-8">
        <UFormField name="username" label="Nome de Usuário" required>
          <UInput 
            v-model="form.username"
            type="text"
            placeholder="Informe seu nome"
            required
            trailing-icon="ph:user"
            class="w-full"
            color="secondary"
          />
        </UFormField>
        
        <UFormField name="email" label="E-Mail" required>
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

        <UFormField name="password" label="Senha" required>
          <UInput
            v-model="form.password"
            type="password"
            placeholder="Informe sua senha"
            required
            trailing-icon="ph:password-duotone"
            class="w-full"
            color="secondary"
            @focus="passwordFocused = true"
            @blur="passwordFocused = false"
          />
        </UFormField>

        <!-- Regras -->
        <transition name="fade">
          <div class="rounded-md border border-slate-200 dark:border-slate-700 p-3 text-sm space-y-1 bg-slate-50 dark:bg-slate-800">
            <p class="font-medium text-slate-600 dark:text-slate-300 mb-1">Sua senha deve conter:</p>

            <ul class="space-y-1">
              <li :class="ruleClass(passwordRules.minLength)">
                <span class="w-4 text-center">
                  {{ passwordRules.minLength ? "✔" : "⏳"}}
                </span>
                <span>Mínimo de 8 caracteres</span>
              </li>
              <li :class="ruleClass(passwordRules.lowercase)">
                <span class="w-4 text-center">
                  {{ passwordRules.lowercase ? "✔" : "⏳"}}
                </span>
                <span>Conter caracteres minúsculos</span>
              </li>
              <li :class="ruleClass(passwordRules.uppercase)">
                <span class="w-4 text-center">
                  {{ passwordRules.uppercase ? "✔" : "⏳"}}
                </span>
                <span>Conter caracteres maiúsculos</span>
              </li>
              <li :class="ruleClass(passwordRules.number)">
                <span class="w-4 text-center">
                  {{ passwordRules.number ? "✔" : "⏳"}}
                </span>
                <span>Conter números</span>
              </li>
              <li :class="ruleClass(passwordRules.symbol)">
                <span class="w-4 text-center">
                  {{ passwordRules.symbol ? "✔" : "⏳"}}
                </span>
                <span>Conter símbolos</span>
              </li>
            </ul>
          </div>
        </transition>

        <UFormField name="password_confirm" label="Confirmação de senha" required :error="passwordMatch ? undefined : 'Senhas não conferem'">
          <UInput
            v-model="form.password_confirm"
            type="password"
            placeholder="Confirme sua senha"
            required
            trailing-icon="ph:password-fill"
            class="w-full"
            color="secondary"
          />
        </UFormField>

        <p v-if="error" class="text-sm text-red-500 text-center">
          {{ error }}
        </p>

        <div class="flex justify-between items-center space-x-2 mt-15">
          <UButton type="submit" block color="secondary" :disabled="!canSubmit || loading" :loading="loading">
            Registrar
          </UButton>
          <UButton type="button" block color="secondary" variant="outline" @click="router.push('/login')">
            Voltar
          </UButton>
        </div>
      </UForm>
    </UCard>
  </div> 
</template>

<style scoped>
  .fade-enter-active {
    transition: opacity 0.2s ease;
  }

  .fade-enter-from,
  .fade-leave-to {
    opacity: 0;
  }
</style>