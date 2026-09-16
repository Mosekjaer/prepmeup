import { Stack } from 'expo-router';
import { StatusBar } from 'expo-status-bar';

export default function RootLayout() {
  return (
    <>
      <Stack screenOptions={{ headerTitle: 'PrepMeUp' }} />
      <StatusBar style="auto" />
    </>
  );
}
