import { PlannedOrderResponse } from '@prepmeup/api-client';
import { fireEvent, render, screen, within } from '@testing-library/react';
import { beforeEach, expect, it, vi } from 'vitest';

import { DispatchQueueRoute } from '../src/routes/dispatch-queue';

const mocks = vi.hoisted(() => ({
  usePlannedOrders: vi.fn(),
  useMarkOrderShipped: vi.fn(),
  retry: vi.fn(),
  markShipped: vi.fn(),
}));

vi.mock('../src/lib/use-planned-orders', () => ({
  usePlannedOrders: mocks.usePlannedOrders,
}));

vi.mock('../src/lib/use-mark-order-shipped', () => ({
  useMarkOrderShipped: mocks.useMarkOrderShipped,
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
  mocks.useMarkOrderShipped.mockReset();
  mocks.retry.mockReset();
  mocks.markShipped.mockReset();
  mocks.useMarkOrderShipped.mockReturnValue({
    state: { status: 'idle' },
    markShipped: mocks.markShipped,
  });
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

it('shows a Mark as shipped button for every order', () => {
  const orders = [
    orderFor('Malthe', '2026-12-01T10:00:00+01:00'),
    orderFor('Anna', '2026-12-02T10:00:00+01:00'),
  ];
  mocks.usePlannedOrders.mockReturnValue({ state: { status: 'ready', orders }, retry: mocks.retry });

  render(<DispatchQueueRoute />);

  expect(screen.getAllByRole('button', { name: 'Mark as shipped' })).toHaveLength(2);
});

it('marks the order in the pressed row as shipped', () => {
  const orders = [
    orderFor('Malthe', '2026-12-01T10:00:00+01:00'),
    orderFor('Anna', '2026-12-02T10:00:00+01:00'),
  ];
  mocks.usePlannedOrders.mockReturnValue({ state: { status: 'ready', orders }, retry: mocks.retry });
  render(<DispatchQueueRoute />);

  const annaRow = screen.getByText('Anna').closest('tr') as HTMLElement;
  fireEvent.click(within(annaRow).getByRole('button', { name: 'Mark as shipped' }));

  expect(mocks.markShipped).toHaveBeenCalledTimes(1);
  expect(mocks.markShipped).toHaveBeenCalledWith('order-Anna');
});

it('disables the buttons while an order is being marked as shipped', () => {
  const orders = [
    orderFor('Malthe', '2026-12-01T10:00:00+01:00'),
    orderFor('Anna', '2026-12-02T10:00:00+01:00'),
  ];
  mocks.usePlannedOrders.mockReturnValue({ state: { status: 'ready', orders }, retry: mocks.retry });
  mocks.useMarkOrderShipped.mockReturnValue({
    state: { status: 'pending', orderId: 'order-Malthe' },
    markShipped: mocks.markShipped,
  });

  render(<DispatchQueueRoute />);

  for (const button of screen.getAllByRole('button', { name: 'Mark as shipped' })) {
    expect(button).toBeDisabled();
  }
});

it('shows the error message when the order could not be marked as shipped', () => {
  const orders = [orderFor('Malthe', '2026-12-01T10:00:00+01:00')];
  mocks.usePlannedOrders.mockReturnValue({ state: { status: 'ready', orders }, retry: mocks.retry });
  mocks.useMarkOrderShipped.mockReturnValue({
    state: { status: 'error', orderId: 'order-Malthe', message: 'This order has already been shipped.' },
    markShipped: mocks.markShipped,
  });

  render(<DispatchQueueRoute />);

  expect(screen.getByRole('alert')).toHaveTextContent('This order has already been shipped.');
});

it('reloads the planned orders when the hook is done', () => {
  const orders = [orderFor('Malthe', '2026-12-01T10:00:00+01:00')];
  mocks.usePlannedOrders.mockReturnValue({ state: { status: 'ready', orders }, retry: mocks.retry });

  render(<DispatchQueueRoute />);

  expect(mocks.useMarkOrderShipped).toHaveBeenCalledWith(mocks.retry);
});
