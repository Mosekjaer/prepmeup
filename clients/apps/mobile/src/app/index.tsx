import { StyleSheet, Text, View } from 'react-native';

export default function HomeScreen() {
  return (
    <View style={styles.container}>
      <Text style={styles.title}>PrepMeUp</Text>
      <Text style={styles.body}>
        Scaffold only. Add routes as files under src/app/ and call the API through
        @prepmeup/api-client.
      </Text>
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
  body: {
    fontSize: 15,
    textAlign: 'center',
    lineHeight: 22,
  },
});
