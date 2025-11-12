<template>
  <div class="min-h-screen bg-gray-50 p-6" @click="handleOutsideClick">
    <h1 class="text-2xl font-bold text-gray-800 mb-4">Admin Dashboard</h1>

    <!-- ハンバーガーメニュー -->
    <div class="relative inline-block">
      <button
        @click.stop="toggleMenu"
        class="p-2 rounded hover:bg-gray-200 focus:outline-none"
      >
        <i class="fa-solid fa-bars text-gray-700 text-xl"></i>
      </button>

      <div
        v-if="isMenuOpen"
        class="absolute mt-2 w-48 bg-white border rounded shadow-lg z-10 animate-fadeIn"
        @click.stop
      >
        <button
          @click="openCreateModal"
          class="block w-full text-left px-4 py-2 hover:bg-gray-100"
        >
          + 新規登録
        </button>

        <button
          @click="openChangePasswordModal"
          class="block w-full text-left px-4 py-2 hover:bg-gray-100"
        >
          パスワード変更
        </button>

        <button
          @click="logout"
          class="block w-full text-left px-4 py-2 hover:bg-gray-100 text-red-500"
        >
          ログアウト
        </button>
      </div>
    </div>

    <!-- 検索フォームトグルボタン -->
    <div class="flex justify-end mb-2">
      <button
        @click="isSearchOpen = !isSearchOpen"
        class="bg-blue-500 hover:bg-blue-600 text-white px-4 py-2 rounded flex items-center transition"
      >
        <i class="fa-solid fa-magnifying-glass mr-2"></i>
        {{ isSearchOpen ? "検索条件を閉じる" : "検索条件を表示" }}
      </button>
    </div>

    <!-- 検索フォーム -->
    <transition name="slide-down">
      <div
        v-show="isSearchOpen"
        class="bg-white p-4 rounded-lg shadow mb-4 flex flex-wrap gap-4 items-end border border-gray-200"
      >
        <div>
          <label class="block text-sm text-gray-700 mb-1">氏名</label>
          <input
            v-model="searchParams.name"
            type="text"
            class="border rounded px-2 py-1"
          />
        </div>

        <div>
          <label class="block text-sm text-gray-700 mb-1">勤務先</label>
          <input
            v-model="searchParams.workplace"
            type="text"
            class="border rounded px-2 py-1"
          />
        </div>

        <div>
          <label class="block text-sm text-gray-700 mb-1">入社日（From）</label>
          <input
            v-model="searchParams.joinDateFrom"
            type="date"
            class="border rounded px-2 py-1"
          />
        </div>

        <div>
          <label class="block text-sm text-gray-700 mb-1">入社日（To）</label>
          <input
            v-model="searchParams.joinDateTo"
            type="date"
            class="border rounded px-2 py-1"
          />
        </div>

        <div>
          <label class="block text-sm text-gray-700 mb-1">条件モード</label>
          <select v-model="searchParams.mode" class="border rounded px-2 py-1">
            <option value="and">AND</option>
            <option value="or">OR</option>
          </select>
        </div>

        <div>
          <label class="block text-sm text-gray-700 mb-1">ソート対象</label>
          <select
            v-model="searchParams.sortBy"
            class="border rounded px-2 py-1"
          >
            <option value="id">ID</option>
            <option value="name">氏名</option>
            <option value="joinDate">入社日</option>
            <option value="workplace">勤務先</option>
          </select>
        </div>

        <div>
          <label class="block text-sm text-gray-700 mb-1">並び順</label>
          <select
            v-model="searchParams.sortOrder"
            class="border rounded px-2 py-1"
          >
            <option value="asc">昇順</option>
            <option value="desc">降順</option>
          </select>
        </div>

        <!-- スキル選択起動モーダル -->
        <button @click="openSkillModal" class="btn-secondary px-4 py-2 rounded">
          スキルを選択
        </button>

        <!-- 選択されたスキルタグ -->
        <div v-if="selectedSkills.length" class="mb-4">
          <h3 class="font-semibold text-gray-700 mb-2">選択中のスキル:</h3>
          <div class="flex flex-wrap gap-2">
            <span
              v-for="skill in selectedSkills"
              :key="skill.id"
              class="bg-blue text-blue-700 px-3 py-1 rounded-full text-sm flex items-center"
            >
              <img
                v-if="skill.iconPath"
                :src="`http://localhost:5211/${skill.iconPath}`"
                :alt="skill.name"
                class="w-4 h-4 mr-1"
              />

              {{ skill.name }}
              <button
                @click="cancelSelectedSkill(skill)"
                class="ml-2 text-red-500 hover:text-red-700"
              >
                ×
              </button>
            </span>
          </div>
        </div>

        <button
          @click="searchEmployees"
          class="bg-blue-500 hover:bg-blue-600 text-white px-4 py-2 rounded"
        >
          検索
        </button>

        <button
          @click="clearSearch"
          class="bg-gray-300 hover:bg-gray-400 text-gray-800 px-4 py-2 rounded"
        >
          クリア
        </button>
      </div>
    </transition>

    <!-- スキル選択モーダル -->
    <transition name="fade">
      <div
        v-if="isSkillModalOpen"
        class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50"
      >
        <div class="bg-white rounded-lg shadow-lg w-96 p-6">
          <h3 class="text-lg font-semibold mb-4">スキルを選択</h3>

          <input
            v-model="skillSearch"
            @input="onInput"
            @onCompositionstart="isComposing = true"
            @onCompositionend="onCompositionEnd"
            placeholder="例: C#, Vue, MySQL..."
            class="w-full border rounded px-3 py-2 mb-2"
          />

          <!-- サジェスト一覧 -->
          <ul
            v-if="filteredSkills.length"
            class="bg-white border rounded shadow-md max-h-48 overflow-y-auto"
          >
            <li
              v-for="skill in filteredSkills"
              :key="skill.id"
              @click="toggleSkill(skill)"
              class="px-3 py-2 cursor-pointer flex items-center transition"
            >
              <img
                v-if="skill.iconPath"
                :src="`http://localhost:5211/${skill.iconPath}`"
                :alt="skill.name"
                class="w-4 h-4 mr-2"
              />
              {{ skill.name }}
            </li>
          </ul>

          <div class="mt-4 flex justify-end gap-2">
            <button
              @click="isSkillModalOpen = false"
              class="btn-outline px-4 py-2 rounded"
            >
              閉じる
            </button>
            <button
              @click="confirmSkillSelection"
              class="btn-secondary px-4 py-2 rounded"
            >
              決定
            </button>
          </div>
        </div>
      </div>
    </transition>

    <!-- 従業員一覧テーブル -->
    <table class="table-auto border-collapse border w-full bg-white shadow-sm">
      <thead>
        <tr class="bg-gray-100 text-left">
          <th class="border px-3 py-2 text-center">ID</th>
          <th class="border px-3 py-2 text-center">氏名</th>
          <th class="border px-3 py-2 text-center">メール</th>
          <th class="border px-3 py-2 text-center">住所</th>
          <th class="border px-3 py-2 text-center">勤務先</th>
          <th class="border px-3 py-2 text-center">入社日</th>
          <th class="border px-3 py-2 text-center">操作</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="emp in employees" :key="emp.id" class="hover:bg-gray-50">
          <td class="border px-3 py-2 text-center">{{ emp.id }}</td>
          <td class="border px-3 py-2 text-center">{{ emp.fullName }}</td>
          <td class="border px-3 py-2 text-center">{{ emp.user?.email }}</td>
          <td class="border px-3 py-2 text-center">{{ emp.address }}</td>
          <td class="border px-3 py-2 text-center">
            {{ emp.currentWorkplace }}
          </td>
          <td class="border px-3 py-2 text-center">
            {{ formatDate(emp.joinDate) }}
          </td>

          <!-- 三点リーダー -->
          <td class="border px-3 py-2 text-center">
            <div class="relative inline-block text-left">
              <button
                @click.stop="toggleRowMenu(emp.id)"
                class="p-1 hover:bg-gray-200 rounded"
              >
                <i class="fa-solid fa-ellipsis-vertical"></i>
              </button>

              <div
                v-if="openRowMenuID === emp.id"
                class="absolute right-0 mt-2 w-36 bg-white border rounded shadow-lg z-10 animate-fadeIn"
                @click.stop
              >
                <!-- 詳細表示ボタン -->
                <button
                  @click="viewEmployee(emp.id)"
                  class="block w-full text-left px-4 py-2 hover:bg-gray-100"
                >
                  詳細を表示
                </button>

                <!-- パスワード初期化ボタン -->
                <button
                  @click="confirmReset(emp.id)"
                  class="block w-full text-left px-4 py-2 hover:bg-gray-100"
                >
                  パスワード初期化
                </button>

                <!-- 従業員削除ボタン -->
                <button
                  @click="deleteEmployee(emp.id)"
                  class="block w-full text-left px-4 py-2 text-red-500 hover:bg-gray-100"
                >
                  削除
                </button>
              </div>
            </div>
          </td>
        </tr>
      </tbody>
    </table>

    <!-- 件数表示 -->
    <div class="mt-4 text-gray-600 text-sm">
      全 {{ pagination.totalCount }} 件中
      {{ pagination.currentPage }} ページ目（全
      {{ pagination.totalPages }} ページ）
    </div>

    <!-- ページ移動 -->
    <div class="flex justify-center items-center mt-2 space-x-4">
      <button
        @click="gotoPage(searchParams.page - 1)"
        :disabled="searchParams.page === 1"
        class="px-3 py-1 border rounded hover:bg-gray-100 disabled:opacity-50"
      >
        ← 前へ
      </button>

      <span class="text-gray-700">
        {{ pagination.currentPage }} / {{ pagination.totalPages }}
      </span>

      <button
        @click="gotoPage(searchParams.page + 1)"
        :disabled="searchParams.page === pagination.totalPages"
        class="px-3 py-1 border rounded hover:bg-gray-100 disabled:opacity-50"
      >
        次へ →
      </button>
    </div>

    <!-- 登録モーダル -->
    <transition name="fade">
      <div
        v-if="isCreateModalOpen"
        class="fixed inset-0 bg-gray-900 bg-opacity-50 flex items-start items-center justify-center overflow-auto"
        @click.self="isCreateModalOpen = false"
      >
        <div
          class="bg-white p-6 rounded shadow-md w-96 mt-20 max-h-[85vh] overflow-y-auto animate-fadeIn"
        >
          <h2 class="text-xl font-semibold mb-4">新規従業員登録</h2>

          <!--  VeeValidateフォーム -->
          <Form
            :key="isCreateModalOpen"
            :validation-schema="employeeAddSchema"
            :validateOnMount="false"
            @submit="handleCreateClick"
            v-slot="{ errors }"
          >
            <!-- 氏名 -->
            <label class="block text-left text-sm text-gray-700 mb-1"
              >氏名</label
            >
            <Field
              name="fullName"
              type="text"
              class="w-full border rounded px-3 py-2 mb-1"
              placeholder="例：山田 太郎"
            />
            <ErrorMessage name="fullName" class="text-red-500 text-sm mb-2" />

            <!-- メールアドレス -->
            <label class="block text-left text-sm text-gray-700 mb-1"
              >メールアドレス</label
            >
            <Field
              name="email"
              type="email"
              class="w-full border rounded px-3 py-2 mb-1"
              placeholder="example@email.com"
            />
            <ErrorMessage name="email" class="text-red-500 text-sm mb-2" />

            <!-- 生年月日 -->
            <label class="block text-left text-sm text-gray-700 mb-1"
              >生年月日</label
            >
            <Field
              name="dateOfBirth"
              type="date"
              class="w-full border rounded px-3 py-2 mb-1"
            />
            <ErrorMessage
              name="dateOfBirth"
              class="text-red-500 text-sm mb-2"
            />

            <!-- 住所 -->
            <label class="block text-left text-sm text-gray-700 mb-1"
              >住所</label
            >
            <Field
              name="address"
              type="text"
              class="w-full border rounded px-3 py-2 mb-1"
            />
            <ErrorMessage name="address" class="text-red-500 text-sm mb-2" />

            <!-- 性別 -->
            <label class="block text-left text-sm text-gray-700 mb-1"
              >性別</label
            >
            <div class="flex space-x-4 mb-2">
              <label
                ><Field name="gender" type="radio" value="男性" />男性</label
              >
              <label
                ><Field name="gender" type="radio" value="女性" />女性</label
              >
              <label
                ><Field
                  name="gender"
                  type="radio"
                  value="その他"
                />その他</label
              >
            </div>
            <ErrorMessage name="gender" class="text-red-500 text-sm mb-2" />

            <!-- 入社日 -->
            <label class="block text-left text-sm text-gray-700 mb-1"
              >入社日</label
            >
            <Field
              name="joinDate"
              type="date"
              class="w-full border rounded px-3 py-2 mb-1"
            />
            <ErrorMessage name="joinDate" class="text-red-500 text-sm mb-2" />

            <!-- 勤務先 -->
            <label class="block text-left text-sm text-gray-700 mb-1"
              >勤務先</label
            >
            <Field
              name="currentWorkplace"
              type="text"
              class="w-full border rounded px-3 py-2 mb-1"
            />
            <ErrorMessage
              name="currentWorkplace"
              class="text-red-500 text-sm mb-2"
            />

            <div
              class="flex justify-between mt-4 sticky bottom-0 bg-white py-2"
            >
              <button type="submit" class="btn-secondary px-4 py-2 rounded">
                登録
              </button>
              <button
                type="button"
                @click="isCreateModalOpen = false"
                class="btn-outline px-4 py-2 rounded"
              >
                キャンセル
              </button>
            </div>
          </Form>
        </div>
      </div>
    </transition>

    <!-- 詳細モーダル -->
    <transition name="fade">
      <div
        v-if="isDetailModalOpen"
        class="fixed inset-0 bg-gray-900 bg-opacity-50 flex items-center justify-center z-30"
        @click.self="isDetailModalOpen = false"
      >
        <div
          class="bg-white p-6 rounded-lg shadow-lg w-[480px] max-h-[90vh] overflow-y-auto animate-fadeIn"
        >
          <h2 class="text-xl font-semibold mb-4 border-b pb-2">
            従業員情報詳細
          </h2>

          <div v-if="selectedEmployee">
            <p class="mb-2"><strong>ID:</strong> {{ selectedEmployee.id }}</p>
            <p class="mb-2">
              <strong>氏名:</strong> {{ selectedEmployee.fullName }}
            </p>
            <p class="mb-2">
              <strong>メールアドレス:</strong>
              {{ selectedEmployee.user?.email }}
            </p>
            <p class="mb-2">
              <strong>生年月日:</strong>
              {{ formatDate(selectedEmployee.dateOfBirth) }}
            </p>
            <p class="mb-2">
              <strong>住所:</strong> {{ selectedEmployee.address }}
            </p>
            <p class="mb-2">
              <strong>性別:</strong> {{ selectedEmployee.gender }}
            </p>
            <p class="mb-2">
              <strong>入社日:</strong>
              {{ formatDate(selectedEmployee.joinDate) }}
            </p>
            <p class="mb-2">
              <strong>勤務先:</strong> {{ selectedEmployee.currentWorkplace }}
            </p>
            <p class="mb-2">
              <strong>権限:</strong> {{ selectedEmployee.user?.role }}
            </p>

            <!-- 使用可能スキル一覧 -->
            <div v-if="selectedEmployee.skills?.length" class="mt-4">
              <h3 class="text-lg font-semibold border-b pb-1 mb-3">
                使用可能スキル
              </h3>

              <!-- カテゴリ別グループ表示 -->
              <div
                v-for="(skills, category) in groupedSkills"
                :key="category"
                class="mb-3"
              >
                <h4 class="text-sm font-bold text-gray-600 mb-1">
                  {{ category }}
                </h4>

                <div class="flex-wrap gap-2">
                  <div
                    v-for="skill in skills"
                    :key="skill.id"
                    class="flex items-center bg-gray-100 px-2 py-1 rounded-full text-sm"
                  >
                    <img
                      v-if="skill.iconPath"
                      :src="`http://localhost:5211/${skill.iconPath}`"
                      alt="icon"
                      class="w-5 h-5 mr-1"
                    />
                    <span>{{ skill.name }}</span>
                  </div>
                </div>
              </div>
            </div>

            <div v-else class="mt-4 text-gray-500">
              スキル情報は登録されまていません
            </div>
          </div>

          <div class="flex justify-end mt-4 space-x-2">
            <button
              @click="isEditMode = true"
              class="bg-yellow-500 hover:bg-yellow-600 text-white px-4 py-2 rounded"
            >
              編集
            </button>
            <button
              @click="isDetailModalOpen = false"
              class="btn-outline px-4 py-2 rounded"
            >
              閉じる
            </button>
          </div>
        </div>
      </div>
    </transition>

    <!-- 従業員情報変更モーダル -->
    <transition name="scale-fade">
      <div
        v-if="isEditMode"
        class="fixed inset-0 bg-gradient-to-b from-black/40 to-black/60 flex items-center justify-center z-40"
        @click.self="isEditMode = false"
      >
        <div
          class="bg-white p-8 rounded-2xl shadow-2xl w-[500px] max-h-[90vh] overflow-y-auto transform transition-all duration-300"
        >
          <h2 class="text-2xl font-semibold mb-6 border-b pb-2 text-gray-800">
            従業員情報の編集
          </h2>

          <div class="space-y-4">
            <div>
              <label class="block text-sm text-gray-700 mb-1">氏名</label>
              <input
                v-model="selectedEmployee.fullName"
                class="w-full border rounded px-3 py-2"
              />
            </div>

            <div>
              <label class="block text-sm text-gray-700 mb-1"
                >メールアドレス</label
              >
              <input
                v-model="selectedEmployee.user.email"
                type="email"
                class="w-full border rounded px-3 py-2"
              />
              <p v-if="emailError" class="text-red-500 text-sm mb-3">
                {{ emailError }}
              </p>
            </div>

            <div>
              <label class="block text-sm text-gray-700 mb-1">住所</label>
              <input
                v-model="selectedEmployee.address"
                class="w-full border rounded px-3 py-2"
              />
            </div>

            <div>
              <label class="block text-sm text-gray-700 mb-1">勤務先</label>
              <input
                v-model="selectedEmployee.currentWorkplace"
                class="w-full border rounded px-3 py-2"
              />
            </div>

            <div class="mb-4">
              <h2 class="text-xl font-semibold mb-2">権限</h2>
              <div class="flex space-x-4">
                <label
                  ><input
                    v-model="selectedEmployee.user.role"
                    type="radio"
                    value="Admin"
                  />Admin</label
                >
                <label
                  ><input
                    v-model="selectedEmployee.user.role"
                    type="radio"
                    value="General"
                  />General</label
                >
              </div>
            </div>

            <div>
              <label class="block text-sm text-gray-700 mb-1">性別</label>
              <div class="flex space-x-4">
                <label
                  ><input
                    v-model="selectedEmployee.gender"
                    type="radio"
                    value="男性"
                  />
                  男性</label
                >
                <label
                  ><input
                    v-model="selectedEmployee.gender"
                    type="radio"
                    value="女性"
                  />
                  女性</label
                >
                <label
                  ><input
                    v-model="selectedEmployee.gender"
                    type="radio"
                    value="その他"
                  />
                  その他</label
                >
              </div>
            </div>
          </div>

          <!-- スキル編集セクション -->
          <div class="mt-6 border-t pt-4">
            <h3 class="text-lg font-semibold mb-2">使用可能スキル</h3>

            <!-- 既存スキル一覧 -->
            <div
              v-if="selectedEmployee.skills && selectedEmployee.skills.length"
            >
              <div
                v-for="(skillsByCategory, category) in groupedSkills"
                :key="category"
                class="mb-4"
              >
                <h4 class="text-sm font-bold text-gray-700 mb-1">
                  {{ category }}
                </h4>
                <div class="flex flex-wrap gap-2">
                  <div
                    v-for="skill in skillsByCategory"
                    :key="skill.name"
                    class="flex item-center bg-gray-100 border px-2 py-1 rounded-lg text-sm shadow-sm"
                  >
                    <img
                      v-if="skill.iconPath"
                      :src="`http://localhost:5211/${skill.iconPath}`"
                      :alt="skill.name"
                      class="w-4 h-4 mr-1"
                    />
                    <span>{{ skill.name }}</span>
                    <button
                      @click="removeSkill(skill.name)"
                      class="ml-2 text-red-500 hover:text-red-700"
                    >
                      <i class="fa-solid fa-xmark"></i>
                    </button>
                  </div>
                </div>
              </div>
            </div>

            <!-- スキル追加セクション -->
            <div class="mt-4">
              <label class="block text-sm font-medium text-gray-700 mb-1">
                スキル追加
              </label>
              <input
                v-model="skillSearch"
                @input="onInput"
                @onCompositionstart="isComposing = true"
                @onCompositionend="onCompositionEnd"
                placeholder="例: C#, Vue, MySQL..."
                class="w-full border rounded px-3 py-2 mb-2"
              />

              <!-- サジェスト一覧 -->
              <ul
                v-if="filteredSkills.length"
                class="bg-white border rounded shadow-md"
              >
                <li
                  v-for="skill in filteredSkills"
                  :key="skill.id"
                  @click="assignSkill(skill)"
                  class="px-3 py-2 hover:bg-blue-50 cursor-pointer flex items-center"
                >
                  <img
                    v-if="skill.iconPath"
                    :src="`http://localhost:5211/${skill.iconPath}`"
                    :alt="skill.name"
                    class="w-4 h-4 mr-2"
                  />
                  {{ skill.name }}
                </li>
              </ul>
            </div>
          </div>

          <div class="flex justify-between mt-8">
            <button
              @click="updateEmployee"
              class="btn-primary px-5 py-2 rounded-lg transition"
            >
              保存
            </button>
            <button
              @click="
                cancelSkillEdit();
                emailError = '';
                viewEmployee(selectedEmployee.id);
                isEditMode = false;
                isDetailModalOpen = false;
              "
              class="btn-outline px-5 py-2 rounded-lg transition"
            >
              キャンセル
            </button>
          </div>
        </div>
      </div>
    </transition>

    <!-- パスワード変更モーダル -->
    <transition name="scale-fade">
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
            <ErrorMessage
              name="newPassword"
              class="text-red-500 text-sm mb-2"
            />

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
              <button type="submit" class="btn-primary px-4 py-2 rounded">
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
    </transition>
  </div>
