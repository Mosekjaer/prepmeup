import { PlannedOrderResponse } from '@prepmeup/api-client';
import { fireEvent, render, screen, within } from '@testing-library/react';
import { beforeEach, expect, it, vi } from 'vitest';

import { DispatchQueueRoute } from '../src/routes/dispatch-queue';

const mocks = vi.hoisted(() => ({
  usePlannedOrders: vi.fn(),
  retry: vi.fn(),
}));

vi.mock('../src/lib/use-planned-orders', () => ({
  usePlannedOrders: mocks.usePlannedOrders,
}));

function orderFor(customerName: string, plannedAt: string) {
  return PlannedOrderResponse.fromJS({
    orderId: `order-${customerName}`,
    plannedAt,
    customerName,
    address: { street: 'Finlandsgade 22', postalCode: '8200', city: 'Aarhus N' },
    lines: [{ packageTypeName: 'Normal - Adult', quantity: 2 }],
  });
}

beforeEach(() => {
  mocks.usePlannedOrders.mockReset();
  mocks.retry.mockReset();
});

it('shows a loading indicator and no table while loading', () => {
  mocks.usePlannedOrders.mockReturnValue({ state: { status: 'loading' }, retry: mocks.retry });

  render(<DispatchQueueRoute />);

  expect(screen.getByRole('status')).toHaveTextContent('Loading');
  expect(screen.queryByRole('table')).not.toBeInTheDocument();
});

it('shows an error message and retries when Try again is pressed', () => {
  mocks.usePlannedOrders.mockReturnValue({ state: { status: 'error' }, retry: mocks.retry });

  render(<DispatchQueueRoute />);
  fireEvent.click(screen.getByRole('button', { name: 'Try again' }));

  expect(screen.getByText('We could not load the planned orders. Please try again.')).toBeInTheDocument();
  expect(mocks.retry).toHaveBeenCalledTimes(1);
});

it('explains that there is nothing to send when the list is empty', () => {
  mocks.usePlannedOrders.mockReturnValue({ state: { status: 'ready', orders: [] }, retry: mocks.retry });

  render(<DispatchQueueRoute />);

  expect(screen.getByText('There are no planned orders to send right now.')).toBeInTheDocument();
  expect(screen.queryByRole('table')).not.toBeInTheDocument();
});

it('shows the customer, address and contents of an order', () => {
  const orders = [orderFor('Malthe', '2026-12-01T10:00:00+01:00')];
  mocks.usePlannedOrders.mockReturnValue({ state: { status: 'ready', orders }, retry: mocks.retry });

  render(<DispatchQueueRoute />);

  expect(screen.getByText('Malthe')).toBeInTheDocument();
  expect(screen.getByText('Finlandsgade 22, 8200 Aarhus N')).toBeInTheDocument();
  expect(screen.getByText('2 × Normal - Adult')).toBeInTheDocument();
  expect(screen.queryByText('There are no planned orders to send right now.')).not.toBeInTheDocument();
});

it('keeps the order the API sent the orders in', () => {
  const orders = [
    orderFor('Later', '2026-12-02T10:00:00+01:00'),
    orderFor('Earlier', '2026-12-01T10:00:00+01:00'),
  ];
  mocks.usePlannedOrders.mockReturnValue({ state: { status: 'ready', orders }, retry: mocks.retry });

  render(<DispatchQueueRoute />);

  const customers = screen
    .getAllByRole('row')
    .slice(1)
    .map((row) => within(row).getAllByRole('cell')[1]?.textContent);
  expect(customers).toEqual(['Later', 'Earlier']);
});
