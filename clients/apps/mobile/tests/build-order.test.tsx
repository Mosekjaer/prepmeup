import { fireEvent, render, screen } from '@testing-library/react';
import { beforeEach, expect, it, vi } from 'vitest';

import BuildOrderScreen from '../src/app/build-order';

const mocks = vi.hoisted(() => ({
  router: { push: vi.fn() },
  getActivePackageTypes: vi.fn(),
}));

vi.mock('expo-router', () => ({
  useRouter: () => mocks.router,
}));

vi.mock('../src/lib/api', async (importOriginal) => ({
  ...(await importOriginal<typeof import('../src/lib/api')>()),
  packageTypesClient: { getActivePackageTypes: mocks.getActivePackageTypes },
}));

beforeEach(() => {
  mocks.router.push.mockReset();
  mocks.getActivePackageTypes.mockReset();
});

it('shows a loading indicator while the package types are loading', () => {
  mocks.getActivePackageTypes.mockReturnValue(new Promise(() => {}));

  render(<BuildOrderScreen />);

  expect(screen.getByRole('progressbar')).toBeTruthy();
});

it('says that there are no package types to choose from when the list is empty', async () => {
  mocks.getActivePackageTypes.mockResolvedValue([]);

  render(<BuildOrderScreen />);

  expect(await screen.findByText('There are no package types to choose from.')).toBeTruthy();
});

const adultType = { id: 'adult', name: 'Normal - Adult', contents: [] };
const childType = { id: 'child', name: 'Normal - Child', contents: [] };

const quantityOf = (name: string) => screen.getByLabelText(`${name} quantity`).textContent;
const add = (name: string) => fireEvent.click(screen.getByLabelText(`Add ${name}`));
const remove = (name: string) => fireEvent.click(screen.getByLabelText(`Remove ${name}`));

it('shows a counter for each package type that goes up and down but never below zero', async () => {
  mocks.getActivePackageTypes.mockResolvedValue([adultType, childType]);
  render(<BuildOrderScreen />);
  await screen.findByText('Normal - Adult');
  expect(quantityOf('Normal - Adult')).toBe('0');
  expect(quantityOf('Normal - Child')).toBe('0');

  add('Normal - Adult');
  add('Normal - Adult');
  remove('Normal - Adult');
  remove('Normal - Child');

  expect(quantityOf('Normal - Adult')).toBe('1');
  expect(quantityOf('Normal - Child')).toBe('0');
});

const isDisabled = (element: HTMLElement) => element.getAttribute('aria-disabled') === 'true';

it('only enables Continue when at least one package is chosen', async () => {
  mocks.getActivePackageTypes.mockResolvedValue([adultType, childType]);
  render(<BuildOrderScreen />);
  await screen.findByText('Normal - Adult');
  expect(isDisabled(screen.getByRole('button', { name: 'Continue' }))).toBe(true);

  add('Normal - Child');

  expect(isDisabled(screen.getByRole('button', { name: 'Continue' }))).toBe(false);

  remove('Normal - Child');

  expect(isDisabled(screen.getByRole('button', { name: 'Continue' }))).toBe(true);
});

it('opens the contents of a package type', async () => {
  mocks.getActivePackageTypes.mockResolvedValue([adultType, childType]);
  render(<BuildOrderScreen />);
  await screen.findByText('Normal - Adult');

  fireEvent.click(screen.getByLabelText('Show contents of Normal - Child'));

  expect(mocks.router.push).toHaveBeenCalledWith({ pathname: '/package-types/[id]', params: { id: 'child' } });
});

it('shows an error that can be retried when loading fails', async () => {
  mocks.getActivePackageTypes.mockRejectedValueOnce({ status: 500 });
  mocks.getActivePackageTypes.mockResolvedValueOnce([adultType]);
  render(<BuildOrderScreen />);
  expect(await screen.findByText('Could not load the package types.')).toBeTruthy();

  fireEvent.click(screen.getByText('Try again'));

  expect(await screen.findByText('Normal - Adult')).toBeTruthy();
  expect(mocks.getActivePackageTypes).toHaveBeenCalledTimes(2);
});

it('keeps a separate counter for each of ten package types', async () => {
  const names = Array.from({ length: 10 }, (_, index) => `Type ${index + 1}`);
  mocks.getActivePackageTypes.mockResolvedValue(names.map((name, index) => ({ id: `type-${index + 1}`, name, contents: [] })));
  render(<BuildOrderScreen />);
  await screen.findByText('Type 1');

  add('Type 3');
  add('Type 3');
  add('Type 7');

  expect(names.map(quantityOf)).toEqual(['0', '0', '2', '0', '0', '0', '1', '0', '0', '0']);
});

it('opens the confirmation with only the chosen package types when Continue is pressed', async () => {
  mocks.getActivePackageTypes.mockResolvedValue([adultType, childType]);
  render(<BuildOrderScreen />);
  await screen.findByText('Normal - Adult');
  add('Normal - Adult');
  add('Normal - Adult');

  fireEvent.click(screen.getByRole('button', { name: 'Continue' }));

  expect(mocks.router.push).toHaveBeenCalledWith({
    pathname: '/confirm-order',
    params: { lines: JSON.stringify([{ packageTypeId: 'adult', name: 'Normal - Adult', quantity: 2 }]) },
  });
});
