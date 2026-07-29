import { defineConfig, transformWithEsbuild } from 'vite'
import react from '@vitejs/plugin-react'

export default defineConfig(({ command }) => {
  const aspNetCoreHttpsPort = process.env.ASPNETCORE_HTTPS_PORT;
  const target = aspNetCoreHttpsPort
    ? `https://localhost:${aspNetCoreHttpsPort}`
    : 'https://localhost:5001';

  return {
    plugins: [
      // Allow JSX in .js files (project predates the .jsx convention)
      {
        name: 'treat-js-files-as-jsx',
        async transform(code, id) {
          if (!id.match(/src\/.*\.js$/)) return null;
          return transformWithEsbuild(code, id, { loader: 'jsx', jsx: 'automatic' });
        },
      },
      react({ include: /\.(js|jsx|ts|tsx)$/ }),
    ],
    base: command === 'build' ? '/whist/' : '/',
    build: {
      outDir: 'dist',
    },
    optimizeDeps: {
      esbuildOptions: {
        loader: { '.js': 'jsx' },
      },
    },
    server: {
      // HTTP is intentional: this app uses no cookie-based auth, so there is
      // no need for a dev HTTPS certificate (unlike the removed aspnetcore-react.js).
      port: 5173,
      proxy: {
        '/WhistHub': {
          target,
          secure: false,
          ws: true,
        },
      },
    },
    test: {
      environment: 'jsdom',
      globals: true,
    },
  }
})
