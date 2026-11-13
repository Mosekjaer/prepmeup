import { apiConfig, ItemsClient, PackageTypesClient, SubscriptionClient, UserClient } from '@prepmeup/api-client';

/**
 * Base address of the PrepMeUp API. Set EXPO_PUBLIC_API_URL in the environment; on a physical
 * device localhost is the phone, not the dev machine, so use the machine's LAN address.
 */
export const apiBaseUrl = process.env.EXPO_PUBLIC_API_URL ?? 'http://localhost:5001';

export const itemsClient = new ItemsClient(apiBaseUrl, apiConfig);
export const packageTypesClient = new PackageTypesClient(apiBaseUrl, apiConfig);
export const subscriptionClient = new SubscriptionClient(apiBaseUrl, apiConfig);
export const userClient = new UserClient(apiBaseUrl, apiConfig);

export function hasStatus(error: unknown, status: number): boolean {
  return typeof error === 'object' && error !== null && 'status' in error && error.status === status;
}

export function isNotFound(error: unknown): boolean {
  return hasStatus(error, 404);
}
