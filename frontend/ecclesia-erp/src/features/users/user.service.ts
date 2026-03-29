// ============================================
// features/users/user.service.ts
// ============================================

import { apiClient } from '../../shared/services/apiClient';
import type {
  User,
  PagedResult,
  PagedQuery,
  CreateUserRequest,
  UpdateUserRequest,
  AssignRoleRequest,
} from './user.types';

const BASE = '/api/users';

export const userService = {
  getAll(query: PagedQuery): Promise<PagedResult<User>> {
    const params = new URLSearchParams({
      page:     String(query.page),
      pageSize: String(query.pageSize),
      orderDescending: String(query.orderDescending),
      ...(query.search ? { search: query.search } : {}),
    });
    return apiClient.get<PagedResult<User>>(`${BASE}?${params}`);
  },

  getById(id: string): Promise<User> {
    return apiClient.get<User>(`${BASE}/${id}`);
  },

  create(data: CreateUserRequest): Promise<User> {
    return apiClient.post<User>(BASE, data);
  },

  update(id: string, data: UpdateUserRequest): Promise<User> {
    return apiClient.put<User>(`${BASE}/${id}`, data);
  },

  remove(id: string): Promise<void> {
    return apiClient.delete<void>(`${BASE}/${id}`);
  },

  assignRole(userId: string, data: AssignRoleRequest): Promise<void> {
    return apiClient.post<void>(`${BASE}/${userId}/roles`, data);
  },
};
