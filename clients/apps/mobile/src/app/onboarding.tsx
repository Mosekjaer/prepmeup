import { AddressContract, SaveCustomerRequest } from '@prepmeup/api-client';
import { useRouter } from 'expo-router';
import { useState } from 'react';
import { Button, StyleSheet, Text, TextInput, View } from 'react-native';

import { userClient } from '../lib/api';

export default function OnboardingScreen() {
  const router = useRouter();
  const [name, setName] = useState('');
  const [phone, setPhone] = useState('');
  const [street, setStreet] = useState('');
  const [postalCode, setPostalCode] = useState('');
  const [city, setCity] = useState('');
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState(false);

  function save() {
    setSaving(true);
    setError(false);
    const request = new SaveCustomerRequest({
      name,
      phone: phone || undefined,
      address: new AddressContract({ street, postalCode, city }),
    });
    userClient
      .saveCustomer(request)
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
      <TextInput
        style={styles.input}
        placeholder="Phone (optional)"
        keyboardType="phone-pad"
        value={phone}
        onChangeText={setPhone}
      />
      <TextInput style={styles.input} placeholder="Street" value={street} onChangeText={setStreet} />
      <TextInput
        style={styles.input}
        placeholder="Postal code"
        keyboardType="number-pad"
        maxLength={4}
        value={postalCode}
        onChangeText={setPostalCode}
      />
      <TextInput style={styles.input} placeholder="City" value={city} onChangeText={setCity} />
      {error && (
        <Text>
          Could not save your details. Check that your postal code has 4 digits and your phone number has 8 digits.
        </Text>
      )}
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
