// ============================================
// features/auth/hooks/useLoginForm.ts
// ============================================

import { useForm } from 'react-hook-form';
import type { LoginRequest } from '../auth.types';

export const useLoginForm = () => {
  return useForm<LoginRequest>({
    mode: 'onBlur',
    defaultValues: { email: '', password: '' },
  });
};
