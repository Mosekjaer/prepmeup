import { render, screen } from '@testing-library/react';
import { MemoryRouter } from 'react-router';
import { expect, it, vi } from 'vitest';

import { AppLayout } from '../src/routes/app-layout';

vi.mock('../src/lib/use-sign-out', () => ({
  useSignOut: () => vi.fn(),
}));

it('links to the dispatch queue and package types and offers sign out', () => {
  render(
    <MemoryRouter>
      <AppLayout />
    </MemoryRouter>,
  );

  expect(screen.getByRole('link', { name: 'Dispatch queue' })).toHaveAttribute('href', '/dispatch');
  expect(screen.getByRole('link', { name: 'Package types' })).toHaveAttribute('href', '/package-types');
  expect(screen.getByRole('button', { name: 'Sign out' })).toBeInTheDocument();
});

it('marks only the open tab as current', () => {
  render(
    <MemoryRouter initialEntries={['/package-types']}>
      <AppLayout />
    </MemoryRouter>,
  );

  expect(screen.getByRole('link', { name: 'Package types' })).toHaveAttribute('aria-current', 'page');
  expect(screen.getByRole('link', { name: 'Dispatch queue' })).not.toHaveAttribute('aria-current');
});

it('has no dashboard link', () => {
  render(
    <MemoryRouter>
      <AppLayout />
    </MemoryRouter>,
  );

  expect(screen.queryByRole('link', { name: 'Dashboard' })).not.toBeInTheDocument();
});