</template>

<script setup>
import { ref, onMounted, computed } from "vue";
import { useRouter } from "vue-router";
import { formatDate } from "@/utils/format";
import { Form, Field, ErrorMessage } from "vee-validate";
import { employeeAddSchema } from "@/validation/employeeSchemas";
import { validateEmailFormat } from "@/validation/employeeSchemas";
import { passwordSchema } from "@/validation/passwordSchemas";
import apiClient from "@/utils/apiClient";
import * as yup from "yup";
import {
  showConfirm,
  showSuccess,
  showWarning,
  showError,
} from "@/utils/alertService";
import qs from "qs";

// ----------------------
//  初期化・状態管理
// ----------------------
const router = useRouter();
const employees = ref([]);
const selectedEmployee = ref(null);
const isCreateModalOpen = ref(false);
const isDetailModalOpen = ref(false);
const isPasswordModalOpen = ref(false);
const isMenuOpen = ref(false);
const openRowMenuID = ref(null);
const isEditMode = ref(false);
const isSearchOpen = ref(false);
const emailError = ref("");
const allSkills = ref([]);
const skillSearch = ref("");
const filteredSkills = ref([]);
const isSkillModalOpen = ref(false);
const selectedSkills = ref([]);
const isComposing = ref(false);

// ----------------------
//  メニュー開閉制御
// ----------------------
const toggleMenu = () => (isMenuOpen.value = !isMenuOpen.value);
const toggleRowMenu = (id) => {
  openRowMenuID.value = openRowMenuID.value === id ? null : id;
};

