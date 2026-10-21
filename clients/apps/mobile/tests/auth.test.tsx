import { apiConfig } from '@prepmeup/api-client';
import { fireEvent, render, screen, waitFor } from '@testing-library/react';
import { useEffect } from 'react';
import { beforeEach, expect, it, vi } from 'vitest';

import { AuthProvider, useAuth } from '../src/lib/auth';

const mocks = vi.hoisted(() => ({
  request: { codeVerifier: 'verifier' },
  discovery: { tokenEndpoint: 'http://keycloak/token', endSessionEndpoint: 'http://keycloak/logout' },
  response: null as { type: string; params: Record<string, string> } | null,
  exchangeCodeAsync: vi.fn(),
  openAuthSessionAsync: vi.fn(),
}));

vi.mock('expo-auth-session', () => ({
  makeRedirectUri: () => 'prepmeup://auth/callback',
  useAutoDiscovery: () => mocks.discovery,
  useAuthRequest: () => [mocks.request, mocks.response, vi.fn()],
  exchangeCodeAsync: mocks.exchangeCodeAsync,
}));

vi.mock('expo-web-browser', () => ({
  maybeCompleteAuthSession: vi.fn(),
  openAuthSessionAsync: mocks.openAuthSessionAsync,
}));

function Probe() {
  const { token, signOut } = useAuth();
  return <button onClick={signOut}>{token ?? 'signed out'}</button>;
}

async function authorizationHeaderOfNextApiCall() {
  const fetch = vi.fn().mockResolvedValue(new Response());
  vi.stubGlobal('fetch', fetch);
  await apiConfig.fetch('http://api/items');
  return new Headers(fetch.mock.calls[0]?.[1]?.headers).get('Authorization');
}

beforeEach(() => {
  mocks.response = null;
  mocks.exchangeCodeAsync.mockResolvedValue({ accessToken: 'access-token', idToken: 'id-token' });
  mocks.openAuthSessionAsync.mockResolvedValue({ type: 'success' });
});

it('sends no token before the user signs in', async () => {
  render(
    <AuthProvider>
      <Probe />
    </AuthProvider>,
  );

  expect(screen.getByText('signed out')).toBeTruthy();
  expect(await authorizationHeaderOfNextApiCall()).toBeNull();
});

it('exchanges the code with the PKCE verifier and sends the access token', async () => {
  mocks.response = { type: 'success', params: { code: 'code-from-keycloak' } };

  render(
    <AuthProvider>
      <Probe />
    </AuthProvider>,
  );

  expect(await screen.findByText('access-token')).toBeTruthy();
  expect(mocks.exchangeCodeAsync).toHaveBeenCalledWith(
    expect.objectContaining({ code: 'code-from-keycloak', extraParams: { code_verifier: 'verifier' } }),
    expect.anything(),
  );
  expect(await authorizationHeaderOfNextApiCall()).toBe('Bearer access-token');
});

it('hands the token to api-client before child effects call the API', async () => {
  mocks.response = { type: 'success', params: { code: 'code-from-keycloak' } };
  const headersSeenByChild: (string | null)[] = [];

  function CallsApiWhenSignedIn() {
    const { token } = useAuth();
    useEffect(() => {
      if (token) {
        authorizationHeaderOfNextApiCall().then((header) => headersSeenByChild.push(header));
      }
    }, [token]);
    return null;
  }

  render(
    <AuthProvider>
      <CallsApiWhenSignedIn />
    </AuthProvider>,
  );

  await waitFor(() => expect(headersSeenByChild).toEqual(['Bearer access-token']));
});

it('ends the Keycloak session on sign out and stops sending the token', async () => {
  mocks.response = { type: 'success', params: { code: 'code-from-keycloak' } };
  render(
    <AuthProvider>
      <Probe />
    </AuthProvider>,
  );
  fireEvent.click(await screen.findByText('access-token'));

  await waitFor(() => expect(mocks.openAuthSessionAsync).toHaveBeenCalled());
  const logoutUrl = new URL(mocks.openAuthSessionAsync.mock.calls[0]?.[0]);
  expect(logoutUrl.origin + logoutUrl.pathname).toBe('http://keycloak/logout');
  expect(logoutUrl.searchParams.get('id_token_hint')).toBe('id-token');
  await screen.findByText('signed out');
  expect(await authorizationHeaderOfNextApiCall()).toBeNull();
});
