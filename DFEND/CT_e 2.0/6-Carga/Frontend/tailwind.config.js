/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    './apps/**/*.{html,ts}',
    './libs/**/*.{html,ts}',
  ],
  theme: {
    extend: {
      colors: {
        slate: {
          925: '#0b1220',
        },
      },
    },
  },
  plugins: [],
};
