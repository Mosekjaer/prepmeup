import { setAccessTokenProvider, setUnauthorizedHandler } from '@prepmeup/api-client';
import {
  exchangeCodeAsync,
  makeRedirectUri,
  refreshAsync,
  useAuthRequest,
  useAutoDiscovery,
  type TokenResponse,
} from 'expo-auth-session';
import * as SecureStore from 'expo-secure-store';
import * as WebBrowser from 'expo-web-browser';
import { createContext, useContext, useEffect, useRef, useState, type ReactNode } from 'react';

WebBrowser.maybeCompleteAuthSession();

const keycloakUrl = process.env.EXPO_PUBLIC_KEYCLOAK_URL ?? 'http://localhost:8082';
const issuer = `${keycloakUrl}/realms/prepmeup`;
const clientId = 'prepmeup-mobile';
const refreshTokenKey = 'refresh-token';
const renewMarginSeconds = 60;
const redirectUri = makeRedirectUri({ scheme: 'prepmeup', path: 'auth/callback' });

type Auth = {
  token?: string;
  signIn: () => void;
  signOut: () => void;
};

const AuthContext = createContext<Auth | undefined>(undefined);

function isInvalidGrant(error: unknown): boolean {
  return typeof error === 'object' && error !== null && 'code' in error && error.code === 'invalid_grant';
}

function startSession(result: TokenResponse, setTokens: (tokens: TokenResponse) => void) {
  setAccessTokenProvider(() => result.accessToken);
  setTokens(result);
  if (result.refreshToken) {
    SecureStore.setItemAsync(refreshTokenKey, result.refreshToken);
  }
}

function clearSession(setTokens: (tokens: TokenResponse | undefined) => void) {
  setAccessTokenProvider(() => undefined);
  setTokens(undefined);
  return SecureStore.deleteItemAsync(refreshTokenKey);
}

function endRejectedSession(error: unknown, setTokens: (tokens: TokenResponse | undefined) => void) {
  if (isInvalidGrant(error)) {
    return clearSession(setTokens);
  }
}

export function AuthProvider({ children }: { children: ReactNode }) {
  const discovery = useAutoDiscovery(issuer);
  const [request, response, promptAsync] = useAuthRequest({ clientId, redirectUri, scopes: ['openid'] }, discovery);
  const [tokens, setTokens] = useState<TokenResponse>();
  const token = tokens?.accessToken;
  const tokensRef = useRef(tokens);
  tokensRef.current = tokens;

  useEffect(() => {
    if (!discovery) {
      return;
    }

    SecureStore.getItemAsync(refreshTokenKey).then((stored) => {
      if (!stored) {
        return;
      }

      refreshAsync({ clientId, refreshToken: stored }, discovery)
        .then((result) => startSession(result, setTokens))
        .catch((error: unknown) => endRejectedSession(error, setTokens));
    });
  }, [discovery]);

  useEffect(() => {
    if (!discovery || !tokens?.refreshToken || !tokens.expiresIn) {
      return;
    }

    const refreshToken = tokens.refreshToken;
    const timer = setTimeout(() => {
      refreshAsync({ clientId, refreshToken }, discovery)
        .then((result) => startSession(result, setTokens))
        .catch((error: unknown) => endRejectedSession(error, setTokens));
    }, (tokens.expiresIn - renewMarginSeconds) * 1000);

    return () => clearTimeout(timer);
  }, [tokens, discovery]);

  useEffect(() => {
    if (!discovery) {
      return;
    }

    setUnauthorizedHandler({
      renew: async () => {
        const refreshToken = tokensRef.current?.refreshToken;
        if (!refreshToken) {
          return undefined;
        }

        try {
          const result = await refreshAsync({ clientId, refreshToken }, discovery);
          startSession(result, setTokens);
          return result.accessToken;
        } catch {
          return undefined;
        }
      },
      expire: () => clearSession(setTokens),
    });

    return () => setUnauthorizedHandler(undefined);
  }, [discovery]);

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
    ).then((result) => startSession(result, setTokens));
  }, [response, request, discovery]);

  async function signOut() {
    const idToken = tokens?.idToken;
    setAccessTokenProvider(() => undefined);
    setTokens(undefined);
    await SecureStore.deleteItemAsync(refreshTokenKey).catch(() => undefined);

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
