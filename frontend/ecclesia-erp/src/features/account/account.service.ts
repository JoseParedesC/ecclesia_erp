// ============================================
// features/account/account.service.ts
// ============================================

import { apiClient } from '../../shared/services/apiClient';
import type {
  Account,
  CreateAccountRequest,
  UpdateAccountRequest,
} from './account.types';

const BASE = '/api/accounts';

export const accountService = {
  getAll(): Promise<Account[]> {
    return apiClient.get<Account[]>(BASE);
  },

  getById(id: string): Promise<Account> {
    return apiClient.get<Account>(`${BASE}/${id}`);
  },

  create(data: CreateAccountRequest): Promise<Account> {
    return apiClient.post<Account>(BASE, data);
  },

  update(id: string, data: UpdateAccountRequest): Promise<Account> {
    return apiClient.put<Account>(`${BASE}/${id}`, data);
  },

  remove(id: string): Promise<void> {
    return apiClient.delete<void>(`${BASE}/${id}`);
  },
};