//  モーダル外クリック時に閉じる（ハンバーガー＆3点リーダー対応）
const handleOutsideClick = () => {
  isMenuOpen.value = false;
  openRowMenuID.value = null;
};

// ----------------------
//  モーダル開閉
// ----------------------
const openCreateModal = () => {
  isCreateModalOpen.value = true;
  isMenuOpen.value = false;
};
const openChangePasswordModal = () => {
  isPasswordModalOpen.value = true;
  isMenuOpen.value = false;
};

// ----------------------
//  検索・ページング関連
// ----------------------
const searchParams = ref({
  name: "",
  workplace: "",
  joinDateFrom: "",
  joinDateTo: "",
  mode: "and",
  page: 1,
  pageSize: 10,
  sortBy: "id",
  sortOrder: "asc",
  skillIds: [],
});

const pagination = ref({
  totalCount: 0,
  totalPages: 1,
  currentPage: 1,
});

// 検索API
const searchEmployees = async () => {
  try {
    const response = await apiClient.get("/admin/search", {
      params: searchParams.value,

      paramsSerializer: (params) =>
      qs.stringify(params, { arrayFormat: "repeat" }),
    });

    employees.value = response.data.data;
    pagination.value.totalCount = response.data.totalCount;
    pagination.value.totalPages = response.data.totalPages;
    pagination.value.currentPage = response.data.currentPage;
  } catch (error) {
    console.error("検索エラー:", error);
  }
};

