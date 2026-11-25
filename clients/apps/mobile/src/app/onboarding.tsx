import { AddressContract, SaveCustomerRequest, ValidationProblemDetails } from '@prepmeup/api-client';
import { useRouter } from 'expo-router';
import { useState } from 'react';
import { Button, StyleSheet, Text, TextInput, View } from 'react-native';

import { customerClient } from '../lib/api';

const fieldHints: Record<string, string> = {
  Name: 'Enter your name.',
  Phone: 'Enter a phone number with 8 digits, or leave it empty.',
  'Address.Street': 'Enter your street.',
  'Address.PostalCode': 'Enter a postal code with 4 digits.',
  'Address.City': 'Enter your city.',
};

function saveErrorMessage(reason: unknown): string {
  const fields = reason instanceof ValidationProblemDetails ? Object.keys(reason.errors ?? {}) : [];
  const hints = fields.map((field) => fieldHints[field]).filter((hint) => hint !== undefined);
  return hints.length > 0
    ? `We could not save your details. ${hints.join(' ')}`
    : 'We could not save your details. Check your internet connection and try again.';
}

export default function OnboardingScreen() {
  const router = useRouter();
  const [name, setName] = useState('');
  const [phone, setPhone] = useState('');
  const [street, setStreet] = useState('');
  const [postalCode, setPostalCode] = useState('');
  const [city, setCity] = useState('');
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState<string>();

  function save() {
    setSaving(true);
    setError(undefined);
    const request = new SaveCustomerRequest({
      name,
      phone: phone || undefined,
      address: new AddressContract({ street, postalCode, city }),
    });
    customerClient
      .saveCustomer(request)
      .then(() => router.back())
      .catch((reason: unknown) => {
        setError(saveErrorMessage(reason));
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
      {error && <Text>{error}</Text>}
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
