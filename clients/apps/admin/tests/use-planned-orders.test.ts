import { PlannedOrderResponse } from '@prepmeup/api-client';
import { act, renderHook, waitFor } from '@testing-library/react';
import { beforeEach, expect, it, vi } from 'vitest';

import { usePlannedOrders } from '../src/lib/use-planned-orders';

const mocks = vi.hoisted(() => ({
  getPlannedOrders: vi.fn(),
}));

vi.mock('../src/lib/api', () => ({
  adminClient: { getPlannedOrders: mocks.getPlannedOrders },
}));

const order = PlannedOrderResponse.fromJS({
  orderId: 'order-1',
  plannedAt: '2026-12-01T10:00:00+01:00',
  customerName: 'Malthe',
  address: { street: 'Finlandsgade 22', postalCode: '8200', city: 'Aarhus N' },
  lines: [{ packageTypeName: 'Normal - Adult', quantity: 2 }],
});

beforeEach(() => {
  mocks.getPlannedOrders.mockReset();
});

it('starts in the loading state', () => {
  mocks.getPlannedOrders.mockReturnValue(new Promise(() => undefined));

  const { result } = renderHook(() => usePlannedOrders());

  expect(result.current.state).toEqual({ status: 'loading' });
});

it('is ready with the orders when the request succeeds', async () => {
  mocks.getPlannedOrders.mockResolvedValue([order]);

  const { result } = renderHook(() => usePlannedOrders());

  await waitFor(() => expect(result.current.state).toEqual({ status: 'ready', orders: [order] }));
});

it('is ready with an empty list when there are no orders', async () => {
  mocks.getPlannedOrders.mockResolvedValue([]);

  const { result } = renderHook(() => usePlannedOrders());

  await waitFor(() => expect(result.current.state).toEqual({ status: 'ready', orders: [] }));
});

it('is in the error state when the request fails', async () => {
  mocks.getPlannedOrders.mockRejectedValue(new Error('network'));

  const { result } = renderHook(() => usePlannedOrders());

  await waitFor(() => expect(result.current.state).toEqual({ status: 'error' }));
});

it('loads again when retry is called', async () => {
  mocks.getPlannedOrders
    .mockRejectedValueOnce(new Error('network'))
    .mockResolvedValueOnce([order]);
  const { result } = renderHook(() => usePlannedOrders());
  await waitFor(() => expect(result.current.state.status).toBe('error'));

  act(() => result.current.retry());

  expect(result.current.state).toEqual({ status: 'loading' });
  await waitFor(() => expect(result.current.state).toEqual({ status: 'ready', orders: [order] }));
  expect(mocks.getPlannedOrders).toHaveBeenCalledTimes(2);
});
