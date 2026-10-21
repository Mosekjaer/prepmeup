import { SaveCustomerRequest } from '@prepmeup/api-client';
import { useRouter } from 'expo-router';
import { useState } from 'react';
import { Button, StyleSheet, Text, TextInput, View } from 'react-native';

import { userClient } from '../lib/api';

export default function OnboardingScreen() {
  const router = useRouter();
  const [name, setName] = useState('');
  const [address, setAddress] = useState('');
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState(false);

  function save() {
    setSaving(true);
    setError(false);
    userClient
      .saveCustomer(new SaveCustomerRequest({ name, address }))
      .then(() => router.back())
      .catch(() => {
        setError(true);
        setSaving(false);
      });
  }

  return (
    <View style={styles.container}>
      <Text style={styles.title}>Tell us about you</Text>
      <TextInput style={styles.input} placeholder="Name" value={name} onChangeText={setName} />
      <TextInput style={styles.input} placeholder="Address" value={address} onChangeText={setAddress} />
      {error && <Text>Could not save your details. Check your name and address.</Text>}
      <Button title="Save" onPress={save} disabled={saving} />
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    justifyContent: 'center',
    padding: 24,
    gap: 12,
  },
  title: {
    fontSize: 22,
    fontWeight: '600',
  },
  input: {
    borderWidth: 1,
    borderColor: '#ccc',
    borderRadius: 6,
    padding: 10,
  },
});
