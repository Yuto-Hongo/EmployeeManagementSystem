<template>
  <div class="min-h-screen flex flex-col items-center justify-center bg-gradient-to-br from-blue-50 to-indigo-100">
    <!-- ロゴ・タイトル -->
    <h1 class="text-4xl font-extrabold text-gray-800 mb-8 tracking-wide drop-shadow-sm">
      Employee Management System
    </h1>

    <!-- カード -->
    <div class="bg-white/90 shadow-xl rounded-2xl p-10 w-96 border border-gray-100 backdrop-blur-md transition-all hover:shadow-2xl hover:scale-[1.02] duration-200">
      <div class="flex flex-col gap-4">
        <button
          @click="goLogin"
          class="btn-primary"
        >
          ログイン
        </button>

        <button
          @click="goRegister"
          class="btn-secondary"
        >
          新規登録
        </button>
      </div>
    </div>

    <!-- フッター -->
    <p class="mt-10 text-gray-500 text-sm">© 2025 Employee Management App</p>
  </div>
</template>

<script setup>
import { useRouter } from "vue-router";
const router = useRouter();

const goLogin = () => router.push("/login");
const goRegister = () => router.push("/register");
</script>

<style scoped>
.btn-primary {
  @apply bg-blue-600 hover:bg-blue-700 text-white py-2.5 px-4 rounded-lg font-semibold shadow-md hover:shadow-lg transition-all duration-200;
}

.btn-secondary {
  @apply bg-emerald-500 hover:bg-emerald-600 text-white py-2.5 px-4 rounded-lg font-semibold shadow-md hover:shadow-lg transition-all duration-200;
}
</style>
