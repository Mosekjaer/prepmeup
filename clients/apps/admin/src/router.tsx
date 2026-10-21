import { createBrowserRouter } from 'react-router';

import { AppLayout } from './routes/app-layout';
import { AuthGate } from './routes/auth-gate';
import { DashboardRoute } from './routes/dashboard';
import { NotFoundRoute } from './routes/not-found';

export const router = createBrowserRouter([
  {
    path: '/',
    element: (
      <AuthGate>
        <AppLayout />
      </AuthGate>
    ),
    children: [
      { index: true, element: <DashboardRoute /> },
      { path: '*', element: <NotFoundRoute /> },
    ],
  },
]);
