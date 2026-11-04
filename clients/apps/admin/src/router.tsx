import { createBrowserRouter, Navigate } from 'react-router';

import { AppLayout } from './routes/app-layout';
import { AuthGate } from './routes/auth-gate';
import { DispatchQueueRoute } from './routes/dispatch-queue';
import { NotFoundRoute } from './routes/not-found';
import { PackageTypesRoute } from './routes/package-types';

export const routes = [
  {
    path: '/',
    element: (
      <AuthGate>
        <AppLayout />
      </AuthGate>
    ),
    children: [
      { index: true, element: <Navigate to="/dispatch" replace /> },
      { path: 'dispatch', element: <DispatchQueueRoute /> },
      { path: 'package-types', element: <PackageTypesRoute /> },
      { path: '*', element: <NotFoundRoute /> },
    ],
  },
];

export const router = createBrowserRouter(routes);
