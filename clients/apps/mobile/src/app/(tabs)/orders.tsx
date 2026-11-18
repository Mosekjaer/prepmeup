import type { SubscriptionResponse } from '@prepmeup/api-client';
import { useFocusEffect } from 'expo-router';
import { useCallback, useState } from 'react';
import { ActivityIndicator, Button, StyleSheet, Text, View } from 'react-native';

import { isNotFound, subscriptionClient } from '../../lib/api';
import { formatDate } from '../../lib/format';

type State =
  | { kind: 'loading' }
  | { kind: 'none' }
  | { kind: 'error' }
  | { kind: 'loaded'; subscription: SubscriptionResponse };

export default function OrdersScreen() {
  const [state, setState] = useState<State>({ kind: 'loading' });

  const load = useCallback(() => {
    setState({ kind: 'loading' });
    subscriptionClient
      .getSubscription()
      .then((subscription) => setState({ kind: 'loaded', subscription }))
      .catch((reason: unknown) => setState({ kind: isNotFound(reason) ? 'none' : 'error' }));
  }, []);

  useFocusEffect(load);

  if (state.kind === 'loading') {
    return <ActivityIndicator />;
  }

  if (state.kind === 'error') {
    return (
      <View style={styles.container}>
        <Text>Could not load your orders.</Text>
        <Button title="Try again" onPress={load} />
      </View>
    );
  }

  const nextOrder =
    state.kind === 'loaded'
      ? state.subscription.orders
          .filter((order) => !order.sentAt)
          .sort((a, b) => new Date(a.plannedAt).getTime() - new Date(b.plannedAt).getTime())[0]
      : undefined;

  if (!nextOrder) {
    return (
      <View style={styles.container}>
        <Text>No order planned.</Text>
      </View>
    );
  }

  return (
    <View style={styles.container}>
      <Text>Planned for {formatDate(nextOrder.plannedAt)}</Text>
      <Text>Status: Planned</Text>
      {nextOrder.lines.map((line) => (
        <Text key={line.packageTypeId}>
          {line.packageTypeName} × {line.quantity}
        </Text>
      ))}
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    padding: 16,
  },
});
