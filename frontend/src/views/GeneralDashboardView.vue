<template>
  <div
    class="min-h-[calc(100vh-64px)] flex flex-col items-center justify-start bg-gray-50 py-10"
  >
    <div
      class="bg-white w-full max-w-2xl shadow-lg rounded-2xl p-8 border border-gray-100"
    >
      <h1 class="text-3xl font-bold text-gray-800 text-center mb-6">
        マイプロフィール
      </h1>

      <!-- 情報カード -->
      <div v-if="employee" class="space-y-3 text-gray-700">
        <div class="flex justify-between border-b pb-2">
          <span class="font-semibold">氏名</span>
          <span>{{ employee.fullName }}</span>
        </div>
        <div class="flex justify-between border-b pb-2">
          <span class="font-semibold">生年月日</span>
          <span>{{ formatDate(employee.dateOfBirth) }}</span>
        </div>
        <div class="flex justify-between border-b pb-2">
          <span class="font-semibold">性別</span>
          <span>{{ employee.gender }}</span>
        </div>
        <div class="flex justify-between border-b pb-2">
          <span class="font-semibold">住所</span>
          <span>{{ employee.address }}</span>
        </div>
        <div class="flex justify-between border-b pb-2">
          <span class="font-semibold">入社日</span>
          <span>{{ formatDate(employee.joinDate) }}</span>
        </div>
        <div class="flex justify-between border-b pb-2">
          <span class="font-semibold">勤務先</span>
          <span>{{ employee.currentWorkplace }}</span>
        </div>
        <div class="flex justify-between">
          <span class="font-semibold">メール</span>
          <span>{{ employee.user?.email }}</span>
        </div>
      </div>

      <!-- ボタン群 -->
      <div class="mt-8 flex flex-col gap-3">
        <button
          @click="isEditOpen = true"
          class="w-full bg-blue-500 hover:bg-blue-600 text-white py-2 rounded-lg transition"
        >
          プロフィール編集
        </button>

        <button
          @click="isPasswordModalOpen = true"
          class="w-full bg-yellow-500 hover:bg-yellow-600 text-white py-2 rounded-lg transition"
        >
          パスワード変更
        </button>
      </div>
    </div>

    <!-- 編集モーダル -->
    <transition name="slide-fade">
      <div
        v-if="isEditOpen"
        class="fixed right-0 top-0 h-full w-full md:w-1/3 bg-white shadow-2xl p-6 z-50 overflow-auto"
      >
        <h2 class="text-xl font-semibold mb-4">プロフィール編集</h2>

        <div class="space-y-3">
          <label class="block text-sm font-medium">氏名</label>
          <input
            v-model="editData.fullName"
            type="text"
            class="w-full border rounded px-3 py-2"
          />

          <label class="block text-sm font-medium">住所</label>
          <input
            v-model="editData.address"
            type="text"
            class="w-full border rounded px-3 py-2"
          />

          <label class="block text-sm font-medium">勤務先</label>
          <input
            v-model="editData.currentWorkplace"
            type="text"
            class="w-full border rounded px-3 py-2"
          />

          <label class="block text-sm font-medium">メールアドレス</label>
          <input
            v-model="editData.user.email"
            type="email"
            class="w-full border rounded px-3 py-2"
          />
          <p v-if="emailError" class="text-red-500 text-sm mb-3">
            {{ emailError }}
          </p>

          <div class="flex justify-between mt-6">
            <button
              @click="updateProfile"
              class="btn-primary px-4 py-2 rounded"
            >
              保存
            </button>
            <button
              @click="
              isEditOpen = false;
              emailError = '';
              fetchEmployeeInfo();
              "
              class="btn-outline px-4 py-2 rounded"
            >
              閉じる
            </button>
          </div>
        </div>
      </div>
    </transition>

    <!-- パスワード変更モーダル（既存） -->
    <div
      v-if="isPasswordModalOpen"
      class="fixed inset-0 bg-gray-900 bg-opacity-50 flex items-center justify-center"
      @click.self="isPasswordModalOpen = false"
    >
      <div class="bg-white p-6 rounded shadow-md w-96">
        <h2 class="text-xl font-semibold mb-4">パスワード変更</h2>
        <Form
          :key="isPasswordModalOpen"
          :validation-schema="passwordSchema"
          :validateOnMount="false"
          @submit="handleChangePassClick"
          v-slot="{ errors }"
        >
          <label class="block text-left text-sm text-gray-700 mb-1"
            >現在のパスワード</label
          >
          <Field
            name="currentPassword"
            type="password"
            class="w-full border rounded px-3 py-2 mb-3"
          />
          <ErrorMessage
            name="currentPassword"
            class="text-red-500 text-sm mb-2"
          />

          <label class="block text-left text-sm text-gray-700 mb-1"
            >新しいパスワード</label
          >
          <Field
            name="newPassword"
            type="password"
            class="w-full border rounded px-3 py-2 mb-3"
          />
          <ErrorMessage name="newPassword" class="text-red-500 text-sm mb-2" />

          <label class="block text-left text-sm text-gray-700 mb-1"
            >新しいパスワード（確認）</label
          >
          <Field
            name="confirmPassword"
            type="password"
            class="w-full border rounded px-3 py-2 mb-3"
          />
          <ErrorMessage
            name="confirmPassword"
            class="text-red-500 text-sm mb-2"
          />

          <div class="flex justify-between mt-4">
            <button
              type="submit"
              class="btn-primary px-4 py-2 rounded"
            >
              変更
            </button>

            <button
              type="button"
              @click="isPasswordModalOpen = false"
              class="btn-outline px-4 py-2 rounded"
            >
              キャンセル
            </button>
          </div>
        </Form>
      </div>
    </div>

    <!-- ログアウト -->
      <button
        @click="logout"
        class="btn-danger px-4 py-2 rounded-lg transition mt-5"
      >
        ログアウト
      </button>
  </div>
