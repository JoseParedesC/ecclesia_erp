// ============================================
// features/accountingPeriod/accountingPeriod.service.ts
// ============================================

import { apiClient } from '../../shared/services/apiClient';
import type { AccountingPeriod, CreatePeriodRequest } from './accountingPeriod.types';

const BASE = '/api/accounting-periods';

export const accountingPeriodService = {
  getAll(): Promise<AccountingPeriod[]> {
    return apiClient.get<AccountingPeriod[]>(BASE);
  },

  getCurrent(): Promise<AccountingPeriod> {
    return apiClient.get<AccountingPeriod>(`${BASE}/current`);
  },

  create(data: CreatePeriodRequest): Promise<AccountingPeriod> {
    return apiClient.post<AccountingPeriod>(BASE, data);
  },

  close(id: string): Promise<AccountingPeriod> {
    return apiClient.patch<AccountingPeriod>(`${BASE}/${id}/close`, {});
  },
};
