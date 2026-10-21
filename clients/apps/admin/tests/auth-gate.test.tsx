import { apiConfig, setAccessTokenProvider } from '@prepmeup/api-client';
import { act, fireEvent, render, screen, waitFor } from '@testing-library/react';
import { beforeEach, expect, it, vi } from 'vitest';

import { apiBaseUrl } from '../src/lib/api';
import { AuthGate } from '../src/routes/auth-gate';

const mocks = vi.hoisted(() => ({
  auth: {
    isAuthenticated: false,
    isLoading: false,
    user: null as { access_token: string; id_token?: string } | null,
    signinRedirect: vi.fn(),
    signoutRedirect: vi.fn(),
    removeUser: vi.fn(),
  },
  fetch: vi.fn(),
}));

vi.mock('react-oidc-context', () => ({
  useAuth: () => mocks.auth,
}));

beforeEach(() => {
  mocks.auth.isAuthenticated = false;
  mocks.auth.isLoading = false;
  mocks.auth.user = null;
  mocks.auth.signinRedirect.mockReset();
  mocks.auth.signoutRedirect.mockReset();
  mocks.auth.removeUser.mockReset();
  mocks.fetch.mockReset();
  vi.stubGlobal('fetch', mocks.fetch);
  setAccessTokenProvider(() => undefined);
});

it('shows the sign-in screen when signed out', async () => {
  render(
    <AuthGate>
      <p>Panel</p>
    </AuthGate>,
  );
  await act(async () => undefined);

  expect(screen.getByRole('button', { name: 'Sign in' })).toBeInTheDocument();
  expect(screen.queryByText('Panel')).not.toBeInTheDocument();
  expect(mocks.fetch).not.toHaveBeenCalled();
});

it('starts the Keycloak redirect when Sign in is pressed', () => {
  render(
    <AuthGate>
      <p>Panel</p>
    </AuthGate>,
  );

  fireEvent.click(screen.getByRole('button', { name: 'Sign in' }));

  expect(mocks.auth.signinRedirect).toHaveBeenCalledTimes(1);
});

it('shows a loading indicator while the library is loading', () => {
  mocks.auth.isLoading = true;

  render(
    <AuthGate>
      <p>Panel</p>
    </AuthGate>,
  );

  expect(screen.getByRole('status')).toHaveTextContent('Loading');
  expect(screen.queryByRole('button', { name: 'Sign in' })).not.toBeInTheDocument();
  expect(screen.queryByText('Panel')).not.toBeInTheDocument();
});

it('sends the access token on the admin ping', async () => {
  mocks.auth.isAuthenticated = true;
  mocks.auth.user = { access_token: 'access-token' };
  mocks.fetch.mockResolvedValue(new Response(null, { status: 200 }));

  render(
    <AuthGate>
      <p>Panel</p>
    </AuthGate>,
  );

  await waitFor(() => expect(mocks.fetch).toHaveBeenCalledTimes(1));
  expect(mocks.fetch.mock.calls[0]?.[0]).toBe(`${apiBaseUrl}/admin/ping`);
  expect(new Headers(mocks.fetch.mock.calls[0]?.[1]?.headers).get('Authorization')).toBe('Bearer access-token');
});

it('shows the panel when ping answers 200', async () => {
  mocks.auth.isAuthenticated = true;
  mocks.auth.user = { access_token: 'access-token' };
  let answer: (response: Response) => void = () => undefined;
  mocks.fetch.mockReturnValue(
    new Promise<Response>((resolve) => {
      answer = resolve;
    }),
  );

  render(
    <AuthGate>
      <p>Panel</p>
    </AuthGate>,
  );

  expect(screen.getByRole('status')).toBeInTheDocument();
  expect(screen.queryByText('Panel')).not.toBeInTheDocument();

  await act(async () => answer(new Response(null, { status: 200 })));

  expect(screen.getByText('Panel')).toBeInTheDocument();
  expect(screen.queryByRole('status')).not.toBeInTheDocument();
});

it('shows a denial message when ping answers 403', async () => {
  mocks.auth.isAuthenticated = true;
  mocks.auth.user = { access_token: 'access-token' };
  mocks.fetch.mockResolvedValue(new Response(null, { status: 403 }));

  render(
    <AuthGate>
      <p>Panel</p>
    </AuthGate>,
  );

  expect(await screen.findByText('This account does not have access to the admin panel.')).toBeInTheDocument();
  expect(screen.queryByText('Panel')).not.toBeInTheDocument();
  expect(screen.queryByRole('status')).not.toBeInTheDocument();
});

it('ends the Keycloak session on sign out', async () => {
  mocks.auth.isAuthenticated = true;
  mocks.auth.user = { access_token: 'access-token', id_token: 'id-token' };
  mocks.fetch.mockResolvedValue(new Response(null, { status: 403 }));

  render(
    <AuthGate>
      <p>Panel</p>
    </AuthGate>,
  );
  fireEvent.click(await screen.findByRole('button', { name: 'Sign out' }));

  expect(mocks.auth.signoutRedirect).toHaveBeenCalledWith({ id_token_hint: 'id-token' });
});

it('sends the user to the sign-in screen when ping answers 401', async () => {
  mocks.auth.isAuthenticated = true;
  mocks.auth.user = { access_token: 'expired-token' };
  mocks.fetch.mockResolvedValue(new Response(null, { status: 401 }));

  render(
    <AuthGate>
      <p>Panel</p>
    </AuthGate>,
  );

  await waitFor(() => expect(mocks.auth.removeUser).toHaveBeenCalledTimes(1));
  expect(screen.queryByText('Panel')).not.toBeInTheDocument();
});

it('offers a retry when ping fails', async () => {
  mocks.auth.isAuthenticated = true;
  mocks.auth.user = { access_token: 'access-token' };
  mocks.fetch
    .mockResolvedValueOnce(new Response(null, { status: 500 }))
    .mockResolvedValueOnce(new Response(null, { status: 200 }));

  render(
    <AuthGate>
      <p>Panel</p>
    </AuthGate>,
  );

  expect(await screen.findByText('We could not check your access. Please try again.')).toBeInTheDocument();
  expect(screen.queryByText('Panel')).not.toBeInTheDocument();

  fireEvent.click(screen.getByRole('button', { name: 'Try again' }));

  expect(await screen.findByText('Panel')).toBeInTheDocument();
  expect(mocks.fetch).toHaveBeenCalledTimes(2);
});

it('stops sending the token after sign out', async () => {
  mocks.auth.isAuthenticated = true;
  mocks.auth.user = { access_token: 'access-token' };
  mocks.fetch.mockResolvedValue(new Response(null, { status: 200 }));
  const view = render(
    <AuthGate>
      <p>Panel</p>
    </AuthGate>,
  );
  await screen.findByText('Panel');

  mocks.auth.isAuthenticated = false;
  mocks.auth.user = null;
  view.rerender(
    <AuthGate>
      <p>Panel</p>
    </AuthGate>,
  );
  mocks.fetch.mockClear();
  await apiConfig.fetch('http://api/admin/ping');

  expect(new Headers(mocks.fetch.mock.calls[0]?.[1]?.headers).get('Authorization')).toBeNull();
});
