export const apiUrl =
  window.__CONFIG__?.apiUrl ?? import.meta.env.VITE_API_URL ?? 'http://localhost:5001';

export const keycloakUrl =
  window.__CONFIG__?.keycloakUrl ?? import.meta.env.VITE_KEYCLOAK_URL ?? 'http://localhost:8082';
