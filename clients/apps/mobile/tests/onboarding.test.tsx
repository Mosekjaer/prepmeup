import { ValidationProblemDetails } from '@prepmeup/api-client';
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

it('asks to check the connection and stays when saving fails without validation errors', async () => {
  mocks.saveCustomer.mockRejectedValue(new Error('Network request failed'));
  render(<OnboardingScreen />);

  fillInAndSave();

  expect(
    await screen.findByText('We could not save your details. Check your internet connection and try again.'),
  ).toBeTruthy();
  expect(mocks.router.back).not.toHaveBeenCalled();
});

it('says that the name is missing when the API rejects an empty name', async () => {
  mocks.saveCustomer.mockRejectedValue(
    new ValidationProblemDetails({ status: 400, errors: { Name: ['The Name field is required.'] } }),
  );
  render(<OnboardingScreen />);

  fillInAndSave();

  expect(await screen.findByText('We could not save your details. Enter your name.')).toBeTruthy();
});

it('names every field the API rejects', async () => {
  mocks.saveCustomer.mockRejectedValue(
    new ValidationProblemDetails({
      status: 400,
      errors: {
        Phone: ['Phone must be 8 digits.'],
        'Address.PostalCode': ['Postal code must be 4 digits between 1000 and 9999.'],
      },
    }),
  );
  render(<OnboardingScreen />);

  fillInAndSave();

  expect(
    await screen.findByText(
      'We could not save your details. Enter a phone number with 8 digits, or leave it empty. Enter a postal code with 4 digits.',
    ),
  ).toBeTruthy();
});
