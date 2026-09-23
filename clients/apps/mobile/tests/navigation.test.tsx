import { fireEvent, render, screen } from '@testing-library/react';
import { expect, it, vi } from 'vitest';

import HomeScreen from '../src/app/index';

const push = vi.fn();

vi.mock('expo-router', () => ({
  useRouter: () => ({ push }),
}));

it('navigates to the second screen when the button is pressed', () => {
  render(<HomeScreen />);

  fireEvent.click(screen.getByText('Go to second screen'));

  expect(push).toHaveBeenCalledWith('/second');
});
