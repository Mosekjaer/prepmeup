import { render, screen } from '@testing-library/react';
import { describe, expect, it } from 'vitest';

import { DashboardRoute } from '../src/routes/dashboard';

describe('DashboardRoute', () => {
  it('renders its heading', () => {
    render(<DashboardRoute />);

    expect(screen.getByRole('heading', { name: 'Dashboard' })).toBeInTheDocument();
  });
});
