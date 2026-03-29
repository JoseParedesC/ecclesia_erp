// ============================================
// features/account/hooks/useAccountForm.ts
// ============================================

import { useForm } from 'react-hook-form';
import type { CreateAccountRequest, Account } from '../account.types';

export const useAccountForm = (defaults?: Partial<Account>) =>
  useForm<CreateAccountRequest>({
    mode: 'onBlur',
    defaultValues: {
      code: defaults?.code ?? '',
      name: defaults?.name ?? '',
      type: defaults?.type ?? 'ASSET',
      parentAccountId: defaults?.parentAccountId ?? null,
    },
  });
