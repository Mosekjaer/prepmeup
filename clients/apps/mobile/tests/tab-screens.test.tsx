import { render, screen } from '@testing-library/react';
import { expect, it } from 'vitest';

import OrdersScreen from '../src/app/(tabs)/orders';
import ProfileScreen from '../src/app/(tabs)/profile';

it('shows the Orders screen', () => {
  render(<OrdersScreen />);

  expect(screen.getByText('Orders')).toBeTruthy();
});

it('shows the Profile screen', () => {
  render(<ProfileScreen />);

  expect(screen.getByText('Profile')).toBeTruthy();
});
