import { fireEvent, render, screen } from '@testing-library/react';
import { beforeEach, expect, it, vi } from 'vitest';

import PackageTypeScreen from '../src/app/package-types/[id]';

const mocks = vi.hoisted(() => ({
  params: { id: 'adult' } as { id: string },
  getActivePackageTypes: vi.fn(),
}));

vi.mock('expo-router', () => ({
  useLocalSearchParams: () => mocks.params,
}));

vi.mock('../src/lib/api', async (importOriginal) => ({
  ...(await importOriginal<typeof import('../src/lib/api')>()),
  packageTypesClient: { getActivePackageTypes: mocks.getActivePackageTypes },
}));

beforeEach(() => {
  mocks.params = { id: 'adult' };
  mocks.getActivePackageTypes.mockReset();
});

it('shows a loading indicator while the package type is loading', () => {
  mocks.getActivePackageTypes.mockReturnValue(new Promise(() => {}));

  render(<PackageTypeScreen />);

  expect(screen.getByRole('progressbar')).toBeTruthy();
});

const adultType = {
  id: 'adult',
  name: 'Normal - Adult',
  contents: [
    { itemId: 'water', itemName: 'Drinking water 1.5 L', itemType: 'Food', quantity: 6 },
    { itemId: 'torch', itemName: 'Torch', itemType: 'Equipment', quantity: 1 },
  ],
};

it('shows the name and the contents of the package type', async () => {
  mocks.getActivePackageTypes.mockResolvedValue([
    { id: 'child', name: 'Normal - Child', contents: [] },
    adultType,
  ]);

  render(<PackageTypeScreen />);

  expect(await screen.findByText('Normal - Adult')).toBeTruthy();
  expect(screen.getByText('Drinking water 1.5 L × 6')).toBeTruthy();
  expect(screen.getByText('Torch × 1')).toBeTruthy();
  expect(screen.queryByText('Normal - Child')).toBeNull();
});

it('says that the package type is no longer available when it is not active', async () => {
  mocks.params = { id: 'replaced' };
  mocks.getActivePackageTypes.mockResolvedValue([adultType]);

  render(<PackageTypeScreen />);

  expect(await screen.findByText('This package type is no longer available.')).toBeTruthy();
});

it('shows an error that can be retried when loading fails', async () => {
  mocks.getActivePackageTypes.mockRejectedValueOnce({ status: 500 });
  mocks.getActivePackageTypes.mockResolvedValueOnce([adultType]);
  render(<PackageTypeScreen />);
  expect(await screen.findByText('Could not load the package type.')).toBeTruthy();

  fireEvent.click(screen.getByText('Try again'));

  expect(await screen.findByText('Normal - Adult')).toBeTruthy();
  expect(mocks.getActivePackageTypes).toHaveBeenCalledTimes(2);
});