// ページ移動
const gotoPage = (page) => {
  if (page < 1 || page > pagination.value.totalPages) return;
  searchParams.value.page = page;
  searchEmployees();
};

// スキル選択制御
function openSkillModal() {
  isSkillModalOpen.value = true;
  skillSearch.value = "";
  filteredSkills.value = [];
}

// 選択したスキルを検索条件へ追加
function toggleSkill(skill) {
  const exists = selectedSkills.value.some(s=> s.id === skill.id);
  selectedSkills.value = exists
    ? selectedSkills.value.filter(s => s.id !== skill.id)
    : [...selectedSkills.value, skill];
}

// スキル選択を確定しスキル選択モーダルを閉じる
function confirmSkillSelection() {
  searchParams.value.skillIds = selectedSkills.value.map(s => s.id);
  isSkillModalOpen.value = false;
}

// スキル選択モーダルをキャンセルして閉じる
function cancelSelectedSkill(skill) {
  selectedSkills.value = selectedSkills.value.filter(s => s.id !== skill.id)
  searchParams.value.skillIds = selectedSkills.value.map(s => s.id);
}

// 条件リセット
const clearSearch = () => {
  searchParams.value = {
    name: "",
    workplace: "",
    joinDateFrom: "",
    joinDateTo: "",
    mode: "and",
    page: 1,
    pageSize: 10,
    sortBy: "id",
    sortOrder: "asc",
    skillIds: [],
  };
  selectedSkills.value = [];
  searchEmployees();
};

