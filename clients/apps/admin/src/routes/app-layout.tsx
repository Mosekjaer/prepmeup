import { NavLink, Outlet } from 'react-router';

export function AppLayout() {
  return (
    <div className="layout">
      <header className="layout__header">
        <strong>PrepMeUp Admin</strong>
        <nav>
          <NavLink to="/">Dashboard</NavLink>
        </nav>
      </header>
      <main className="layout__main">
        <Outlet />
      </main>
    </div>
  );
}
