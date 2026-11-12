import { SubscribeLineRequest, SubscribeRequest } from '@prepmeup/api-client';
import { useLocalSearchParams, useRouter } from 'expo-router';
import { useState } from 'react';
import { ActivityIndicator, Button, StyleSheet, Text, View } from 'react-native';

import { hasStatus, subscriptionClient } from '../lib/api';

type ChosenLine = { packageTypeId: string; name: string; quantity: number };

function parseLines(value: string | undefined): ChosenLine[] {
  try {
    const parsed: unknown = JSON.parse(value ?? '[]');
    return Array.isArray(parsed) ? parsed : [];
  } catch {
    return [];
  }
}

type State = { kind: 'idle' } | { kind: 'submitting' } | { kind: 'conflict' } | { kind: 'unavailable' } | { kind: 'failed' };

export default function ConfirmOrderScreen() {
  const router = useRouter();
  const { lines } = useLocalSearchParams<{ lines?: string }>();
  const chosenLines = parseLines(lines);
  const [state, setState] = useState<State>({ kind: 'idle' });

  const confirm = () => {
    setState({ kind: 'submitting' });
    subscriptionClient
      .subscribe(
        new SubscribeRequest({
          lines: chosenLines.map((line) => new SubscribeLineRequest({ packageTypeId: line.packageTypeId, quantity: line.quantity })),
        }),
      )
      .then(() => router.dismissTo('/'))
      .catch((reason: unknown) => {
        if (hasStatus(reason, 409)) {
          setState({ kind: 'conflict' });
        } else if (hasStatus(reason, 400)) {
          setState({ kind: 'unavailable' });
        } else {
          setState({ kind: 'failed' });
        }
      });
  };

  if (chosenLines.length === 0) {
    return (
      <View style={styles.container}>
        <Text>There is nothing in your order.</Text>
      </View>
    );
  }

  if (state.kind === 'conflict') {
    return (
      <View style={styles.container}>
        <Text>You already have a subscription.</Text>
        <Button title="Go to Home" onPress={() => router.dismissTo('/')} />
      </View>
    );
  }

  if (state.kind === 'failed') {
    return (
      <View style={styles.container}>
        <Text>Could not create your subscription.</Text>
        <Button title="Try again" onPress={confirm} />
      </View>
    );
  }

  if (state.kind === 'unavailable') {
    return (
      <View style={styles.container}>
        <Text>One of the package types is no longer available.</Text>
        <Button title="Back to order" onPress={() => router.back()} />
      </View>
    );
  }

  return (
    <View style={styles.container}>
      {chosenLines.map((line) => (
        <Text key={line.packageTypeId}>
          {line.name} × {line.quantity}
        </Text>
      ))}
      {state.kind === 'submitting' && <ActivityIndicator />}
      <Button title="Confirm and subscribe" disabled={state.kind === 'submitting'} onPress={confirm} />
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    padding: 16,
  },
});
