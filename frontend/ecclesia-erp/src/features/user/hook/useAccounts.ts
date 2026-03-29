// ============================================
// features/account/hooks/useAccounts.ts
// ============================================

import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { accountService } from '../account.service';
import type { CreateAccountRequest, UpdateAccountRequest } from '../account.types';

export const ACCOUNTS_KEY = ['accounts'] as const;

export const useAccounts = () =>
  useQuery({
    queryKey: ACCOUNTS_KEY,
    queryFn: accountService.getAll,
  });

export const useCreateAccount = () => {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (data: CreateAccountRequest) => accountService.create(data),
    onSuccess: () => qc.invalidateQueries({ queryKey: ACCOUNTS_KEY }),
  });
};

export const useUpdateAccount = () => {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: ({ id, data }: { id: string; data: UpdateAccountRequest }) =>
      accountService.update(id, data),
    onSuccess: () => qc.invalidateQueries({ queryKey: ACCOUNTS_KEY }),
  });
};

export const useDeleteAccount = () => {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (id: string) => accountService.remove(id),
    onSuccess: () => qc.invalidateQueries({ queryKey: ACCOUNTS_KEY }),
  });
};
