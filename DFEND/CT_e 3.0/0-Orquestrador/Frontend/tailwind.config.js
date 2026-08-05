/** @type {import('tailwindcss').Config} */
/**
 * Orquestrador — azul-marinho escuro (mais escuro da cadeia).
 * indigo/violet remapeados para navy/blue para classes existentes.
 */
module.exports = {
  content: ['./apps/**/*.{html,ts}', './libs/**/*.{html,ts}'],
  theme: {
    extend: {
      colors: {
        slate: {
          925: '#070f1c',
        },
        indigo: {
          50: '#eef5ff',
          100: '#d9e9ff',
          200: '#b8d4ff',
          300: '#85b4ff',
          400: '#4d8ef7',
          500: '#2563eb',
          600: '#1d4ed8',
          700: '#1e3a8a',
          800: '#152a5c',
          900: '#0c1e3d',
          950: '#060e1c',
        },
        violet: {
          50: '#eff6ff',
          100: '#dbeafe',
          200: '#bfdbfe',
          300: '#93c5fd',
          400: '#60a5fa',
          500: '#3b82f6',
          600: '#2563eb',
          700: '#1d4ed8',
          800: '#1e40af',
          900: '#1e3a8a',
          950: '#0b1a3a',
        },
      },
    },
  },
  plugins: [],
};
