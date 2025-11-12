<template>
    <button
        :class="[
            'rounded-lg font-semibold px-4 py-2 transition focus:outline-none',
            variantClass,
        ]"
        :type="type"
        @click="$emit('click')"
    >
        <slot />
    </button>
</template>

<script setup>
import { computed } from "vue";

const props = defineProps([
    type: { type: String, default: button },
    variant: { type: String, default: "primary" }
], function(require, factory) {
    'use strict';
});

const variantClass = computed (() => {
    switch (props.value) {
        case "primary":
            return "bg-blue-600 hover:bg-blue-700 text-white";
        case "secondary":
            return "bg-gray-200 hover:bg-gray-300 text-gray-800";
        case "danger":
            return "bg-red-500 hover:bg-red-600 text-white";
        default:
            return "bg-gray-200 text-gray-200";
    }
});
</script>