import { fireEvent, render, screen, waitFor } from '@testing-library/react';
import { beforeEach, expect, it, vi } from 'vitest';

import OnboardingScreen from '../src/app/onboarding';

const mocks = vi.hoisted(() => ({
  router: { push: vi.fn(), back: vi.fn() },
  saveCustomer: vi.fn(),
}));

vi.mock('expo-router', () => ({
  useRouter: () => mocks.router,
}));

vi.mock('../src/lib/api', () => ({
  userClient: { saveCustomer: mocks.saveCustomer },
}));

beforeEach(() => {
  mocks.router.back.mockReset();
  mocks.saveCustomer.mockReset();
});

function fillInAndSave() {
  fireEvent.change(screen.getByPlaceholderText('Name'), { target: { value: 'Malthe' } });
  fireEvent.change(screen.getByPlaceholderText('Address'), { target: { value: 'Finlandsgade 22' } });
  fireEvent.click(screen.getByText('Save'));
}

it('saves the details and goes back', async () => {
  mocks.saveCustomer.mockResolvedValue(undefined);
  render(<OnboardingScreen />);

  fillInAndSave();

  await waitFor(() => expect(mocks.router.back).toHaveBeenCalled());
  expect(mocks.saveCustomer).toHaveBeenCalledWith(
    expect.objectContaining({ name: 'Malthe', address: 'Finlandsgade 22' }),
  );
});

it('shows an error and stays when saving fails', async () => {
  mocks.saveCustomer.mockRejectedValue({ status: 400 });
  render(<OnboardingScreen />);

  fillInAndSave();

  expect(await screen.findByText('Could not save your details. Check your name and address.')).toBeTruthy();
  expect(mocks.router.back).not.toHaveBeenCalled();
});
