import { fireEvent, render, screen, waitFor } from '@testing-library/react';
import { beforeEach, expect, it, vi } from 'vitest';

import HomeScreen from '../src/app/index';

const mocks = vi.hoisted(() => ({
  token: undefined as string | undefined,
  router: { push: vi.fn(), back: vi.fn() },
  getCustomer: vi.fn(),
}));

vi.mock('expo-router', async () => {
  const { useEffect } = await import('react');
  return {
    useRouter: () => mocks.router,
    useFocusEffect: (effect: () => void) => useEffect(effect, [effect]),
  };
});

vi.mock('../src/lib/auth', () => ({
  useAuth: () => ({ token: mocks.token, signIn: vi.fn(), signOut: vi.fn() }),
}));

vi.mock('../src/lib/api', async (importOriginal) => ({
  ...(await importOriginal<typeof import('../src/lib/api')>()),
  userClient: { getCustomer: mocks.getCustomer },
}));

beforeEach(() => {
  mocks.token = 'access-token';
  mocks.router.push.mockReset();
  mocks.getCustomer.mockReset();
});

it('does not load the customer when signed out', () => {
  mocks.token = undefined;

  render(<HomeScreen />);

  expect(screen.getByText('Sign in')).toBeTruthy();
  expect(mocks.getCustomer).not.toHaveBeenCalled();
});

it('welcomes an onboarded customer by name', async () => {
  mocks.getCustomer.mockResolvedValue({ name: 'Malthe', address: 'Finlandsgade 22' });

  render(<HomeScreen />);

  expect(await screen.findByText('Welcome, Malthe')).toBeTruthy();
});

it('sends a customer who has not onboarded to onboarding', async () => {
  mocks.getCustomer.mockRejectedValue({ status: 404 });

  render(<HomeScreen />);

  await waitFor(() => expect(mocks.router.push).toHaveBeenCalledWith('/onboarding'));
});

it('shows an error that can be retried when loading fails', async () => {
  mocks.getCustomer.mockRejectedValue({ status: 500 });
  render(<HomeScreen />);

  fireEvent.click(await screen.findByText('Try again'));

  await waitFor(() => expect(mocks.getCustomer).toHaveBeenCalledTimes(2));
  expect(await screen.findByText('Could not load your profile.')).toBeTruthy();
});
