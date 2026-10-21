import { NavLink, Outlet } from 'react-router';

import { useSignOut } from '../lib/use-sign-out';

export function AppLayout() {
  const signOut = useSignOut();

  return (
    <div className="layout">
      <header className="layout__header">
        <strong>PrepMeUp Admin</strong>
        <nav>
          <NavLink to="/">Dashboard</NavLink>
        </nav>
        <button onClick={signOut}>Sign out</button>
      </header>
      <main className="layout__main">
        <Outlet />
      </main>
    </div>
  );
}
