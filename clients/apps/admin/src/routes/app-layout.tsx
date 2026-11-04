import { NavLink, Outlet } from 'react-router';

import { useSignOut } from '../lib/use-sign-out';

export function AppLayout() {
  const signOut = useSignOut();

  return (
    <div className="layout">
      <aside className="layout__sidebar">
        <strong className="layout__brand">PrepMeUp Admin</strong>
        <nav className="layout__nav">
          <NavLink to="/dispatch">Dispatch queue</NavLink>
          <NavLink to="/package-types">Package types</NavLink>
          <button onClick={signOut}>Sign out</button>
        </nav>
      </aside>
      <main className="layout__main">
        <Outlet />
      </main>
    </div>
  );
}
