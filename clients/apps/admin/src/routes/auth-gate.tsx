import type { ReactNode } from 'react';
import { useAuth } from 'react-oidc-context';

import { useAdminAccess } from '../lib/admin-access';
import { useSignOut } from '../lib/use-sign-out';

function Loading() {
  return <p role="status">Loading…</p>;
}

export function AuthGate({ children }: { children: ReactNode }) {
  const auth = useAuth();
  const { access, retry } = useAdminAccess();
  const signOut = useSignOut();

  if (auth.isLoading) {
    return <Loading />;
  }

  if (!auth.isAuthenticated) {
    return (
      <section>
        <h1>PrepMeUp Admin</h1>
        <button onClick={() => void auth.signinRedirect()}>Sign in</button>
      </section>
    );
  }

  if (access === 'checking') {
    return <Loading />;
  }

  if (access === 'error') {
    return (
      <section>
        <h1>PrepMeUp Admin</h1>
        <p>We could not check your access. Please try again.</p>
        <button onClick={retry}>Try again</button>
      </section>
    );
  }

  if (access === 'denied') {
    return (
      <section>
        <h1>PrepMeUp Admin</h1>
        <p>This account does not have access to the admin panel.</p>
        <button onClick={signOut}>Sign out</button>
      </section>
    );
  }

  return <>{children}</>;
}
