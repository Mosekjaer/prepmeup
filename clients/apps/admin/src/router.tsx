import { createBrowserRouter } from 'react-router';

import { AppLayout } from './routes/app-layout';
import { DashboardRoute } from './routes/dashboard';
import { NotFoundRoute } from './routes/not-found';

export const router = createBrowserRouter([
  {
    path: '/',
    element: <AppLayout />,
    children: [
      { index: true, element: <DashboardRoute /> },
      { path: '*', element: <NotFoundRoute /> },
    ],
  },
]);
