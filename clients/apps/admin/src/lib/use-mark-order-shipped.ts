import { useState } from 'react';
import { adminClient } from './api';

export type MarkShippedState =
    | { status: 'idle' }
    | { status: 'pending'; orderId: string }
    | { status: 'error'; orderId: string; message: string };

function statusOf(error: unknown): number | undefined {
    if (typeof error === 'object' && error !== null && 'status' in error && typeof error.status === 'number') {
        return error.status;
    }
    return undefined;
}


export function useMarkOrderShipped(onDone: () => void): {
    state: MarkShippedState;
    markShipped: (orderId: string) => void;
} {
    const [state, setState] = useState<MarkShippedState>({ status: 'idle' });

    function markShipped(orderId: string) {
        setState({ status: 'pending', orderId });
        void adminClient.markOrderShipped(orderId).then(() => {
            setState({ status: 'idle' });
            onDone();
        }).catch((error: unknown) => {
            if (statusOf(error) === 409) {
                setState({ status: 'error', orderId, message: 'This order has already been shipped.' });
                onDone();
                return;
            }

            if (statusOf(error) === 404) {
                setState({ status: 'error', orderId, message: 'This order could not be found.' });
                onDone();
                return;
            }

            setState({
                status: 'error',
                orderId,
                message: 'We could not mark the order as shipped. Please try again.',
            });
        });

    }
    return { state, markShipped };
}
