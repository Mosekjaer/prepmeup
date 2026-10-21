import type { CustomerResponse } from '@prepmeup/api-client';
import { useFocusEffect, useRouter } from 'expo-router';
import { useCallback, useState } from 'react';
import { ActivityIndicator, Button, StyleSheet, Text, View } from 'react-native';

import { isNotFound, userClient } from '../lib/api';
import { useAuth } from '../lib/auth';

export default function HomeScreen() {
  const router = useRouter();
  const { token, signIn, signOut } = useAuth();
  const [customer, setCustomer] = useState<CustomerResponse>();
  const [error, setError] = useState(false);

  const loadCustomer = useCallback(() => {
    setCustomer(undefined);
    setError(false);
    if (!token) {
      return;
    }

    userClient
      .getCustomer()
      .then(setCustomer)
      .catch((reason: unknown) => (isNotFound(reason) ? router.push('/onboarding') : setError(true)));
  }, [token, router]);

  useFocusEffect(loadCustomer);

  return (
    <View style={styles.container}>
      <Text style={styles.title}>PrepMeUp</Text>
      {!token ? (
        <Button title="Sign in" onPress={signIn} />
      ) : (
        <>
          {error ? (
            <>
              <Text>Could not load your profile.</Text>
              <Button title="Try again" onPress={loadCustomer} />
            </>
          ) : customer ? (
            <>
              <Text>Welcome, {customer.name}</Text>
              <Text>{customer.address}</Text>
            </>
          ) : (
            <ActivityIndicator />
          )}
          <Button title="Sign out" onPress={signOut} />
        </>
      )}
      <Button title="Go to second screen" onPress={() => router.push('/second')} />
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    alignItems: 'center',
    justifyContent: 'center',
    padding: 24,
    gap: 12,
  },
  title: {
    fontSize: 28,
    fontWeight: '600',
  },
});
