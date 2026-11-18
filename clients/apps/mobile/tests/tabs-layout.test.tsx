import { render, screen } from '@testing-library/react';
import { beforeEach, expect, it, vi } from 'vitest';

import TabsLayout from '../src/app/(tabs)/_layout';

const mocks = vi.hoisted(() => ({
  token: 'access-token' as string | undefined,
}));

vi.mock('../src/lib/auth', () => ({
  useAuth: () => ({ token: mocks.token, signIn: vi.fn(), signOut: vi.fn() }),
}));

vi.mock('expo-router', async () => {
  const { createElement } = await import('react');
  const Screen = ({ name, options }: { name: string; options?: { title?: string } }) =>
    createElement('span', { 'data-testid': 'tab' }, `${name}:${options?.title}`);
  const Protected = ({ guard, children }: { guard: boolean; children?: React.ReactNode }) =>
    guard ? createElement('div', null, children) : null;
  const Tabs = Object.assign(
    ({ children, screenOptions }: { children?: React.ReactNode; screenOptions?: { tabBarStyle?: { display?: string } } }) =>
      createElement('div', { 'data-testid': 'tabs', 'data-tab-bar': screenOptions?.tabBarStyle?.display ?? 'flex' }, children),
    { Screen, Protected },
  );
  return { Tabs };
});

beforeEach(() => {
  mocks.token = 'access-token';
});

it('offers Home, Orders and Profile as tabs in that order when signed in', () => {
  render(<TabsLayout />);

  const tabs = screen.getAllByTestId('tab').map((tab) => tab.textContent);

  expect(tabs).toEqual(['index:Home', 'orders:Orders', 'profile:Profile']);
});

it('does not turn stack screens into tabs', () => {
  render(<TabsLayout />);

  const names = screen.getAllByTestId('tab').map((tab) => tab.textContent?.split(':')[0]);

  expect(names).not.toContain('onboarding');
  expect(names).toHaveLength(3);
});

it('hides the tab bar when signed out', () => {
  mocks.token = undefined;

  render(<TabsLayout />);

  expect(screen.getByTestId('tabs').getAttribute('data-tab-bar')).toBe('none');
});

it('only offers Home as a tab when signed out', () => {
  mocks.token = undefined;

  render(<TabsLayout />);

  const tabs = screen.getAllByTestId('tab').map((tab) => tab.textContent);

  expect(tabs).toEqual(['index:Home']);
});

it('shows the tab bar and the other tabs when the token arrives after the app has started', () => {
  mocks.token = undefined;
  const { rerender } = render(<TabsLayout />);
  expect(screen.getAllByTestId('tab')).toHaveLength(1);

  mocks.token = 'access-token';
  rerender(<TabsLayout />);

  expect(screen.getAllByTestId('tab').map((tab) => tab.textContent)).toEqual(['index:Home', 'orders:Orders', 'profile:Profile']);
  expect(screen.getByTestId('tabs').getAttribute('data-tab-bar')).not.toBe('none');
});
