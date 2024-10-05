import { fileURLToPath, URL } from 'node:url'

import { defineConfig, loadEnv  } from 'vite'
import vue from '@vitejs/plugin-vue'

// https://vitejs.dev/config/
export default defineConfig(({ mode }) => {
  const env = loadEnv(mode, process.cwd(), '');
  return {
    define: {
      'process.env.OTEL_EXPORTER_OTLP_ENDPOINT': JSON.stringify(env.OTEL_EXPORTER_OTLP_ENDPOINT),
      'process.env.OTEL_EXPORTER_OTLP_HEADERS': JSON.stringify(env.OTEL_EXPORTER_OTLP_HEADERS),
      'process.env.OTEL_RESOURCE_ATTRIBUTES': JSON.stringify(env.OTEL_RESOURCE_ATTRIBUTES),
      'process.env.OTEL_SERVICE_NAME': JSON.stringify(env.OTEL_SERVICE_NAME)
    },
    
  plugins: [
    vue(),
  ],
  resolve: {
    alias: {
      '@': fileURLToPath(new URL('./src', import.meta.url))
    }
  },
  server: {
    host: true,
    port: parseInt(process.env.PORT ?? "5173"),
    proxy: {
      '/api': {
        target: process.env.services__backofficeapi__https__0 || process.env.services__backofficeapi__http__0,
        changeOrigin: true,
        rewrite: path => path.replace(/^\/api/, ''),
        secure: false
      }
    }
  }
  }
})
