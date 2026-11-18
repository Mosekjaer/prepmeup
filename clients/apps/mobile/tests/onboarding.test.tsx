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

function fillIn(field: string, value: string) {
  fireEvent.change(screen.getByPlaceholderText(field), { target: { value } });
}

function fillInAndSave(phone = '') {
  fillIn('Name', 'Malthe');
  fillIn('Phone (optional)', phone);
  fillIn('Street', 'Finlandsgade 22');
  fillIn('Postal code', '8200');
  fillIn('City', 'Aarhus N');
  fireEvent.click(screen.getByText('Save'));
}

it('saves the details and goes back', async () => {
  mocks.saveCustomer.mockResolvedValue(undefined);
  render(<OnboardingScreen />);

  fillInAndSave('12345678');

  await waitFor(() => expect(mocks.router.back).toHaveBeenCalled());
  expect(mocks.saveCustomer).toHaveBeenCalledWith(
    expect.objectContaining({
      name: 'Malthe',
      phone: '12345678',
      address: expect.objectContaining({ street: 'Finlandsgade 22', postalCode: '8200', city: 'Aarhus N' }),
    }),
  );
});

it('sends no phone when the field is left empty', async () => {
  mocks.saveCustomer.mockResolvedValue(undefined);
  render(<OnboardingScreen />);

  fillInAndSave();

  await waitFor(() => expect(mocks.saveCustomer).toHaveBeenCalled());
  const [request] = mocks.saveCustomer.mock.calls[0] ?? [];
  expect(request?.phone).toBeUndefined();
});

it('shows an error and stays when saving fails', async () => {
  mocks.saveCustomer.mockRejectedValue({ status: 400 });
  render(<OnboardingScreen />);

  fillInAndSave();

  expect(await screen.findByText(/Could not save your details/)).toBeTruthy();
  expect(mocks.router.back).not.toHaveBeenCalled();
});
