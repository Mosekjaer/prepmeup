/**
 * Base address of the PrepMeUp API. Set EXPO_PUBLIC_API_URL in the environment; on a physical
 * device localhost is the phone, not the dev machine, so use the machine's LAN address.
 */
export const apiBaseUrl = process.env.EXPO_PUBLIC_API_URL ?? 'http://localhost:5001';