// ----------------------
//  CRUD操作群
// ----------------------
const newEmployee = {
  fullName: "",
  user: { email: "" },
  dateOfBirth: "",
  address: "",
  gender: "",
  joinDate: "",
  currentWorkplace: "",
};

const passwordData = {
  currentPassword: "",
  newPassword: "",
  confirmPassword: "",
};

// 作成
const createEmployee = async (formData) => {
  //request bodyを設定
  const dataToSend = {
    fullName: formData.fullName,
    dateOfBirth: formData.dateOfBirth,
    address: formData.address,
    gender: formData.gender,
    joinDate: formData.joinDate,
    currentWorkplace: formData.currentWorkplace,
    user: {
      email: formData.email,
    },
  };

  try {
    await apiClient.post("/admin/add-employee", dataToSend);
    showSuccess("従業員を追加しました");
    isCreateModalOpen.value = false;
    await clearSearch();
  } catch (error) {
    console.error("登録エラー", error);
    showError("登録に失敗しました");
  }
};

// 作成機能にバリデーションスキーマを適用
const handleCreateClick = async (values) => {
  await createEmployee(values);
};

// 従業員削除
const deleteEmployee = async (id) => {
  const confirmed = await showConfirm("この従業員を削除しますか?");
  if (!confirmed) return;

  try {
    await apiClient.delete(`/admin/${id}`);
    showSuccess("削除が完了しました");
    employees.value = employees.value.filter((e) => e.id !== id);
  } catch (error) {
    console.error("削除エラー:", error);
    showError("削除に失敗しました");
  }
};

