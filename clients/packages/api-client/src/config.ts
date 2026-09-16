type AccessTokenProvider = () => string | undefined | Promise<string | undefined>;

let accessTokenProvider: AccessTokenProvider = () => undefined;

/**
 * Lets each app decide where its token lives - SecureStore on the phone, memory in the
 * browser - without the generated client knowing anything about either.
 */
export function setAccessTokenProvider(provider: AccessTokenProvider): void {
  accessTokenProvider = provider;
}

export const apiConfig = {
  /** Injected by the generated NSwag client as its fetch implementation. */
  async fetch(url: RequestInfo, init?: RequestInit): Promise<Response> {
    const token = await accessTokenProvider();
    const headers = new Headers(init?.headers);

    if (token) {
      headers.set('Authorization', `Bearer ${token}`);
    }

    return globalThis.fetch(url, { ...init, headers });
  },
};
