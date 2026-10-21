import { useRouter } from 'expo-router';
import { Button, StyleSheet, Text, View } from 'react-native';

import { useAuth } from '../lib/auth';

export default function HomeScreen() {
  const router = useRouter();
  const { token, signIn, signOut } = useAuth();
  return (
    <View style={styles.container}>
      <Text style={styles.title}>PrepMeUp</Text>
      {token ? <Button title="Sign out" onPress={signOut} /> : <Button title="Sign in" onPress={signIn} />}
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
