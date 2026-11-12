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

it('shows when the subscription started and the next planned order', async () => {
  mocks.getSubscription.mockResolvedValue({
    status: 'Active',
    startedAt: '2026-10-01T10:26:32+00:00',
    orders: [plannedOrder],
  });

  render(<SubscriptionSummary />);

  expect(await screen.findByText('Active since 1 Oct 2026')).toBeTruthy();
  expect(screen.getByText('Planned for 14 Jan 2027')).toBeTruthy();
  expect(screen.getByText('Status: Planned')).toBeTruthy();
});

it('shows the earliest planned order and skips orders that are sent', async () => {
  mocks.getSubscription.mockResolvedValue({
    status: 'Active',
    startedAt: '2026-10-01T10:26:32+00:00',
    orders: [
      { plannedAt: '2027-03-01T10:00:00+00:00', sentAt: undefined, lines: [] },
      { plannedAt: '2026-10-01T10:00:00+00:00', sentAt: '2026-10-02T09:00:00+00:00', lines: [] },
      { plannedAt: '2027-01-14T10:00:00+00:00', sentAt: undefined, lines: [] },
    ],
  });

  render(<SubscriptionSummary />);

  expect(await screen.findByText('Planned for 14 Jan 2027')).toBeTruthy();
  expect(screen.queryByText('Planned for 1 Mar 2027')).toBeNull();
  expect(screen.queryByText('Planned for 1 Oct 2026')).toBeNull();
});

it('says that no order is planned when every order is sent', async () => {
  mocks.getSubscription.mockResolvedValue({
    status: 'Active',
    startedAt: '2026-10-01T10:26:32+00:00',
    orders: [{ plannedAt: '2026-10-01T10:00:00+00:00', sentAt: '2026-10-02T09:00:00+00:00', lines: [] }],
  });

  render(<SubscriptionSummary />);

  expect(await screen.findByText('No order planned.')).toBeTruthy();
  expect(screen.queryByText(/Planned for/)).toBeNull();
});

it('offers to subscribe when the customer has no subscription', async () => {
  mocks.getSubscription.mockRejectedValue({ status: 404 });

  render(<SubscriptionSummary />);

  expect(await screen.findByText('You do not have a subscription yet.')).toBeTruthy();
  expect(screen.getByText('Subscribe')).toBeTruthy();
});

it('opens the order builder instead of subscribing right away', async () => {
  mocks.getSubscription.mockRejectedValue({ status: 404 });
  render(<SubscriptionSummary />);

  fireEvent.click(await screen.findByText('Subscribe'));

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
  expect(await screen.findByText('Could not load your subscription.')).toBeTruthy();

  fireEvent.click(screen.getByText('Try again'));

  expect(await screen.findByText('Active since 1 Oct 2026')).toBeTruthy();
  expect(mocks.getSubscription).toHaveBeenCalledTimes(2);
});

it('shows a loading indicator while the subscription is loading', () => {
  mocks.getSubscription.mockReturnValue(new Promise(() => {}));

  render(<SubscriptionSummary />);

  expect(screen.getByRole('progressbar')).toBeTruthy();
});

it('shows the lines of the next order', async () => {
  mocks.getSubscription.mockResolvedValue({
    status: 'Active',
    startedAt: '2026-10-01T10:26:32+00:00',
    orders: [
      {
        plannedAt: '2027-01-14T10:00:00+00:00',
        sentAt: undefined,
        lines: [
          { packageTypeId: 'adult', packageTypeName: 'Normal - Adult', quantity: 2 },
          { packageTypeId: 'child', packageTypeName: 'Normal - Child', quantity: 1 },
        ],
      },
    ],
  });

  render(<SubscriptionSummary />);

  expect(await screen.findByText('Normal - Adult × 2')).toBeTruthy();
  expect(screen.getByText('Normal - Child × 1')).toBeTruthy();
});

it('shows no lines when the next order has none', async () => {
  mocks.getSubscription.mockResolvedValue({
    status: 'Active',
    startedAt: '2026-10-01T10:26:32+00:00',
    orders: [plannedOrder],
  });

  render(<SubscriptionSummary />);

  expect(await screen.findByText('Planned for 14 Jan 2027')).toBeTruthy();
  expect(screen.queryByText(/×/)).toBeNull();
});
