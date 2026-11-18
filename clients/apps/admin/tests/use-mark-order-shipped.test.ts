import { ProblemDetails } from '@prepmeup/api-client';
import { act, renderHook, waitFor } from '@testing-library/react';
import { beforeEach, expect, it, vi } from 'vitest';

import { useMarkOrderShipped } from '../src/lib/use-mark-order-shipped';

const mocks = vi.hoisted(() => ({
  markOrderShipped: vi.fn(),
  onDone: vi.fn(),
}));

vi.mock('../src/lib/api', () => ({
  adminClient: { markOrderShipped: mocks.markOrderShipped },
}));

beforeEach(() => {
  mocks.markOrderShipped.mockReset();
  mocks.onDone.mockReset();
});

it('starts idle', () => {
  const { result } = renderHook(() => useMarkOrderShipped(mocks.onDone));

  expect(result.current.state).toEqual({ status: 'idle' });
});

it('is pending for the order while the request is in flight', () => {
  mocks.markOrderShipped.mockReturnValue(new Promise(() => undefined));
  const { result } = renderHook(() => useMarkOrderShipped(mocks.onDone));

  act(() => result.current.markShipped('order-1'));

  expect(mocks.markOrderShipped).toHaveBeenCalledWith('order-1');
  expect(result.current.state).toEqual({ status: 'pending', orderId: 'order-1' });
});

it('calls onDone and goes back to idle when the order is shipped', async () => {
  mocks.markOrderShipped.mockResolvedValue(undefined);
  const { result } = renderHook(() => useMarkOrderShipped(mocks.onDone));

  act(() => result.current.markShipped('order-1'));

  await waitFor(() => expect(result.current.state).toEqual({ status: 'idle' }));
  expect(mocks.onDone).toHaveBeenCalledTimes(1);
});

it('explains that the order was already shipped and calls onDone on a 409', async () => {
  mocks.markOrderShipped.mockRejectedValue(ProblemDetails.fromJS({ status: 409 }));
  const { result } = renderHook(() => useMarkOrderShipped(mocks.onDone));

  act(() => result.current.markShipped('order-1'));

  await waitFor(() =>
    expect(result.current.state).toEqual({
      status: 'error',
      orderId: 'order-1',
      message: 'This order has already been shipped.',
    }),
  );
  expect(mocks.onDone).toHaveBeenCalledTimes(1);
});

it('explains that the order was not found and calls onDone on a 404', async () => {
  mocks.markOrderShipped.mockRejectedValue(ProblemDetails.fromJS({ status: 404 }));
  const { result } = renderHook(() => useMarkOrderShipped(mocks.onDone));

  act(() => result.current.markShipped('order-1'));

  await waitFor(() =>
    expect(result.current.state).toEqual({
      status: 'error',
      orderId: 'order-1',
      message: 'This order could not be found.',
    }),
  );
  expect(mocks.onDone).toHaveBeenCalledTimes(1);
});

it('shows a general message and does not call onDone on any other error', async () => {
  mocks.markOrderShipped.mockRejectedValue(new Error('network'));
  const { result } = renderHook(() => useMarkOrderShipped(mocks.onDone));

  act(() => result.current.markShipped('order-1'));

  await waitFor(() =>
    expect(result.current.state).toEqual({
      status: 'error',
      orderId: 'order-1',
      message: 'We could not mark the order as shipped. Please try again.',
    }),
  );
  expect(mocks.onDone).not.toHaveBeenCalled();
});
