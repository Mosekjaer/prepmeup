import type { SubscriptionResponse } from '@prepmeup/api-client';
import { useRouter } from 'expo-router';
import { useCallback, useEffect, useState } from 'react';
import { ActivityIndicator, Button, Text, View } from 'react-native';

import { isNotFound, subscriptionClient } from '../lib/api';
import { formatDate } from '../lib/format';

type State =
  | { kind: 'loading' }
  | { kind: 'none' }
  | { kind: 'error' }
  | { kind: 'loaded'; subscription: SubscriptionResponse };

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

  return (
    <View>
      <Text>Active since {formatDate(state.subscription.startedAt)}</Text>
    </View>
  );
}
