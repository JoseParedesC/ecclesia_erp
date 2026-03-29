// ============================================
// features/auth/hooks/useLogin.ts
// ============================================

import { useMutation } from '@tanstack/react-query';
import { authService } from '../auth.service';
import type { LoginRequest, LoginResponse } from '../auth.types';

export const useLogin = (onSuccess?: (data: LoginResponse) => void) => {
  return useMutation<LoginResponse, Error, LoginRequest>({
    mutationFn: authService.login,
    onSuccess: (data) => {
      authService.saveToken(data.token);
      onSuccess?.(data);
    },
  });
};
