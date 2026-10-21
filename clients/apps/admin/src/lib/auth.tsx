import { InMemoryWebStorage, WebStorageStateStore } from 'oidc-client-ts';
import type { ReactNode } from 'react';
import { AuthProvider as OidcProvider, type AuthProviderProps } from 'react-oidc-context';

const keycloakUrl = import.meta.env.VITE_KEYCLOAK_URL ?? 'http://localhost:8082';

const oidcConfig: AuthProviderProps = {
  authority: `${keycloakUrl}/realms/prepmeup`,
  client_id: 'prepmeup-admin',
  redirect_uri: `${window.location.origin}/`,
  post_logout_redirect_uri: `${window.location.origin}/`,
  scope: 'openid',
  automaticSilentRenew: true,
  userStore: new WebStorageStateStore({ store: new InMemoryWebStorage() }),
  onSigninCallback: () => window.history.replaceState({}, document.title, window.location.pathname),
};

export function AuthProvider({ children }: { children: ReactNode }) {
  return <OidcProvider {...oidcConfig}>{children}</OidcProvider>;
}
