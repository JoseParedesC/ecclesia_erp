// ============================================
// features/users/role.service.ts
// ============================================

import { apiClient } from '../../shared/services/apiClient';
import type { Role } from './user.types';

export const roleService = {
  getAll(): Promise<Role[]> {
    return apiClient.get<Role[]>('/api/roles');
  },
};
