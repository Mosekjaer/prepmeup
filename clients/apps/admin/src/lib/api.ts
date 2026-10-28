import { AdminClient, apiConfig } from '@prepmeup/api-client';

import { apiUrl } from './config';

/** Base address of the PrepMeUp API. Set VITE_API_URL in .env.local. */
export const apiBaseUrl = apiUrl;

export const adminClient = new AdminClient(apiBaseUrl, apiConfig);
