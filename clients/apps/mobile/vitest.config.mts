import { defineConfig } from 'vitest/config';

// React Native cannot run under Vitest, so components are tested against react-native-web.
// Keep tests on component logic and state, not on rendering fidelity.
export default defineConfig({
  resolve: {
    alias: {
      'react-native': 'react-native-web',
      '@': new URL('./src/', import.meta.url).pathname,
    },
  },
  test: {
    environment: 'jsdom',
    globals: true,
    include: ['src/**/*.test.{ts,tsx}'],
  },
});
