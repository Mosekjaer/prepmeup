import { setAccessTokenProvider } from '@prepmeup/api-client';
import {
  exchangeCodeAsync,
  makeRedirectUri,
  useAuthRequest,
  useAutoDiscovery,
  type TokenResponse,
} from 'expo-auth-session';
import * as WebBrowser from 'expo-web-browser';
import { createContext, useContext, useEffect, useState, type ReactNode } from 'react';

WebBrowser.maybeCompleteAuthSession();

const keycloakUrl = process.env.EXPO_PUBLIC_KEYCLOAK_URL ?? 'http://localhost:8082';
const issuer = `${keycloakUrl}/realms/prepmeup`;
const clientId = 'prepmeup-mobile';
const redirectUri = makeRedirectUri({ scheme: 'prepmeup', path: 'auth/callback' });

type Auth = {
  token?: string;
  signIn: () => void;
  signOut: () => void;
};

const AuthContext = createContext<Auth | undefined>(undefined);

export function AuthProvider({ children }: { children: ReactNode }) {
  const discovery = useAutoDiscovery(issuer);
  const [request, response, promptAsync] = useAuthRequest({ clientId, redirectUri, scopes: ['openid'] }, discovery);
  const [tokens, setTokens] = useState<TokenResponse>();
  const token = tokens?.accessToken;

  useEffect(() => {
    if (response?.type !== 'success' || !request?.codeVerifier || !discovery) {
      return;
    }

    const code = response.params.code;
    if (!code) {
      return;
    }

    exchangeCodeAsync(
      {
        clientId,
        redirectUri,
        code,
        extraParams: { code_verifier: request.codeVerifier },
      },
      discovery,
    ).then((result) => {
      setAccessTokenProvider(() => result.accessToken);
      setTokens(result);
    });
  }, [response, request, discovery]);

  async function signOut() {
    const idToken = tokens?.idToken;
    setAccessTokenProvider(() => undefined);
    setTokens(undefined);

    if (!discovery?.endSessionEndpoint || !idToken) {
      return;
    }

    const params = new URLSearchParams({ id_token_hint: idToken, post_logout_redirect_uri: redirectUri });
    await WebBrowser.openAuthSessionAsync(`${discovery.endSessionEndpoint}?${params}`, redirectUri);
  }

  const value: Auth = {
    token,
    signIn: () => promptAsync(),
    signOut,
  };

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}

export function useAuth(): Auth {
  const auth = useContext(AuthContext);
  if (!auth) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return auth;
}
