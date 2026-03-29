// ============================================
// features/accountingPeriod/hooks/useAccountingPeriods.ts
// ============================================

import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { accountingPeriodService } from '../accountingPeriod.service';
import type { CreatePeriodRequest } from '../accountingPeriod.types';

export const PERIODS_KEY = ['accounting-periods'] as const;

export const useAccountingPeriods = () =>
  useQuery({
    queryKey: PERIODS_KEY,
    queryFn:  accountingPeriodService.getAll,
  });

export const useCurrentPeriod = () =>
  useQuery({
    queryKey: [...PERIODS_KEY, 'current'],
    queryFn:  accountingPeriodService.getCurrent,
  });

export const useCreatePeriod = () => {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (data: CreatePeriodRequest) => accountingPeriodService.create(data),
    onSuccess:  () => qc.invalidateQueries({ queryKey: PERIODS_KEY }),
  });
};

export const useClosePeriod = () => {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (id: string) => accountingPeriodService.close(id),
    onSuccess:  () => qc.invalidateQueries({ queryKey: PERIODS_KEY }),
  });
};
