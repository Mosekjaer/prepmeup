import { fireEvent, render, screen } from '@testing-library/react';
import { beforeEach, expect, it, vi } from 'vitest';

import ProfileScreen from '../src/app/(tabs)/profile';

const mocks = vi.hoisted(() => ({
  router: { push: vi.fn() },
  getCustomer: vi.fn(),
}));

vi.mock('expo-router', async () => {
  const { useEffect } = await import('react');
  return {
    useRouter: () => mocks.router,
    useFocusEffect: (effect: () => void) => useEffect(effect, [effect]),
  };
});

vi.mock('../src/lib/api', async (importOriginal) => ({
  ...(await importOriginal<typeof import('../src/lib/api')>()),
  customerClient: { getCustomer: mocks.getCustomer },
}));

beforeEach(() => {
  mocks.router.push.mockReset();
  mocks.getCustomer.mockReset();
});

it('shows a loading indicator while the profile is loading', () => {
  mocks.getCustomer.mockReturnValue(new Promise(() => {}));

  render(<ProfileScreen />);

  expect(screen.getByRole('progressbar')).toBeTruthy();
});

const customer = {
  name: 'Malthe',
  email: 'malthe@example.com',
  phone: '12345678',
  address: { street: 'Finlandsgade 22', postalCode: '8200', city: 'Aarhus N' },
};

it('shows the name, e-mail, phone and address of the customer', async () => {
  mocks.getCustomer.mockResolvedValue(customer);

  render(<ProfileScreen />);

  expect(await screen.findByText('Malthe')).toBeTruthy();
  expect(screen.getByText('malthe@example.com')).toBeTruthy();
  expect(screen.getByText('12345678')).toBeTruthy();
  expect(screen.getByText('Finlandsgade 22')).toBeTruthy();
  expect(screen.getByText('8200 Aarhus N')).toBeTruthy();
});

it('shows no phone line when the customer has no phone', async () => {
  mocks.getCustomer.mockResolvedValue({ ...customer, phone: undefined });

  const { container } = render(<ProfileScreen />);
  await screen.findByText('Malthe');

  expect(container.querySelectorAll('[dir="auto"]')).toHaveLength(4);
});

it('asks a customer who has not onboarded to fill in the details', async () => {
  mocks.getCustomer.mockRejectedValue({ status: 404 });
  render(<ProfileScreen />);

  expect(await screen.findByText('You have not filled in your details yet.')).toBeTruthy();
  fireEvent.click(screen.getByRole('button', { name: 'Fill in details' }));

  expect(mocks.router.push).toHaveBeenCalledWith('/onboarding');
});

it('shows an error that can be retried when loading fails', async () => {
  mocks.getCustomer.mockRejectedValueOnce({ status: 500 });
  mocks.getCustomer.mockResolvedValueOnce(customer);
  render(<ProfileScreen />);
  expect(await screen.findByText('Could not load your profile.')).toBeTruthy();

  fireEvent.click(screen.getByRole('button', { name: 'Try again' }));

  expect(await screen.findByText('Malthe')).toBeTruthy();
  expect(mocks.getCustomer).toHaveBeenCalledTimes(2);
});
