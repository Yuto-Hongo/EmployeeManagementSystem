<template>
  <form @submit.prevent="handleLogin" class="space-y-5">
    <div>
      <label class="block text-sm font-medium text-gray-700 mb-1">
        メールアドレス
      </label>
      <input
        v-model="email"
        type="email"
        placeholder="yourname@example.com"
        required
        class="w-full border border-gray-300 px-3 py-2 rounded-lg focus:ring-2 focus:ring-blue-400 focus:outline-none transition"
      />
    </div>

    <div>
      <label class="block text-sm font-medium text-gray-700 mb-1">
        パスワード
      </label>
      <div class="relative">
        <input
          v-model="password"
          :type="showPassword ? 'text' : 'password'"
          placeholder="••••••••"
          required
          class="w-full border border-gray-300 px-3 py-2 rounded-lg focus:ring-2 focus:ring-blue-400 focus:outline-none transition"
        />
        <button
          type="button"
          @click="togglePassword"
          class="absolute inset-y-0 right-3 flex items-center text-gray-500 hover:text-blue-600"
        >
          <i :class="showPassword ? 'fas fa-eye-slash' : 'fas fa-eye'"></i>
        </button>
      </div>
    </div>

    <button
      type="submit"
      class="w-full bg-blue-600 hover:bg-blue-700 text-white py-2.5 rounded-lg font-semibold transition"
    >
      ログイン
    </button>

    <p v-if="message" class="text-center text-red-500 text-sm font-medium mt-3">
      {{ message }}
    </p>
  </form>
</template>

<script setup>
import { ref } from "vue";
import apiClient from "@/utils/apiClient";
import { useRouter } from "vue-router";
import { jwtDecode } from "jwt-decode";

const router = useRouter();
const email = ref("");
const password = ref("");
const message = ref("");
const showPassword = ref(false);

const togglePassword = () => (showPassword.value = !showPassword.value);

// ログイン処理
const handleLogin = async () => {
  try {
    const { data } = await apiClient.post(
      "/auth/login",
      {
        email: email.value,
        password: password.value,
      },
      { skipGlobalErrorHandler: true }
    );

    const token = data.token;
    localStorage.setItem("token", token);

    const decoded = jwtDecode(token);
    if (decoded.Roles === "Admin") {
      router.push("/dashboard/admin");
    } else {
      router.push("/dashboard/general");
    }
  } catch (error) {
    console.error("ログイン失敗:", error);
    message.value = "メールアドレスまたはパスワードが正しくありません。";
  }
};
</script>
