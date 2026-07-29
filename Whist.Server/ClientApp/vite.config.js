import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

export default defineConfig(({ command }) => {
  const aspNetCoreHttpsPort = process.env.ASPNETCORE_HTTPS_PORT;
  const target = aspNetCoreHttpsPort
    ? `https://localhost:${aspNetCoreHttpsPort}`
    : 'https://localhost:5001';

  return {
    plugins: [react()],
    base: command === 'build' ? '/whist/' : '/',
    build: {
      outDir: 'build',
    },
    server: {
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
