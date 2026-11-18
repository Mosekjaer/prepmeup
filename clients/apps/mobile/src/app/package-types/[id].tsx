import type { PackageTypeResponse } from '@prepmeup/api-client';
import { useLocalSearchParams } from 'expo-router';
import { useCallback, useEffect, useState } from 'react';
import { ActivityIndicator, Button, StyleSheet, Text, View } from 'react-native';

import { packageTypesClient } from '../../lib/api';

type State =
  | { kind: 'loading' }
  | { kind: 'error' }
  | { kind: 'unavailable' }
  | { kind: 'loaded'; packageType: PackageTypeResponse };

export default function PackageTypeScreen() {
  const { id } = useLocalSearchParams<{ id: string }>();
  const [state, setState] = useState<State>({ kind: 'loading' });

  const load = useCallback(() => {
    setState({ kind: 'loading' });
    packageTypesClient
      .getActivePackageTypes()
      .then((packageTypes) => {
        const packageType = packageTypes.find((candidate) => candidate.id === id);
        setState(packageType ? { kind: 'loaded', packageType } : { kind: 'unavailable' });
      })
      .catch(() => setState({ kind: 'error' }));
  }, [id]);

  useEffect(load, [load]);

  if (state.kind === 'loading') {
    return <ActivityIndicator />;
  }

  if (state.kind === 'error') {
    return (
      <View style={styles.container}>
        <Text>Could not load the package type.</Text>
        <Button title="Try again" onPress={load} />
      </View>
    );
  }

  if (state.kind === 'unavailable') {
    return (
      <View style={styles.container}>
        <Text>This package type is no longer available.</Text>
      </View>
    );
  }

  const { packageType } = state;

  return (
    <View style={styles.container}>
      <Text style={styles.title}>{packageType.name}</Text>
      {packageType.contents.map((content) => (
        <Text key={content.itemId}>
          {content.itemName} × {content.quantity}
        </Text>
      ))}
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    padding: 16,
  },
  title: {
    fontSize: 18,
    fontWeight: '600',
    textAlign: 'center',
    marginBottom: 12,
  },
});
