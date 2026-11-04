import { apiConfig, setAccessTokenProvider } from '@prepmeup/api-client';
import { act, fireEvent, render, screen, waitFor } from '@testing-library/react';
import { useEffect } from 'react';
import { afterEach, beforeEach, expect, it, vi } from 'vitest';

import { AuthProvider, useAuth } from '../src/lib/auth';

const mocks = vi.hoisted(() => ({
  request: { codeVerifier: 'verifier' },
  discovery: { tokenEndpoint: 'http://keycloak/token', endSessionEndpoint: 'http://keycloak/logout' },
  response: null as { type: string; params: Record<string, string> } | null,
  exchangeCodeAsync: vi.fn(),
  refreshAsync: vi.fn(),
  openAuthSessionAsync: vi.fn(),
  getItemAsync: vi.fn(),
  setItemAsync: vi.fn(),
  deleteItemAsync: vi.fn(),
}));

vi.mock('expo-auth-session', () => ({
  makeRedirectUri: () => 'prepmeup://auth/callback',
  useAutoDiscovery: () => mocks.discovery,
  useAuthRequest: () => [mocks.request, mocks.response, vi.fn()],
  exchangeCodeAsync: mocks.exchangeCodeAsync,
  refreshAsync: mocks.refreshAsync,
}));

