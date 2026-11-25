import { fireEvent, render, screen } from '@testing-library/react';
import { beforeEach, expect, it, vi } from 'vitest';

import { SubscriptionSummary } from '../src/components/SubscriptionSummary';

const mocks = vi.hoisted(() => ({
  router: { push: vi.fn() },
  getSubscription: vi.fn(),
  subscribe: vi.fn(),
}));

vi.mock('expo-router', () => ({
  useRouter: () => mocks.router,
}));

vi.mock('../src/lib/api', async (importOriginal) => ({
  ...(await importOriginal<typeof import('../src/lib/api')>()),
  subscriptionClient: { getSubscription: mocks.getSubscription, subscribe: mocks.subscribe },
}));

beforeEach(() => {
  mocks.router.push.mockReset();
  mocks.getSubscription.mockReset();
  mocks.subscribe.mockReset();
});

const plannedOrder = { plannedAt: '2027-01-14T10:00:00+00:00', sentAt: undefined, lines: [] };

it('shows when the subscription started but not the orders, which belong to the Orders tab', async () => {
  mocks.getSubscription.mockResolvedValue({
    status: 'Active',
    startedAt: '2026-10-01T10:26:32+00:00',
    orders: [
      {
        plannedAt: '2027-01-14T10:00:00+00:00',
        sentAt: undefined,
        lines: [{ packageTypeId: 'adult', packageTypeName: 'Normal - Adult', quantity: 2 }],
      },
    ],
  });

  render(<SubscriptionSummary />);

  expect(await screen.findByText('Active since 1 Oct 2026')).toBeTruthy();
  expect(screen.queryByText(/Planned for/)).toBeNull();
  expect(screen.queryByText(/Status:/)).toBeNull();
  expect(screen.queryByText(/×/)).toBeNull();
});

it('offers to subscribe when the customer has no subscription', async () => {
  mocks.getSubscription.mockRejectedValue({ status: 404 });

  render(<SubscriptionSummary />);

  expect(await screen.findByText('You do not have a subscription yet.')).toBeTruthy();
  expect(screen.getByText('Prep me up')).toBeTruthy();
});

it('opens the order builder instead of subscribing right away', async () => {
  mocks.getSubscription.mockRejectedValue({ status: 404 });
  render(<SubscriptionSummary />);

  fireEvent.click(await screen.findByText('Prep me up'));

  expect(mocks.router.push).toHaveBeenCalledWith('/build-order');
  expect(mocks.subscribe).not.toHaveBeenCalled();
});

it('shows an error that can be retried when loading fails', async () => {
  mocks.getSubscription.mockRejectedValueOnce({ status: 500 });
  mocks.getSubscription.mockResolvedValueOnce({
    status: 'Active',
    startedAt: '2026-10-01T10:26:32+00:00',
    orders: [plannedOrder],
  });
  render(<SubscriptionSummary />);
  expect(await screen.findByText('We could not load your subscription. Check your internet connection and try again.')).toBeTruthy();

  fireEvent.click(screen.getByText('Try again'));

  expect(await screen.findByText('Active since 1 Oct 2026')).toBeTruthy();
  expect(mocks.getSubscription).toHaveBeenCalledTimes(2);
});

it('shows a loading indicator while the subscription is loading', () => {
  mocks.getSubscription.mockReturnValue(new Promise(() => {}));

  render(<SubscriptionSummary />);

  expect(screen.getByRole('progressbar')).toBeTruthy();
});
