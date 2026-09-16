import { describe, expect, it } from 'vitest';

import { apiBaseUrl } from './api';

describe('apiBaseUrl', () => {
  it('falls back to the local API when no environment variable is set', () => {
    expect(apiBaseUrl).toMatch(/^https?:\/\//);
  });
});
