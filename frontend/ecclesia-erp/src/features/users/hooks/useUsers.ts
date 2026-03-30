// ============================================
// features/users/hooks/useUsers.ts
// ============================================

import { useQuery, useMutation, useQueryClient, keepPreviousData } from '@tanstack/react-query';
import { userService } from '../user.service';
import { roleService } from '../role.service';
import type { CreateUserRequest, UpdateUserRequest, AssignRoleRequest } from '../user.types';

export const USERS_KEY  = ['users']  as const;
export const ROLES_KEY  = ['roles']  as const;

// ── Queries ──────────────────────────────────

export const useUsers = (page: number, pageSize: number, search: string) =>
  useQuery({
    queryKey:    [...USERS_KEY, page, pageSize, search],
    queryFn:     () => userService.getAll({ page, pageSize, search, orderDescending: false }),
    placeholderData: keepPreviousData,
  });

export const useRoles = () =>
  useQuery({
    queryKey: ROLES_KEY,
    queryFn:  roleService.getAll,
    staleTime: 1000 * 60 * 10, // roles raramente cambian
  });

export const useRolesSearch = () => {
  return useQuery({
    queryKey: ROLES_KEY,
    queryFn: roleService.search,
    staleTime: 1000 * 60 * 10,
  });
};

// ── Mutations ────────────────────────────────

export const useCreateUser = () => {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (data: CreateUserRequest) => userService.create(data),
    onSuccess:  () => qc.invalidateQueries({ queryKey: USERS_KEY }),
  });
};

export const useUpdateUser = () => {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: ({ id, data }: { id: string; data: UpdateUserRequest }) =>
      userService.update(id, data),
    onSuccess: () => qc.invalidateQueries({ queryKey: USERS_KEY }),
  });
};

export const useDeleteUser = () => {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (id: string) => userService.remove(id),
    onSuccess:  () => qc.invalidateQueries({ queryKey: USERS_KEY }),
  });
};

export const useAssignRole = () => {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: ({ userId, data }: { userId: string; data: AssignRoleRequest }) =>
      userService.assignRole(userId, data),
    onSuccess: () => qc.invalidateQueries({ queryKey: USERS_KEY }),
  });
};
