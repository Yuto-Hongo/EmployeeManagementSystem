// tailwind.config.js
/** @type {import('tailwindcss').Config} */
export default {
  content: [
    "./index.html",
    "./src/**/*.{vue,js,ts,jsx,tsx}",
  ],
  theme: {
    extend: {
      colors: {
        primary: {
          DEFAULT: "#2563eb", // 通常の青 (blue-600)
          light: "#3b82f6",   // hoverなどに使う
          dark: "#1e40af",    // activeや濃いトーン
        },
        secondary: {
          DEFAULT: "#10b981", // エメラルド系
          light: "#34d399",
          dark: "#047857",
        },
        danger: {
          DEFAULT: "#ef4444",
          light: "#f87171",
          dark: "#b91c1c",
        },
        neutral: {
          light: "#f3f4f6",
          DEFAULT: "#6b7280",
          dark: "#374151",
        },
      },
      fontFamily: {
        sans: ["'Noto Sans JP'", "Inter", "system-ui", "sans-serif"],
      },
      boxShadow: {
        soft: "0 4px 12px rgba(0, 0, 0, 0.05)",
        medium: "0 6px 18px rgba(0, 0, 0, 0.1)",
      },
    },
  },
  plugins: [],
}
