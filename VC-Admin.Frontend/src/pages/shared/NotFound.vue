<script setup lang="ts">
</script>

<template>
  <section class="min-h-screen flex flex-col items-center justify-center text-center space-y-6 px-4">
    <!-- Icone -->
    <UIcon name="ph:warning-octagon" class="text-9xl dark:text-orange-500 text-orange-700 animate-bounce" />
    <!-- Texto -->
    <div class="space-y-2">
      <p class="text-sm font-semibold tracking-widest text-orange-900 dark:text-orange-600">404</p>
      <h1 class="text-3xl md:text-4xl font-extrabold">Página não encontrada</h1>
      <p class="text-sm text-surface-500 max-w-md">A página que você tentou acessar não existe, foi movida ou está temporariamente indisponível.</p>
    </div>
    <!-- Botão -->
     <section class="m-auto max-w-lg my-10 px-6">
      <RouterLink to="/dashboard"
        class="flex justify-center items-center bg-slate-800 text-white text-center py-4 px-6 rounded-xl hover:bg-slate-600 space-x-3 hover:shadow-2xl active:scale-95
          active:bg-slate-700 transition-all duration-150 cursor-pointer">
        <span>Voltar ao Início</span>
      </RouterLink>
     </section>
  </section>
</template>