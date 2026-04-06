// ============================================
// features/Comminity/Comminity.service.ts
// ============================================

import { apiClient } from '../../shared/services/apiClient';
import type {
  Comminity,
  CreateComminityRequest,
  UpdateComminityRequest,
} from './community.types';

const BASE = '/api/Comminitys';

export const ComminityService = {
  getAll(): Promise<Comminity[]> {
    return apiClient.get<Comminity[]>(BASE);
  },

  getById(id: string): Promise<Comminity> {
    return apiClient.get<Comminity>(`${BASE}/${id}`);
  },

  create(data: CreateComminityRequest): Promise<Comminity> {
    return apiClient.post<Comminity>(BASE, data);
  },

  update(id: string, data: UpdateComminityRequest): Promise<Comminity> {
    return apiClient.put<Comminity>(`${BASE}/${id}`, data);
  },

  remove(id: string): Promise<void> {
    return apiClient.delete<void>(`${BASE}/${id}`);
  },
};