</template>

<script setup>
import { ref, onMounted } from "vue";
import { useRouter } from "vue-router";
import { formatDate } from "@/utils/format";
import { Form, Field, ErrorMessage } from "vee-validate";
import { passwordSchema } from "@/validation/passwordSchemas";
import { validateEmailFormat } from "@/validation/employeeSchemas";
import apiClient from "@/utils/apiClient";
import * as yup from "yup";
import { showConfirm, showSuccess, showWarning, showError } from "@/utils/alertService";

const router = useRouter();
const employee = ref(null);
const editData = ref({
  fullName: "",
  address: "",
  currentWorkplace: "",
  user: { email: "" },
});
const isEditOpen = ref(false);
const isPasswordModalOpen = ref(false);
const passwordData = {
  currentPassword: "",
  newPassword: "",
  confirmPassword: "",
};
const emailError = ref("");

// 情報取得
const fetchEmployeeInfo = async () => {
  try {
    const response = await apiClient.get("/general/info", {});
    employee.value = response.data;
    editData.value = { ...response.data, user: { ...response.data.user } };
  } catch (error) {
    console("従業員情報を取得できません:", error);
  }
};

// 更新
const updateProfile = async () => {
  emailError.value = "";
  const checkResult = validateEmailFormat(editData.value.user.email);

  if (checkResult) {
    emailError.value = checkResult;
    return;
  }

  try {
    await apiClient.put("/general/edit", editData.value, {});
    isEditOpen.value = false;
    fetchEmployeeInfo();
  } catch (error) {
    console("従業員情報を取得できません:", error);
  }
};

// パスワード変更
const changePassword = async (formData) => {
  // request Bodyを設定
  const dataToSend = {
    currentPassword: formData.currentPassword,
    newPassword: formData.newPassword,
    confirmPassword: formData.confirmPassword,
  };

  try {
    const response = await apiClient.put(
        "/auth/change-password", 
        dataToSend, 
        {skipGlobalErrorHandler: true,}
    );
    showSuccess(response.data.message);
    isPasswordModalOpen.value = false;
  } catch (error) {
    console.error("パスワード変更エラー", error);
    if (error.response && error.response.status === 401) {
      showWarning("現在のパスワードが異なります。");
    } else if (error.response && error.response.status === 400) {
      showWarning(
        "新しいパスワードの入力形式に誤りがあります（大・小文字を含む英数字かつ8文字以上の値）"
      );
    } else {
      showError("パスワード変更エラー");
    }
  }
};

// ログアウト処理
const logout = () => {
  localStorage.removeItem("token");
  router.push("/login");
};

// パスワード変更機能にバリデーションスキーマを適用
const handleChangePassClick = async (values) => {
  await changePassword(values);
};

onMounted(fetchEmployeeInfo);
</script>

<style scoped>
.slide-fade-enter-active,
.slide-fade-leave-active {
  transition: all 0.4s ease;
}
.slide-fade-enter-from,
.slide-fade-leave-to {
  transform: translateX(100%);
  opacity: 0;
}
</style>
