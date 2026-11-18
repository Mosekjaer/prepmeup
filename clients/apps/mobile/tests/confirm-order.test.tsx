import { fireEvent, render, screen, waitFor } from '@testing-library/react';
import { beforeEach, expect, it, vi } from 'vitest';

import ConfirmOrderScreen from '../src/app/confirm-order';

const mocks = vi.hoisted(() => ({
  params: {} as { lines?: string },
  router: { push: vi.fn(), back: vi.fn(), replace: vi.fn(), dismissTo: vi.fn() },
  subscribe: vi.fn(),
}));

vi.mock('expo-router', () => ({
  useLocalSearchParams: () => mocks.params,
  useRouter: () => mocks.router,
}));

vi.mock('../src/lib/api', async (importOriginal) => ({
  ...(await importOriginal<typeof import('../src/lib/api')>()),
  subscriptionClient: { subscribe: mocks.subscribe },
}));

const chosenLines = [
  { packageTypeId: 'adult', name: 'Normal - Adult', quantity: 2 },
  { packageTypeId: 'child', name: 'Normal - Child', quantity: 1 },
];

beforeEach(() => {
  mocks.params = { lines: JSON.stringify(chosenLines) };
  Object.values(mocks.router).forEach((method) => method.mockReset());
  mocks.subscribe.mockReset();
});

it('shows the chosen package types with their quantities and the confirm button', () => {
  render(<ConfirmOrderScreen />);

  expect(screen.getByText('Normal - Adult × 2')).toBeTruthy();
  expect(screen.getByText('Normal - Child × 1')).toBeTruthy();
  expect(screen.getByRole('button', { name: 'Confirm and subscribe' })).toBeTruthy();
});

it('subscribes with the chosen lines when the customer confirms', () => {
  mocks.subscribe.mockReturnValue(new Promise(() => {}));
  render(<ConfirmOrderScreen />);

  fireEvent.click(screen.getByRole('button', { name: 'Confirm and subscribe' }));

  expect(mocks.subscribe).toHaveBeenCalledTimes(1);
  expect(mocks.subscribe).toHaveBeenCalledWith(
    expect.objectContaining({
      lines: [
        expect.objectContaining({ packageTypeId: 'adult', quantity: 2 }),
        expect.objectContaining({ packageTypeId: 'child', quantity: 1 }),
      ],
    }),
  );
});

it('disables the button and shows an indicator while the subscription is being created', () => {
  mocks.subscribe.mockReturnValue(new Promise(() => {}));
  render(<ConfirmOrderScreen />);

  fireEvent.click(screen.getByRole('button', { name: 'Confirm and subscribe' }));
  fireEvent.click(screen.getByRole('button', { name: 'Confirm and subscribe' }));

  expect(screen.getByRole('progressbar')).toBeTruthy();
  expect(screen.getByRole('button', { name: 'Confirm and subscribe' }).getAttribute('aria-disabled')).toBe('true');
  expect(mocks.subscribe).toHaveBeenCalledTimes(1);
});

it('goes back to Home when the subscription is created', async () => {
  mocks.subscribe.mockResolvedValue({ status: 'Active', startedAt: '2026-11-12T10:00:00+00:00', orders: [] });
  render(<ConfirmOrderScreen />);

  fireEvent.click(screen.getByRole('button', { name: 'Confirm and subscribe' }));

  await waitFor(() => expect(mocks.router.dismissTo).toHaveBeenCalledWith('/'));
});

it('says that the customer already has a subscription and offers Home when the API answers 409', async () => {
  mocks.subscribe.mockRejectedValue({ status: 409 });
  render(<ConfirmOrderScreen />);

  fireEvent.click(screen.getByRole('button', { name: 'Confirm and subscribe' }));

  expect(await screen.findByText('You already have a subscription.')).toBeTruthy();
  fireEvent.click(screen.getByRole('button', { name: 'Go to Home' }));
  expect(mocks.router.dismissTo).toHaveBeenCalledWith('/');
});

it('says that a package type is no longer available and goes back to the order when the API answers 400', async () => {
  mocks.subscribe.mockRejectedValue({ status: 400 });
  render(<ConfirmOrderScreen />);

  fireEvent.click(screen.getByRole('button', { name: 'Confirm and subscribe' }));

  expect(await screen.findByText('One of the package types is no longer available.')).toBeTruthy();
  fireEvent.click(screen.getByRole('button', { name: 'Back to order' }));
  expect(mocks.router.back).toHaveBeenCalledTimes(1);
});

it('shows an error that can be retried when subscribing fails for another reason', async () => {
  mocks.subscribe.mockRejectedValueOnce({ status: 500 });
  mocks.subscribe.mockResolvedValueOnce({ status: 'Active', startedAt: '2026-11-12T10:00:00+00:00', orders: [] });
  render(<ConfirmOrderScreen />);
  fireEvent.click(screen.getByRole('button', { name: 'Confirm and subscribe' }));
  expect(await screen.findByText('Could not create your subscription.')).toBeTruthy();

  fireEvent.click(screen.getByRole('button', { name: 'Try again' }));

  await waitFor(() => expect(mocks.router.dismissTo).toHaveBeenCalledWith('/'));
  expect(mocks.subscribe).toHaveBeenCalledTimes(2);
});

it.each([
  ['the lines are missing', undefined],
  ['the lines are not valid JSON', 'not json'],
  ['the list of lines is empty', '[]'],
])('says that there is nothing in the order when %s', (_case, lines) => {
  mocks.params = lines === undefined ? {} : { lines };

  render(<ConfirmOrderScreen />);

  expect(screen.getByText('There is nothing in your order.')).toBeTruthy();
  expect(screen.queryByRole('button', { name: 'Confirm and subscribe' })).toBeNull();
  expect(mocks.subscribe).not.toHaveBeenCalled();
});
