import { useEffect, useState } from 'react';
import { ActivityIndicator, StyleSheet, Text, View } from 'react-native';

import type { ItemResponse } from '@prepmeup/api-client';

import { itemsClient } from '../lib/api';

export default function SecondScreen() {
  const [items, setItems] = useState<ItemResponse[]>();
  const [error, setError] = useState(false);

  useEffect(() => {
    itemsClient
      .getItems()
      .then(setItems)
      .catch(() => setError(true));
  }, []);

  return (
    <View style={styles.container}>
      <Text style={styles.title}>Second screen</Text>
      {error ? (
        <Text>Could not load items.</Text>
      ) : !items ? (
        <ActivityIndicator />
      ) : (
        items.map((item) => <Text key={item.name}>{item.name}</Text>)
      )}
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    alignItems: 'center',
    justifyContent: 'center',
    padding: 24,
    gap: 8,
  },
  title: {
    fontSize: 22,
    fontWeight: '600',
  },
});
