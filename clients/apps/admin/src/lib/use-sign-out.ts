import { useAuth } from 'react-oidc-context';

export function useSignOut(): () => void {
  const { signoutRedirect, user } = useAuth();
  return () => void signoutRedirect({ id_token_hint: user?.id_token });
}
