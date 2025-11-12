<template>
  <div class="min-h-screen flex flex-col bg-gray-50 text-gray-800 font-sans">
    <AppHeader />
      <main class="flex-grow w-full flex justify-center items-start px-6 py-8">
        <router-view />
      </main>
  </div>
</template>

<script setup>
import AppHeader from "./components/AppHeader.vue";
</script>