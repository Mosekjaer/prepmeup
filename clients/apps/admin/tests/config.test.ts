import { afterEach, beforeEach, expect, it, vi } from 'vitest';

beforeEach(() => {
  vi.resetModules();
});

afterEach(() => {
  vi.unstubAllEnvs();
  delete window.__CONFIG__;
});

it('uses the runtime config when it is set', async () => {
  window.__CONFIG__ = { apiUrl: 'https://api.test', keycloakUrl: 'https://auth.test' };
  vi.stubEnv('VITE_API_URL', 'http://env-api');
  vi.stubEnv('VITE_KEYCLOAK_URL', 'http://env-auth');

  const { apiUrl, keycloakUrl } = await import('../src/lib/config');

  expect(apiUrl).toBe('https://api.test');
  expect(keycloakUrl).toBe('https://auth.test');
});

it('falls back to the VITE variables without runtime config', async () => {
  vi.stubEnv('VITE_API_URL', 'http://env-api');
  vi.stubEnv('VITE_KEYCLOAK_URL', 'http://env-auth');

  const { apiUrl, keycloakUrl } = await import('../src/lib/config');

  expect(apiUrl).toBe('http://env-api');
  expect(keycloakUrl).toBe('http://env-auth');
});

it('falls back to the local addresses when nothing is set', async () => {
  vi.stubEnv('VITE_API_URL', undefined);
  vi.stubEnv('VITE_KEYCLOAK_URL', undefined);

  const { apiUrl, keycloakUrl } = await import('../src/lib/config');

  expect(apiUrl).toBe('http://localhost:5001');
  expect(keycloakUrl).toBe('http://localhost:8082');
});
