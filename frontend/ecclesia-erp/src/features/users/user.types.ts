// ============================================
// features/users/user.types.ts
// ============================================

// ── Paginación (patrón backend) ──────────────
export interface PagedResult<T> {
  items: T[];
  totalCount: number;
  page: number;
  pageSize: number;
  totalPages: number;
}

export interface PagedQuery {
  page: number;
  pageSize: number;
  search?: string;
  orderDescending: boolean;
}

// ── Role ─────────────────────────────────────
export interface Role {
  id: string;
  name: string;
}

export interface PagedResult<T> {
  items: T[];
  totalCount: number;
  page: number;
  pageSize: number;
  totalPages: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
}

// ── User ─────────────────────────────────────
export interface User {
  id: string;
  name: string;
  email: string;
  username: string;
  role: Role | null;
  createdAt: string;
  updatedAt: string | null;
}

// ── Commands ─────────────────────────────────
export interface CreateUserRequest {
  name: string;
  email: string;
  username: string;
  password: string;
  roleId?: string | null;
}

export interface UpdateUserRequest {
  name: string;
  email: string;
  username: string;
}

export interface AssignRoleRequest {
  roleId: string;
}