// 詳細
const viewEmployee = async (id) => {
  try {
    const response = await apiClient.get(`/admin/${id}`);
    selectedEmployee.value = response.data;
    isDetailModalOpen.value = true;
    openRowMenuID.value = null;
  } catch (error) {
    console.error("詳細表示エラー:", error);
    showError("従業員情報取得に失敗しました");
  }
};

// 指定従業員情報更新処理
const updateEmployee = async () => {
  emailError.value = "";
  const checkResult = validateEmailFormat(selectedEmployee.value.user.email);

  if (checkResult) {
    emailError.value = checkResult;
    return;
  }

  try {
    await apiClient.put(
      `/admin/edit/${selectedEmployee.value.id}`,
      selectedEmployee.value
    );
    showSuccess("従業員情報を更新しました");
    isEditMode.value = false;
    isDetailModalOpen.value = false;
    await clearSearch();
  } catch (error) {
    console.error("更新エラー:", error);
    isEditMode.value = false;
    isDetailModalOpen.value = false;
    showError("更新に失敗しました");
  }
};

// スキルをカテゴリごとにグループ化
const groupedSkills = computed(() => {
  if (!selectedEmployee.value?.skills) return {};
  return selectedEmployee.value.skills.reduce((groups, skill) => {
    const category = skill.category || "未分類";
    if (!groups[category]) groups[category] = [];
    groups[category].push(skill);
    return groups;
  }, {});
});

