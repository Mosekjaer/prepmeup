import { ItemResponse } from '@prepmeup/api-client';
import { render, screen } from '@testing-library/react';
import { expect, it, vi } from 'vitest';

import SecondScreen from '../src/app/second';
import { itemsClient } from '../src/lib/api';

vi.mock('../src/lib/api', () => ({
  itemsClient: { getItems: vi.fn() },
}));

it('shows the items returned by the API', async () => {
  vi.mocked(itemsClient.getItems).mockResolvedValue([
    new ItemResponse({ name: 'Water' }),
    new ItemResponse({ name: 'Candles' }),
  ]);

  render(<SecondScreen />);

  expect(await screen.findByText('Water')).toBeTruthy();
  expect(screen.getByText('Candles')).toBeTruthy();
});

it('shows an error when the API call fails', async () => {
  vi.mocked(itemsClient.getItems).mockRejectedValue(new Error('Unauthorized'));

  render(<SecondScreen />);

  expect(await screen.findByText('Could not load items.')).toBeTruthy();
});
