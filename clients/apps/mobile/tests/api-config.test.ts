import { apiConfig, setAccessTokenProvider, setUnauthorizedHandler } from '@prepmeup/api-client';
import { beforeEach, expect, it, vi } from 'vitest';

const renew = vi.fn();
const expire = vi.fn();
const fetchMock = vi.fn();

function authorizationOfCall(index: number) {
  return new Headers(fetchMock.mock.calls[index]?.[1]?.headers).get('Authorization');
}

beforeEach(() => {
  renew.mockReset();
  expire.mockReset();
  fetchMock.mockReset();
  vi.stubGlobal('fetch', fetchMock);
  setAccessTokenProvider(() => 'old-token');
  setUnauthorizedHandler({ renew, expire });
});

it('renews the token and repeats the request once after a 401', async () => {
  renew.mockResolvedValue('new-token');
  fetchMock.mockResolvedValueOnce(new Response(null, { status: 401 })).mockResolvedValueOnce(new Response(null, { status: 200 }));

  const response = await apiConfig.fetch('http://api/items');

  expect(response.status).toBe(200);
  expect(fetchMock).toHaveBeenCalledTimes(2);
  expect(authorizationOfCall(0)).toBe('Bearer old-token');
  expect(authorizationOfCall(1)).toBe('Bearer new-token');
});

it('expires the session and stops after a second 401', async () => {
  renew.mockResolvedValue('new-token');
  fetchMock.mockResolvedValue(new Response(null, { status: 401 }));

  const response = await apiConfig.fetch('http://api/items');

  expect(response.status).toBe(401);
  expect(fetchMock).toHaveBeenCalledTimes(2);
  expect(renew).toHaveBeenCalledTimes(1);
  expect(expire).toHaveBeenCalledTimes(1);
});
