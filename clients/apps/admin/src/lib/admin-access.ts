import { setAccessTokenProvider, SwaggerException } from '@prepmeup/api-client';
import { useEffect, useState } from 'react';
import { useAuth } from 'react-oidc-context';

import { adminClient } from './api';

type AdminAccess = 'checking' | 'allowed' | 'denied' | 'error';

export function useAdminAccess(): { access: AdminAccess; retry: () => void } {
  const { isAuthenticated, removeUser, user } = useAuth();
  const token = user?.access_token;
  const [access, setAccess] = useState<AdminAccess>('checking');
  const [attempt, setAttempt] = useState(0);

  useEffect(() => {
    setAccessTokenProvider(() => token);
  }, [token]);

  useEffect(() => {
    if (!isAuthenticated) {
      return;
    }
    adminClient
      .ping()
      .then(() => setAccess('allowed'))
      .catch((error: unknown) => {
        if (error instanceof SwaggerException && error.status === 403) {
          setAccess('denied');
          return;
        }
        if (error instanceof SwaggerException && error.status === 401) {
          void removeUser();
          return;
        }
        setAccess('error');
      });
  }, [isAuthenticated, removeUser, attempt]);

  function retry() {
    setAccess('checking');
    setAttempt((current) => current + 1);
  }

  return { access, retry };
}
