import type { SubscriptionResponse } from '@prepmeup/api-client';
import { useRouter } from 'expo-router';
import { useCallback, useEffect, useState } from 'react';
import { ActivityIndicator, Button, Text, View } from 'react-native';

import { isNotFound, subscriptionClient } from '../lib/api';

type State =
  | { kind: 'loading' }
  | { kind: 'none' }
  | { kind: 'error' }
  | { kind: 'loaded'; subscription: SubscriptionResponse };

function formatDate(value: string): string {
  return new Date(value).toLocaleDateString('en-GB', {
    day: 'numeric',
    month: 'short',
    year: 'numeric',
    timeZone: 'UTC',
  });
}

export function SubscriptionSummary() {
  const router = useRouter();
  const [state, setState] = useState<State>({ kind: 'loading' });

  const load = useCallback(() => {
    setState({ kind: 'loading' });
    subscriptionClient
      .getSubscription()
      .then((subscription) => setState({ kind: 'loaded', subscription }))
      .catch((reason: unknown) =>
        setState(isNotFound(reason) ? { kind: 'none' } : { kind: 'error' }),
      );
  }, []);

  useEffect(load, [load]);

  if (state.kind === 'loading') {
    return <ActivityIndicator />;
  }

  if (state.kind === 'error') {
    return (
      <View>
        <Text>Could not load your subscription.</Text>
        <Button title="Try again" onPress={load} />
      </View>
    );
  }

  if (state.kind === 'none') {
    return (
      <View>
        <Text>You do not have a subscription yet.</Text>
        <Button title="Subscribe" onPress={() => router.push('/build-order')} />
      </View>
    );
  }

  const { subscription } = state;
  const nextOrder = subscription.orders
    .filter((order) => !order.sentAt)
    .sort((a, b) => new Date(a.plannedAt).getTime() - new Date(b.plannedAt).getTime())[0];

  return (
    <View>
      <Text>Active since {formatDate(subscription.startedAt)}</Text>
      {nextOrder ? (
        <>
          <Text>Planned for {formatDate(nextOrder.plannedAt)}</Text>
          <Text>Status: Planned</Text>
        </>
      ) : (
        <Text>No order planned.</Text>
      )}
    </View>
  );
}
