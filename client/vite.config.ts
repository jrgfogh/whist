import { storybookTest } from '@storybook/addon-vitest/vitest-plugin';
import react from '@vitejs/plugin-react';
import { defineConfig } from 'vite';

export default defineConfig(({ command }) => {
  const aspNetCoreHttpsPort = process.env.ASPNETCORE_HTTPS_PORT;
  const target = aspNetCoreHttpsPort
    ? `https://localhost:${aspNetCoreHttpsPort}`
    : 'https://localhost:5001';

  return {
    plugins: [
      react(),
    ],
    base: command === 'build' ? '/whist/' : '/',
    build: {
      outDir: 'dist',
    },
    optimizeDeps: {},
    server: {
      // HTTP is intentional: this app uses no cookie-based auth, so there is
      // no need for a dev HTTPS certificate (unlike the removed aspnetcore-react.js).
      port: 5173,
      proxy: {
        '/WhistHub': {
          target,
          secure: false,
          changeOrigin: true,
          ws: true,
        },
      },
    },
    test: {
      environment: 'jsdom',
      globals: true,
      coverage: {
        provider: 'v8',
        reporter: ['lcov', 'text-summary'],
        thresholds: {
          lines: 20,
          functions: 20,
          branches: 20,
          statements: 20,
        },
      },
      projects: [
        {
          extends: true,
          plugins: [storybookTest({ configDir: '.storybook' })],
          test: {
            name: 'storybook',
            browser: {
              enabled: true,
              headless: true,
              provider: 'playwright',
              instances: [{ browser: 'chromium' }],
            },
          },
        },
      ],
    },
  };
});
