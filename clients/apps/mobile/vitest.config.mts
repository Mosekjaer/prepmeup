import { fileURLToPath } from 'node:url';
import { defineConfig } from 'vitest/config';

const workspaceRoot = (path: string) => fileURLToPath(new URL(`../../node_modules/${path}`, import.meta.url));

// React Native cannot run under Vitest, so components are tested against react-native-web.
// Keep tests on component logic and state, not on rendering fidelity.
export default defineConfig({
  resolve: {
    alias: {
      'react-native': 'react-native-web',
      react: workspaceRoot('react'),
      'react-dom': workspaceRoot('react-dom'),
      '@': new URL('./src/', import.meta.url).pathname,
    },
  },
  test: {
    environment: 'jsdom',
    globals: true,
    include: ['tests/**/*.test.{ts,tsx}'],
  },
});
