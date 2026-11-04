import { render, screen } from '@testing-library/react';
import { expect, it, vi } from 'vitest';

import TabsLayout from '../src/app/(tabs)/_layout';

vi.mock('expo-router', async () => {
  const { createElement } = await import('react');
  const Screen = ({ name, options }: { name: string; options?: { title?: string } }) =>
    createElement('span', { 'data-testid': 'tab' }, `${name}:${options?.title}`);
  const Tabs = Object.assign(({ children }: { children?: React.ReactNode }) => createElement('div', null, children), {
    Screen,
  });
  return { Tabs };
});

it('offers Home, Orders and Profile as tabs in that order', () => {
  render(<TabsLayout />);

  const tabs = screen.getAllByTestId('tab').map((tab) => tab.textContent);

  expect(tabs).toEqual(['index:Home', 'orders:Orders', 'profile:Profile']);
});

it('does not turn stack screens into tabs', () => {
  render(<TabsLayout />);

  const names = screen.getAllByTestId('tab').map((tab) => tab.textContent?.split(':')[0]);

  expect(names).not.toContain('second');
  expect(names).not.toContain('onboarding');
  expect(names).toHaveLength(3);
});
