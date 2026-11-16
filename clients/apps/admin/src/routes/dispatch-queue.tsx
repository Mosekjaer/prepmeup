import type { PlannedOrderResponse } from '@prepmeup/api-client';

import { usePlannedOrders } from '../lib/use-planned-orders';

const plannedFormat = new Intl.DateTimeFormat('en-GB', {
  dateStyle: 'medium',
  timeStyle: 'short',
});

function PlannedOrdersTable({ orders }: { orders: PlannedOrderResponse[] }) {
  return (
    <table>
      <thead>
        <tr>
          <th>Planned</th>
          <th>Customer</th>
          <th>Address</th>
          <th>Contents</th>
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
          </tr>
        ))}
      </tbody>
    </table>
  );
}

export function DispatchQueueRoute() {
  const { state, retry } = usePlannedOrders();

  return (
    <section>
      <h1>Dispatch queue</h1>
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
        <PlannedOrdersTable orders={state.orders} />
      )}
    </section>
  );
}