// 初期化時にスキルマスタをロード
const loadSkills = async () => {
  try {
    const { data } = await apiClient.get("employeeskill/skills");
    allSkills.value = data;
  } catch (error) {
    showError("スキル一覧の取得に失敗しました");
    console.error("スキルマスタ取得失敗エラー:", error);
  }
};

// IME変換中はfilterSkillsを発火させない
const onInput = () => {
  if (isComposing.value) return;
  filterSkills();
};

// 変換確定時に検索実行
const onCompositionEnd = () => {
  isComposing.value = false;
  filterSkills();
};

// スキル検索フィルタ
const filterSkills = () => {
  const query = skillSearch.value.trim().toLowerCase();

  // 空欄時はサジェストを非表示に
  if(!query){
    filteredSkills.value = [];
    return;
  }

  // normalize()で全角・半角差を吸収
  const normalizeQuery = query.normalize("NFKC");

  filteredSkills.value = allSkills.value.filter((s) =>
    s.name.toLowerCase().normalize("NFKC").includes(normalizeQuery)
  );
};

  // スキル追加
  const assignSkill = async (skill) => {
  try {
    await apiClient.post(`/employeeskill/assign`, {
      employeeId: selectedEmployee.value.id,
      skillIds: [skill.id],
    }, {
      skipGlobalErrorHandler: true,
    });
    showSuccess("スキルを追加しました");
    await viewEmployee(selectedEmployee.value.id);
    skillSearch.value = "";
    filteredSkills.value = [];
  } catch (error) {
    if (error.response && error.response.status === 400) {
      showWarning("既に登録済のスキルです");
      return;
    } else {
      showError("スキル追加に失敗しました");
      console.error("スキル追加に失敗しました:", error)
    }
  }
};

