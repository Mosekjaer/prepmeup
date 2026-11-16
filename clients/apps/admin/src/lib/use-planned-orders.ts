import type { PlannedOrderResponse } from '@prepmeup/api-client';
import { useEffect, useState } from 'react';

import { adminClient } from './api';

type PlannedOrdersState =
  | { status: 'loading' }
  | { status: 'error' }
  | { status: 'ready'; orders: PlannedOrderResponse[] };

export function usePlannedOrders(): { state: PlannedOrdersState; retry: () => void } {
  const [state, setState] = useState<PlannedOrdersState>({ status: 'loading' });
  const [attempt, setAttempt] = useState(0);

  useEffect(() => {
    let cancelled = false;
    adminClient
      .getPlannedOrders()
      .then((orders) => {
        if (!cancelled) {
          setState({ status: 'ready', orders });
        }
      })
      .catch(() => {
        if (!cancelled) {
          setState({ status: 'error' });
        }
      });
    return () => {
      cancelled = true;
    };
  }, [attempt]);

  function retry() {
    setState({ status: 'loading' });
    setAttempt((current) => current + 1);
  }

  return { state, retry };
}
