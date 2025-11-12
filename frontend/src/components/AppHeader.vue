<template>
  <header
    class="w-full bg-white shadow-sm border-b border-gray-200 sticky top-0 z-50"
  >
    <div class="max-w-6xl mx-auto flex justify-between items-center px-6 py-3">
      <!-- ロゴ -->
      <h1
        class="text-2xl font-bold text-blue-600 tracking-wide cursor-pointer"
        @click="goHome"
      >
        EMS Portal
      </h1>   
    </div>
  </header>
</template>

<script setup>
import { ref } from "vue";
import { useRouter } from "vue-router";

const router = useRouter();

const goHome = () => {
  router.push("/");
};
</script>