function cancelSkillEdit() {
  skillSearch.value = "";
  filteredSkills.value = [];
}

// スキル削除
const removeSkill = async (skillName) => {
  try {
    const skill = allSkills.value.find((s) => s.name === skillName);
    if (!skill) return;

    await apiClient.delete(`/employeeskill/remove`, {
      data: {
        employeeId: selectedEmployee.value.id,
        skillId: skill.id,
      },
    });
    showSuccess("スキルを削除しました");
    await viewEmployee(selectedEmployee.value.id);
  } catch (error) {
    showError("スキル削除に失敗しました");
    console.error("スキル削除失敗:", error);
  }
};

// パスワード初期化
const confirmReset = async (id) => {
  const confirmed = await showConfirm(
    "パスワードを初期化してもよろしいですか？"
  );
  if (!confirmed) {
    handleOutsideClick();
    return;
  }

  try {
    await apiClient.put(`/admin/reset-password/${id}`, {});
    showSuccess("パスワードを初期化しました");
  } catch {
    showError("初期化に失敗しました");
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
    const response = await apiClient.put("/auth/change-password", dataToSend, {
      skipGlobalErrorHandler: true,
    });
    showSuccess(response.data.message);
    isPasswordModalOpen.value = false;
  } catch (error) {
    console.error("パスワード変更エラー:", error);
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

// パスワード変更機能にバリデーションスキーマを適用
const handleChangePassClick = async (values) => {
  await changePassword(values);
};

// ログアウト
const logout = () => {
  localStorage.removeItem("token");
  router.push("/login");
};

// 初期ロード時にスキルマスタ取得
onMounted(() => {
  loadSkills();
  searchEmployees();
});
</script>

<style scoped>
/* 詳細モーダルのフェード */
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.2s ease;
}
.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}

/* 右スライドアニメーション */
.slide-right-enter-active,
.slide-right-leave-active {
  transition: transform 0.3s ease, opacity 0.2s ease;
}
.slide-right-enter-from {
  transform: translateX(100%);
  opacity: 0;
}
.slide-right-enter-to {
  transform: translateX(0);
  opacity: 1;
}
.slide-right-leave-from {
  transform: translateX(0);
  opacity: 1;
}
.slide-right-leave-to {
  transform: translateX(100%);
  opacity: 0;
}

/* カードフェードイン */
@keyframes fadeIn {
  from {
    opacity: 0;
    transform: scale(0.97);
  }
  to {
    opacity: 1;
    transform: scale(1);
  }
}
.animate-fadeIn {
  animation: fadeIn 0.2s ease-out forwards;
}

/* 拡大＋フェードアニメーション */
.scale-fade-enter-active,
.scale-fade-leave-active {
  transition: all 0.3s ease;
}
.scale-fade-enter-from,
.scale-fade-leave-to {
  opacity: 0;
  transform: scale(0.9);
}
.scale-fade-enter-to,
.scale-fade-leave-from {
  opacity: 1;
  transform: scale(1);
}

/* スライド開閉アニメーション */
.slide-down-enter-active,
.slide-down-leave-active {
  transition: all 0.3s ease;
  overflow: hidden;
}
.slide-down-enter-from,
.slide-down-leave-to {
  opacity: 0;
  transform: translateY(-8px);
  max-height: 0;
}
.slide-down-enter-to,
.slide-down-leave-from {
  opacity: 1;
  transform: translateY(0);
  max-height: 1000px;
}
</style>
