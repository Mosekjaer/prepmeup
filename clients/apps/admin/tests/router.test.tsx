import { render, screen } from '@testing-library/react';
import { createMemoryRouter, RouterProvider } from 'react-router';
import { expect, it, vi } from 'vitest';

import { routes } from '../src/router';

vi.mock('../src/lib/use-sign-out', () => ({
  useSignOut: () => vi.fn(),
}));

vi.mock('../src/routes/auth-gate', () => ({
  AuthGate: ({ children }: { children: React.ReactNode }) => <>{children}</>,
}));

function renderAt(path: string) {
  render(<RouterProvider router={createMemoryRouter(routes, { initialEntries: [path] })} />);
}

it('shows the dispatch queue at /dispatch', () => {
  renderAt('/dispatch');

  expect(screen.getByRole('heading', { name: 'Dispatch queue' })).toBeInTheDocument();
});

it('redirects the root path to the dispatch queue', () => {
  renderAt('/');

  expect(screen.getByRole('heading', { name: 'Dispatch queue' })).toBeInTheDocument();
});

it('shows the not-found page for an unknown path', () => {
  renderAt('/xyz');

  expect(screen.getByRole('heading', { name: 'Not found' })).toBeInTheDocument();
});

it('shows package types at /package-types', () => {
  renderAt('/package-types');

  expect(screen.getByRole('heading', { name: 'Package types' })).toBeInTheDocument();
});
