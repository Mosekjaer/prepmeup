import { fireEvent, render, screen } from '@testing-library/react';
import { beforeEach, expect, it, vi } from 'vitest';

import OrdersScreen from '../src/app/(tabs)/orders';

const mocks = vi.hoisted(() => ({
  router: { push: vi.fn() },
  getSubscription: vi.fn(),
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
  subscriptionClient: { getSubscription: mocks.getSubscription },
}));

beforeEach(() => {
  mocks.router.push.mockReset();
  mocks.getSubscription.mockReset();
});

it('shows a loading indicator while the orders are loading', () => {
  mocks.getSubscription.mockReturnValue(new Promise(() => {}));

  render(<OrdersScreen />);

  expect(screen.getByRole('progressbar')).toBeTruthy();
});

const startedAt = '2026-10-01T10:26:32+00:00';
const plannedOrder = {
  plannedAt: '2027-01-14T10:00:00+00:00',
  sentAt: undefined,
  lines: [
    { packageTypeId: 'adult', packageTypeName: 'Normal - Adult', quantity: 2 },
    { packageTypeId: 'child', packageTypeName: 'Normal - Child', quantity: 1 },
  ],
};

it('shows the next order with its date, status and lines', async () => {
  mocks.getSubscription.mockResolvedValue({ status: 'Active', startedAt, orders: [plannedOrder] });

  render(<OrdersScreen />);

  expect(await screen.findByText('Planned for 14 Jan 2027')).toBeTruthy();
  expect(screen.getByText('Status: Planned')).toBeTruthy();
  expect(screen.getByText('Normal - Adult × 2')).toBeTruthy();
  expect(screen.getByText('Normal - Child × 1')).toBeTruthy();
});

it('shows the earliest planned order and skips orders that are sent', async () => {
  mocks.getSubscription.mockResolvedValue({
    status: 'Active',
    startedAt,
    orders: [
      { plannedAt: '2027-03-01T10:00:00+00:00', sentAt: undefined, lines: [] },
      { plannedAt: '2026-10-01T10:00:00+00:00', sentAt: '2026-10-02T09:00:00+00:00', lines: [] },
      plannedOrder,
    ],
  });

  render(<OrdersScreen />);

  expect(await screen.findByText('Planned for 14 Jan 2027')).toBeTruthy();
  expect(screen.queryByText('Planned for 1 Mar 2027')).toBeNull();
  expect(screen.queryByText('Planned for 1 Oct 2026')).toBeNull();
});

it('says that no order is planned when every order is sent', async () => {
  mocks.getSubscription.mockResolvedValue({
    status: 'Active',
    startedAt,
    orders: [{ plannedAt: '2026-10-01T10:00:00+00:00', sentAt: '2026-10-02T09:00:00+00:00', lines: [] }],
  });

  render(<OrdersScreen />);

  expect(await screen.findByText('No order planned.')).toBeTruthy();
  expect(screen.queryByText(/Planned for/)).toBeNull();
});

it('shows no lines when the next order has none', async () => {
  mocks.getSubscription.mockResolvedValue({
    status: 'Active',
    startedAt,
    orders: [{ plannedAt: '2027-01-14T10:00:00+00:00', sentAt: undefined, lines: [] }],
  });

  render(<OrdersScreen />);

  expect(await screen.findByText('Planned for 14 Jan 2027')).toBeTruthy();
  expect(screen.queryByText(/×/)).toBeNull();
});

it('says that no order is planned when the customer has no subscription', async () => {
  mocks.getSubscription.mockRejectedValue({ status: 404 });

  render(<OrdersScreen />);

  expect(await screen.findByText('No order planned.')).toBeTruthy();
  expect(screen.queryByRole('button', { name: 'Subscribe' })).toBeNull();
  expect(screen.queryByText('You do not have a subscription yet.')).toBeNull();
});

it('shows an error that can be retried when loading fails', async () => {
  mocks.getSubscription.mockRejectedValueOnce({ status: 500 });
  mocks.getSubscription.mockResolvedValueOnce({ status: 'Active', startedAt, orders: [plannedOrder] });
  render(<OrdersScreen />);
  expect(await screen.findByText('Could not load your orders.')).toBeTruthy();

  fireEvent.click(screen.getByRole('button', { name: 'Try again' }));

  expect(await screen.findByText('Planned for 14 Jan 2027')).toBeTruthy();
  expect(mocks.getSubscription).toHaveBeenCalledTimes(2);
});
