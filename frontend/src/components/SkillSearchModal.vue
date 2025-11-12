<template>
    <div
        v-if="isOpen"
        class="fixed inset-0 bg-black bg-opacity-40 flex items-center justify-center z-50"
        @click.self="closeModal"
    >
        <div class="bg-white w-full max-w-2xl rounded-2xl shadow-lg p-6 relative animate-fade-in-up">
            <h2 class="text-xl font-bold text-gray-800 mb-4">高度検索</h2>

            < !-- 氏名 -- >
        </div>
    </div>
</template>