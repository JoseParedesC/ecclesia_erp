// ============================================
// features/journalVoucher/hooks/useJournalVouchers.ts
// ============================================

import { useQuery, useMutation, useQueryClient, keepPreviousData } from '@tanstack/react-query';
import { journalVoucherService } from '../journalVoucher.service';
import type { CreateVoucherRequest, ListVouchersQuery } from '../journalVoucher.types';

export const VOUCHERS_KEY = ['journal-vouchers'] as const;

export const useJournalVouchers = (query: ListVouchersQuery) =>
  useQuery({
    queryKey:        [...VOUCHERS_KEY, query],
    queryFn:         () => journalVoucherService.getAll(query),
    placeholderData: keepPreviousData,
  });

export const useJournalVoucherById = (id: string) =>
  useQuery({
    queryKey: [...VOUCHERS_KEY, id],
    queryFn:  () => journalVoucherService.getById(id),
    enabled:  !!id,
  });

export const useCreateVoucher = () => {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (data: CreateVoucherRequest) => journalVoucherService.create(data),
    onSuccess:  () => qc.invalidateQueries({ queryKey: VOUCHERS_KEY }),
  });
};

export const usePostVoucher = () => {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (id: string) => journalVoucherService.post(id),
    onSuccess:  () => qc.invalidateQueries({ queryKey: VOUCHERS_KEY }),
  });
};

export const useCancelVoucher = () => {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (id: string) => journalVoucherService.cancel(id),
    onSuccess:  () => qc.invalidateQueries({ queryKey: VOUCHERS_KEY }),
  });
};
