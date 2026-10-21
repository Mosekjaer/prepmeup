import { Stack } from 'expo-router';
import { StatusBar } from 'expo-status-bar';

import { AuthProvider } from '../lib/auth';

export default function RootLayout() {
  return (
    <AuthProvider>
      <Stack screenOptions={{ headerTitle: 'PrepMeUp' }} />
      <StatusBar style="auto" />
    </AuthProvider>
  );
}
