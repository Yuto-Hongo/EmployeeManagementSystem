<template>
  <AuthLayout>
      <!-- ロゴタイトル -->
      <h1 class="text-4xl font-bold text-gray-800 mb-10 tracking-wide text-center">
        Employee Management System
      </h1>

      <!-- ログインカード -->
      <div
        class="w-full max-w-md bg-white shadow-lg rounded-2xl p-10 border border-gray-100 transition-all hover:shadow-xl"
      >
        <LoginForm />
      </div>

      <!-- ホームボタン -->
      <div class="text-center mt-6">
        <button
          @click="goHome"
          class="text-gray-500 hover:text-blue-600 transition-colors underline"
        >
          ← ホームへ戻る
        </button>
      </div>

      <!-- フッター -->
      <footer class="mt-10 text-sm text-gray-400 text-center">
        © 2025 Employee Management System
      </footer>
  </AuthLayout>
</template>

<script setup>
import AuthLayout from "../layouts/AuthLayout.vue";
import LoginForm from "../components/LoginForm.vue";
import { useRouter } from "vue-router";
const router = useRouter();

// ホーム画面へ遷移処理
const goHome = () => router.push("/");
</script>