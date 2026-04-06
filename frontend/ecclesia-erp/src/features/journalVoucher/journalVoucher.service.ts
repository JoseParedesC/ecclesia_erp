// ============================================
// features/journalVoucher/journalVoucher.service.ts
// ============================================

import { apiClient } from '../../shared/services/apiClient';
import type { JournalVoucher, CreateVoucherRequest, ListVouchersQuery } from './journalVoucher.types';
import type { PagedResult } from '../users/user.types';

const BASE = '/api/journal-vouchers';

export const journalVoucherService = {
  getAll(q: ListVouchersQuery): Promise<PagedResult<JournalVoucher>> {
    const p = new URLSearchParams({
      page:     String(q.page),
      pageSize: String(q.pageSize),
      ...(q.search     ? { search:      q.search }     : {}),
      ...(q.type       ? { type:        q.type }       : {}),
      ...(q.status     ? { status:      q.status }     : {}),
      ...(q.periodId   ? { periodId:    q.periodId }   : {}),
      ...(q.communityId? { communityId: q.communityId }: {}),
    });
    return apiClient.get(`${BASE}?${p}`);
  },

  getById(id: string): Promise<JournalVoucher> {
    return apiClient.get(`${BASE}/${id}`);
  },

  create(data: CreateVoucherRequest): Promise<JournalVoucher> {
    return apiClient.post(BASE, data);
  },

  post(id: string): Promise<JournalVoucher> {
    return apiClient.patch(`${BASE}/${id}/post`, {});
  },

  cancel(id: string): Promise<JournalVoucher> {
    return apiClient.patch(`${BASE}/${id}/cancel`, {});
  },
};