vi.mock('expo-secure-store', () => ({
  getItemAsync: mocks.getItemAsync,
  setItemAsync: mocks.setItemAsync,
  deleteItemAsync: mocks.deleteItemAsync,
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

afterEach(() => {
  vi.useRealTimers();
});

beforeEach(() => {
  setAccessTokenProvider(() => undefined);
  mocks.response = null;
  mocks.exchangeCodeAsync.mockResolvedValue({ accessToken: 'access-token', idToken: 'id-token' });
  mocks.openAuthSessionAsync.mockResolvedValue({ type: 'success' });
  mocks.getItemAsync.mockResolvedValue(null);
  mocks.setItemAsync.mockResolvedValue(undefined);
  mocks.deleteItemAsync.mockResolvedValue(undefined);
});

it('stays signed out and does not refresh when no refresh token is stored', async () => {
  render(
    <AuthProvider>
      <Probe />
    </AuthProvider>,
  );

  await waitFor(() => expect(mocks.getItemAsync).toHaveBeenCalledWith('refresh-token'));
  expect(screen.getByText('signed out')).toBeTruthy();
  expect(mocks.refreshAsync).not.toHaveBeenCalled();
});

it('signs in again with the stored refresh token after a restart', async () => {
  mocks.getItemAsync.mockResolvedValue('r1');
  mocks.refreshAsync.mockResolvedValue({ accessToken: 'a2', refreshToken: 'r2', expiresIn: 900 });

  render(
    <AuthProvider>
      <Probe />
    </AuthProvider>,
  );

  expect(await screen.findByText('a2')).toBeTruthy();
  expect(mocks.refreshAsync).toHaveBeenCalledWith(
    expect.objectContaining({ refreshToken: 'r1' }),
    expect.anything(),
  );
  expect(await authorizationHeaderOfNextApiCall()).toBe('Bearer a2');
});

it('signs out and deletes the stored token when Keycloak rejects it', async () => {
  mocks.getItemAsync.mockResolvedValue('r1');
  mocks.refreshAsync.mockRejectedValue(Object.assign(new Error('invalid_grant'), { code: 'invalid_grant' }));

  render(
    <AuthProvider>
      <Probe />
    </AuthProvider>,
  );

  await waitFor(() => expect(mocks.deleteItemAsync).toHaveBeenCalledWith('refresh-token'));
  expect(screen.getByText('signed out')).toBeTruthy();
});

it('keeps the stored token when the refresh fails because the network is down', async () => {
  mocks.getItemAsync.mockResolvedValue('r1');
  mocks.refreshAsync.mockRejectedValue(new TypeError('Network request failed'));

  render(
    <AuthProvider>
      <Probe />
    </AuthProvider>,
  );

  await waitFor(() => expect(mocks.refreshAsync).toHaveBeenCalled());
  await new Promise((resolve) => setTimeout(resolve, 0));
  expect(screen.getByText('signed out')).toBeTruthy();
  expect(mocks.deleteItemAsync).not.toHaveBeenCalled();
});

it('stores the refresh token from the code exchange', async () => {
  mocks.response = { type: 'success', params: { code: 'code-from-keycloak' } };
  mocks.exchangeCodeAsync.mockResolvedValue({ accessToken: 'a1', idToken: 'id-token', refreshToken: 'r1' });

  render(
    <AuthProvider>
      <Probe />
    </AuthProvider>,
  );

  await screen.findByText('a1');
  expect(mocks.setItemAsync).toHaveBeenCalledWith('refresh-token', 'r1');
});

it('signs in without storing anything when the exchange returns no refresh token', async () => {
  mocks.response = { type: 'success', params: { code: 'code-from-keycloak' } };

  render(
    <AuthProvider>
      <Probe />
    </AuthProvider>,
  );

  await screen.findByText('access-token');
  expect(mocks.setItemAsync).not.toHaveBeenCalled();
});

async function advanceSeconds(seconds: number) {
  await act(async () => {
    await vi.advanceTimersByTimeAsync(seconds * 1000);
  });
}

it('renews the access token 60 seconds before it expires', async () => {
  vi.useFakeTimers();
  mocks.getItemAsync.mockResolvedValue('r1');
  mocks.refreshAsync
    .mockResolvedValueOnce({ accessToken: 'a2', refreshToken: 'r2', expiresIn: 900 })
    .mockResolvedValueOnce({ accessToken: 'a3', refreshToken: 'r3', expiresIn: 900 });

  render(
    <AuthProvider>
      <Probe />
    </AuthProvider>,
  );
  await advanceSeconds(0);
  expect(screen.getByText('a2')).toBeTruthy();
  await advanceSeconds(840);

  expect(mocks.refreshAsync).toHaveBeenCalledTimes(2);
  expect(mocks.refreshAsync).toHaveBeenLastCalledWith(
    expect.objectContaining({ refreshToken: 'r2' }),
    expect.anything(),
  );
});

it('does not renew the access token one second before the margin', async () => {
  vi.useFakeTimers();
  mocks.getItemAsync.mockResolvedValue('r1');
  mocks.refreshAsync.mockResolvedValue({ accessToken: 'a2', refreshToken: 'r2', expiresIn: 900 });

  render(
    <AuthProvider>
      <Probe />
    </AuthProvider>,
  );
  await advanceSeconds(0);
  await advanceSeconds(839);

  expect(mocks.refreshAsync).toHaveBeenCalledTimes(1);
});

it('renews the access token only once one second after the margin', async () => {
  vi.useFakeTimers();
  mocks.getItemAsync.mockResolvedValue('r1');
  mocks.refreshAsync.mockResolvedValue({ accessToken: 'a2', refreshToken: 'r2', expiresIn: 900 });

  render(
    <AuthProvider>
      <Probe />
    </AuthProvider>,
  );
  await advanceSeconds(0);
  await advanceSeconds(841);

  expect(mocks.refreshAsync).toHaveBeenCalledTimes(2);
});

it('stores the rotated refresh token after a renewal', async () => {
  vi.useFakeTimers();
  mocks.getItemAsync.mockResolvedValue('r1');
  mocks.refreshAsync
    .mockResolvedValueOnce({ accessToken: 'a2', refreshToken: 'r2', expiresIn: 900 })
    .mockResolvedValueOnce({ accessToken: 'a3', refreshToken: 'r3', expiresIn: 900 });

  render(
    <AuthProvider>
      <Probe />
    </AuthProvider>,
  );
  await advanceSeconds(0);
  await advanceSeconds(840);

  expect(screen.getByText('a3')).toBeTruthy();
  expect(mocks.setItemAsync).toHaveBeenLastCalledWith('refresh-token', 'r3');
});

it('stores the rotated refresh token after the refresh on startup', async () => {
  mocks.getItemAsync.mockResolvedValue('r1');
  mocks.refreshAsync.mockResolvedValue({ accessToken: 'a2', refreshToken: 'r2', expiresIn: 900 });

  render(
    <AuthProvider>
      <Probe />
    </AuthProvider>,
  );

  await screen.findByText('a2');
  expect(mocks.setItemAsync).toHaveBeenCalledWith('refresh-token', 'r2');
});

it('signs out and deletes the stored token when a renewal is rejected', async () => {
  vi.useFakeTimers();
  mocks.getItemAsync.mockResolvedValue('r1');
  mocks.refreshAsync
    .mockResolvedValueOnce({ accessToken: 'a2', refreshToken: 'r2', expiresIn: 900 })
    .mockRejectedValueOnce(Object.assign(new Error('invalid_grant'), { code: 'invalid_grant' }));

  render(
    <AuthProvider>
      <Probe />
    </AuthProvider>,
  );
  await advanceSeconds(0);
  await advanceSeconds(840);

  expect(screen.getByText('signed out')).toBeTruthy();
  expect(mocks.deleteItemAsync).toHaveBeenCalledWith('refresh-token');
  expect(await authorizationHeaderOfNextApiCall()).toBeNull();
});

it('deletes the stored refresh token on sign out', async () => {
  mocks.response = { type: 'success', params: { code: 'code-from-keycloak' } };
  mocks.exchangeCodeAsync.mockResolvedValue({ accessToken: 'a1', idToken: 'id-token', refreshToken: 'r1' });
  render(
    <AuthProvider>
      <Probe />
    </AuthProvider>,
  );
  fireEvent.click(await screen.findByText('a1'));

  await waitFor(() => expect(mocks.deleteItemAsync).toHaveBeenCalledWith('refresh-token'));
  await screen.findByText('signed out');
});

it('still signs out and ends the Keycloak session when the store cannot delete the token', async () => {
  mocks.response = { type: 'success', params: { code: 'code-from-keycloak' } };
  mocks.deleteItemAsync.mockRejectedValue(new Error('keystore unavailable'));
  render(
    <AuthProvider>
      <Probe />
    </AuthProvider>,
  );
  fireEvent.click(await screen.findByText('access-token'));

  await screen.findByText('signed out');
  await waitFor(() => expect(mocks.openAuthSessionAsync).toHaveBeenCalled());
});

it('renews the session with the refresh token when the API replies 401', async () => {
  mocks.response = { type: 'success', params: { code: 'code-from-keycloak' } };
  mocks.exchangeCodeAsync.mockResolvedValue({ accessToken: 'a1', idToken: 'id-token', refreshToken: 'r1' });
  mocks.refreshAsync.mockResolvedValue({ accessToken: 'a4', refreshToken: 'r4' });
  render(
    <AuthProvider>
      <Probe />
    </AuthProvider>,
  );
  await screen.findByText('a1');
  const fetch = vi
    .fn()
    .mockResolvedValueOnce(new Response(null, { status: 401 }))
    .mockResolvedValueOnce(new Response(null, { status: 200 }));
  vi.stubGlobal('fetch', fetch);

  await apiConfig.fetch('http://api/items');

  expect(mocks.refreshAsync).toHaveBeenCalledWith(
    expect.objectContaining({ refreshToken: 'r1' }),
    expect.anything(),
  );
  expect(new Headers(fetch.mock.calls[1]?.[1]?.headers).get('Authorization')).toBe('Bearer a4');
  expect(await screen.findByText('a4')).toBeTruthy();
});

it('signs out and deletes the stored token when the API still replies 401 after a renewal', async () => {
  mocks.response = { type: 'success', params: { code: 'code-from-keycloak' } };
  mocks.exchangeCodeAsync.mockResolvedValue({ accessToken: 'a1', idToken: 'id-token', refreshToken: 'r1' });
  mocks.refreshAsync.mockResolvedValue({ accessToken: 'a4', refreshToken: 'r4' });
  render(
    <AuthProvider>
      <Probe />
    </AuthProvider>,
  );
  await screen.findByText('a1');
  vi.stubGlobal('fetch', vi.fn().mockResolvedValue(new Response(null, { status: 401 })));

  await apiConfig.fetch('http://api/items');

  await screen.findByText('signed out');
  expect(mocks.deleteItemAsync).toHaveBeenCalledWith('refresh-token');
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
