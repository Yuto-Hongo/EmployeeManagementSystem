<template>
  <!-- カード -->
  <div
    class="bg-white shadow-lg rounded-2xl p-10 w-full max-w-md border border-gray-100 transition-all hover:shadow-xl"
  >
    <h1 class="text-3xl font-bold text-center text-gray-800 mb-8">新規登録</h1>

    <!-- 登録フォーム -->
    <form @submit.prevent="handleRegister" class="space-y-5">
      <!-- メール -->
      <div>
        <label class="block text-sm font-medium text-gray-700 mb-1"
          >メールアドレス</label
        >
        <input
          v-model="form.email"
          type="email"
          placeholder="yourname@example.com"
          required
          class="w-full border border-gray-300 rounded-lg px-3 py-2 focus:ring-2 focus:ring-blue-400 focus:outline-none transition"
        />
      </div>

      <!-- パスワード -->
      <div>
        <label class="block text-sm font-medium text-gray-700 mb-1"
          >パスワード
        </label>
        <div class="relative">
          <input
            v-model="form.password"
            :type="showPassword ? 'text' : 'password'"
            minlength="8"
            required
            placeholder="••••••••"
            class="w-full border border-gray-300 rounded-lg px-3 py-2 focus:ring-2 focus:ring-blue-400 focus:outline-none transition"
          />
          <button
            type="button"
            @click="togglePassword"
            class="absolute inset-y-0 right-3 flex items-center text-gray-500 hover:text-blue-600"
          >
            <i :class="showPassword ? 'fas fa-eye-slash' : 'fas fa-eye'"></i>
          </button>
        </div>
        <p v-if="passwordError" class="text-red-500 text-sm mt-1">
          {{ passwordError }}
        </p>
      </div>

      <!-- パスワード確認 -->
      <div>
        <label class="block text-sm font-medium text-gray-700 mb-1"
          >パスワード（確認）</label
        >
        <div class="relative">
          <input
            v-model="form.confirmPassword"
            :type="showPassword ? 'text' : 'password'"
            minlength="8"
            required
            class="w-full border border-gray-300 rounded-lg px-3 py-2 focus:ring-2 focus:ring-blue-400 focus:outline-none transition"
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

      <!-- エラーメッセージ -->
      <p v-if="message" class="text-center text-red-500 text-sm font-medium">
        {{ message }}
      </p>

      <!-- 登録ボタン -->
      <button
        type="submit"
        class="w-full bg-green-600 hover:bg-green-700 text-white py-2.5 rounded-lg font-semibold transition"
      >
        登録
      </button>
    </form>

    <!-- ログインへリンク -->
    <div class="text-center mt-6">
      <p class="text-gray-600 text-sm">
        既にアカウントをお持ちの方は
        <span
          class="text-blue-600 hover:underline cursor-pointer"
          @click="goLogin"
        >
          ログイン
        </span>
      </p>
    </div>
  </div>
</template>

<script setup>
import { ref } from "vue";
import apiClient from "@/utils/apiClient";
import { useRouter } from "vue-router";
import { jwtDecode } from "jwt-decode";
import { showConfirm, showSuccess, showWarning, showError } from "@/utils/alertService";

const router = useRouter();

// ログイン時に必要な値の状態
const form = ref({
  email: "",
  password: "",
  confirmPassword: "",
});

// エラーメッセージの状態
const message = ref("");
const passwordError = ref("");

// パスワード表示/非表示を管理
const showPassword = ref(false);
const togglePassword = () => (showPassword.value = !showPassword.value);

// 💡 パスワードのバリデーション
const validatePassword = (password) => {
  // 8文字以上、大文字・小文字・数字をそれぞれ1つ以上含む正規表現
  const complexityRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$/;

  if (password.length < 8) {
    // 現在の文字数を含む警告を出力
    return `このテキストは8文字以上で指定してください（現在は${password.length}文字です）`;
  }

  if (!complexityRegex.test(password)) {
    // 複雑性の警告を出力
    return "パスワードは8文字以上で、大文字・小文字・数字を含めてください";
  }

  return ""; // バリデーション成功
};

// 新規従業員登録処理
const handleRegister = async () => {
  message.value = "";
  passwordError.value = ""; // エラーメッセージをリセット

  // 1. パスワードバリデーションチェック
  const complexityCheck = validatePassword(form.value.password);
  if (complexityCheck) {
    passwordError.value = complexityCheck;
    return;
  }

  if (form.value.password !== form.value.confirmPassword) {
    message.value = "パスワードが一致しません";
    return;
  }

  try {
    const { data } = await apiClient.post(
      "/registration/signup",
      form.value,
      {skipGlobalErrorHandler: true,}
    );

    if (data.token) {
      localStorage.setItem("token", data.token);
      const decode = jwtDecode(data.token);
      const role = decode.Roles;

      showSuccess("登録が完了しました。");
      router.push(role === "Admin" ? "/dashboard/admin" : "/dashboard/general");
    } else {
      showSuccess("登録が完了しました。ログインしてください。");
      router.push("/login");
    }
  } catch (error) {
    console.error("登録エラー:", error);
    if (error.response?.status === 409) {
      message.value = "このメールアドレスは既に登録されています。";
    } else {
      showError("登録エラー");
    }
  }
};

// ログイン画面へ遷移処理
const goLogin = () => router.push("/login");
</script>
