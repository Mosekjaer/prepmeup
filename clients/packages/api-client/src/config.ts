type AccessTokenProvider = () => string | undefined | Promise<string | undefined>;

let accessTokenProvider: AccessTokenProvider = () => undefined;

/**
 * Lets each app decide where its token lives - SecureStore on the phone, memory in the
 * browser - without the generated client knowing anything about either.
 */
export function setAccessTokenProvider(provider: AccessTokenProvider): void {
  accessTokenProvider = provider;
}

type UnauthorizedHandler = {
  renew: () => Promise<string | undefined>;
  expire: () => void;
};

let unauthorizedHandler: UnauthorizedHandler | undefined;

export function setUnauthorizedHandler(handler: UnauthorizedHandler | undefined): void {
  unauthorizedHandler = handler;
}

function send(url: RequestInfo, init: RequestInit | undefined, token: string | undefined): Promise<Response> {
  const headers = new Headers(init?.headers);

  if (token) {
    headers.set('Authorization', `Bearer ${token}`);
  }

  return globalThis.fetch(url, { ...init, headers });
}

export const apiConfig = {
  /** Injected by the generated NSwag client as its fetch implementation. */
  async fetch(url: RequestInfo, init?: RequestInit): Promise<Response> {
    const response = await send(url, init, await accessTokenProvider());

    if (response.status !== 401 || !unauthorizedHandler) {
      return response;
    }

    const retry = await send(url, init, await unauthorizedHandler.renew());

    if (retry.status === 401) {
      unauthorizedHandler.expire();
    }

    return retry;
  },
};
