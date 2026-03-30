// ============================================
// features/users/role.service.ts
// ============================================

import { apiClient } from '../../shared/services/apiClient';
import type { PagedResult, Role } from './user.types';

export const roleService = {
  async getAll(): Promise<Role[]> {
    const response = await apiClient.get<Role[]>('/api/roles/');
    return response;
  },

  async search(): Promise<PagedResult<Role>> {
    return apiClient.get<PagedResult<Role>>('/api/roles/search');
  },
};
