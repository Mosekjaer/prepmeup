import type { CustomerResponse } from '@prepmeup/api-client';
import { useFocusEffect, useRouter } from 'expo-router';
import { useCallback, useState } from 'react';
import { ActivityIndicator, Button, StyleSheet, Text, View } from 'react-native';

import { isNotFound, customerClient } from '../../lib/api';

type State =
  | { kind: 'loading' }
  | { kind: 'none' }
  | { kind: 'error' }
  | { kind: 'loaded'; customer: CustomerResponse };

export default function ProfileScreen() {
  const router = useRouter();
  const [state, setState] = useState<State>({ kind: 'loading' });

  const load = useCallback(() => {
    setState({ kind: 'loading' });
    customerClient
      .getCustomer()
      .then((customer) => setState({ kind: 'loaded', customer }))
      .catch((reason: unknown) => setState({ kind: isNotFound(reason) ? 'none' : 'error' }));
  }, []);

  useFocusEffect(load);

  if (state.kind === 'loading') {
    return <ActivityIndicator />;
  }

  if (state.kind === 'error') {
    return (
      <View style={styles.container}>
        <Text>Could not load your profile.</Text>
        <Button title="Try again" onPress={load} />
      </View>
    );
  }

  if (state.kind === 'none') {
    return (
      <View style={styles.container}>
        <Text>You have not filled in your details yet.</Text>
        <Button title="Fill in details" onPress={() => router.push('/onboarding')} />
      </View>
    );
  }

  const { customer } = state;

  return (
    <View style={styles.container}>
      <Text>{customer.name}</Text>
      <Text>{customer.email}</Text>
      {customer.phone ? <Text>{customer.phone}</Text> : null}
      <Text>{customer.address.street}</Text>
      <Text>
        {customer.address.postalCode} {customer.address.city}
      </Text>
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    padding: 16,
  },
});
