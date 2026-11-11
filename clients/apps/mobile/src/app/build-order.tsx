import type { PackageTypeResponse } from '@prepmeup/api-client';
import { useRouter } from 'expo-router';
import { useCallback, useEffect, useState } from 'react';
import { ActivityIndicator, Button, ScrollView, StyleSheet, Text, View } from 'react-native';

import { packageTypesClient } from '../lib/api';

export default function BuildOrderScreen() {
  const router = useRouter();
  const [packageTypes, setPackageTypes] = useState<PackageTypeResponse[]>();
  const [error, setError] = useState(false);
  const [quantities, setQuantities] = useState<Record<string, number>>({});

  const load = useCallback(() => {
    setPackageTypes(undefined);
    setError(false);
    packageTypesClient
      .getActivePackageTypes()
      .then(setPackageTypes)
      .catch(() => setError(true));
  }, []);

  useEffect(load, [load]);

  const change = (id: string, delta: number) =>
    setQuantities((current) => ({ ...current, [id]: Math.max(0, (current[id] ?? 0) + delta) }));

  if (error) {
    return (
      <View style={styles.message}>
        <Text>Could not load the package types.</Text>
        <Button title="Try again" onPress={load} />
      </View>
    );
  }

  if (!packageTypes) {
    return <ActivityIndicator />;
  }

  if (packageTypes.length === 0) {
    return (
      <View style={styles.message}>
        <Text>There are no package types to choose from.</Text>
      </View>
    );
  }

  const totalQuantity = Object.values(quantities).reduce((sum, quantity) => sum + quantity, 0);

  return (
    <View style={styles.container}>
      <ScrollView>
        {packageTypes.map((packageType) => (
          <View key={packageType.id} style={styles.row}>
            <View style={styles.details}>
              <Text style={styles.name}>{packageType.name}</Text>
              <View style={styles.contentsButton}>
                <Button
                  title="Contents"
                  accessibilityLabel={`Show contents of ${packageType.name}`}
                  onPress={() => router.push({ pathname: '/package-types/[id]', params: { id: packageType.id } })}
                />
              </View>
            </View>
            <View style={styles.stepper}>
              <View style={styles.stepperButtons}>
                <Button title="+" accessibilityLabel={`Add ${packageType.name}`} onPress={() => change(packageType.id, 1)} />
                <Button
                  title="−"
                  accessibilityLabel={`Remove ${packageType.name}`}
                  onPress={() => change(packageType.id, -1)}
                />
              </View>
              <Text style={styles.quantity} accessibilityLabel={`${packageType.name} quantity`}>
                {quantities[packageType.id] ?? 0}
              </Text>
            </View>
          </View>
        ))}
      </ScrollView>
      <View style={styles.footer}>
        <Button title="Continue" disabled={totalQuantity === 0} onPress={() => {}} />
      </View>
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
  },
  message: {
    padding: 16,
  },
  row: {
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'space-between',
    padding: 16,
  },
  details: {
    flex: 1,
    gap: 8,
  },
  name: {
    fontSize: 18,
    fontWeight: '600',
  },
  contentsButton: {
    alignSelf: 'flex-start',
  },
  stepper: {
    flexDirection: 'row',
    alignItems: 'center',
  },
  stepperButtons: {
    width: 56,
    gap: 8,
  },
  quantity: {
    minWidth: 48,
    textAlign: 'center',
    fontSize: 24,
    fontWeight: '600',
  },
  footer: {
    padding: 16,
  },
});
