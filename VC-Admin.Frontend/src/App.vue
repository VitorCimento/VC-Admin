<script setup lang="ts">
import { pt_br } from '@nuxt/ui/locale'
</script>

<template>
  <UApp :locale="pt_br">
    <div class="min-h-screen min-w-screen bg-linear-to-br from-sky-900 dark:from-sky-700 via-sky-500 dark:via-sky-800 to-sky-50 dark:to-sky-950">
      <RouterView />
    </div>
  </UApp>
</template>

<style scoped></style>
