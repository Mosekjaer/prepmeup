import type { PlannedOrderResponse } from '@prepmeup/api-client';

import { useMarkOrderShipped } from '../lib/use-mark-order-shipped';
import { usePlannedOrders } from '../lib/use-planned-orders';

const plannedFormat = new Intl.DateTimeFormat('en-GB', {
  dateStyle: 'medium',
  timeStyle: 'short',
});

function PlannedOrdersTable({
  orders,
  onMarkShipped,
  disabled,
}: {
  orders: PlannedOrderResponse[];
  onMarkShipped: (orderId: string) => void;
  disabled: boolean;
}) {
  return (
    <table>
      <thead>
        <tr>
          <th>Planned</th>
          <th>Customer</th>
          <th>Address</th>
          <th>Contents</th>
          <th>Action</th>
        </tr>
      </thead>
      <tbody>
        {orders.map((order) => (
          <tr key={order.orderId}>
            <td>{plannedFormat.format(new Date(order.plannedAt))}</td>
            <td>{order.customerName}</td>
            <td>
              {order.address.street}, {order.address.postalCode} {order.address.city}
            </td>
            <td>
              <ul>
                {order.lines.map((line) => (
                  <li key={line.packageTypeName}>
                    {line.quantity} × {line.packageTypeName}
                  </li>
                ))}
              </ul>
            </td>
            <td>
              <button type="button" disabled={disabled} onClick={() => onMarkShipped(order.orderId)}>
                Mark as shipped
              </button>
            </td>
          </tr>
        ))}
      </tbody>
    </table>
  );
}

export function DispatchQueueRoute() {
  const { state, retry } = usePlannedOrders();
  const { state: shipState, markShipped } = useMarkOrderShipped(retry);
  return (
    <section>
      <h1>Dispatch queue</h1>
      {shipState.status === 'error' && <p role="alert">{shipState.message}</p>}
      {state.status === 'loading' && <p role="status">Loading…</p>}
      {state.status === 'error' && (
        <>
          <p>We could not load the planned orders. Please try again.</p>
          <button onClick={retry}>Try again</button>
        </>
      )}
      {state.status === 'ready' && state.orders.length === 0 && (
        <p>There are no planned orders to send right now.</p>
      )}
      {state.status === 'ready' && state.orders.length > 0 && (
        <PlannedOrdersTable
          orders={state.orders}
          onMarkShipped={markShipped}
          disabled={shipState.status === 'pending'}
        />
      )}
    </section>
  );
}
