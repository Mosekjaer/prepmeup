import { render, screen } from '@testing-library/react';
import { expect, it, vi } from 'vitest';

import SecondScreen from '../src/app/second';

vi.mock('../src/lib/api', () => ({
  itemsClient: { getItems: () => Promise.resolve([{ name: 'Water' }]) },
}));

it('shows the items returned by the API', async () => {
  render(<SecondScreen />);

  expect(await screen.findByText('Water')).toBeTruthy();
});
