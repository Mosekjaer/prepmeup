import { Tabs } from 'expo-router';

export default function TabsLayout() {
  return (
    <Tabs
      screenOptions={{
        headerTitleAlign: 'left',
        headerStyle: { backgroundColor: '#E6E6E6' },
        headerTitleStyle: { color: '#333333', fontSize: 16, fontWeight: '700' },
        tabBarStyle: { height: 56, backgroundColor: '#EEEEEE', borderTopColor: '#BBBBBB' },
        tabBarItemStyle: { borderLeftWidth: 1, borderColor: '#BBBBBB' },
        tabBarActiveBackgroundColor: '#D0D0D0',
        tabBarInactiveBackgroundColor: '#EEEEEE',
        tabBarActiveTintColor: '#333333',
        tabBarInactiveTintColor: '#333333',
        tabBarLabelStyle: { fontSize: 13, fontWeight: '600' },
        tabBarIconStyle: { display: 'none' },
      }}
    >
      <Tabs.Screen name="index" options={{ title: 'Home' }} />
      <Tabs.Screen name="orders" options={{ title: 'Orders' }} />
      <Tabs.Screen name="profile" options={{ title: 'Profile' }} />
    </Tabs>
  );
}
