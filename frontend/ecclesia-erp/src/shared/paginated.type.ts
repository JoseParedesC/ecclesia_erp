// // ── Paginación (patrón backend) ──────────────
// export interface PagedResult<T> {
//   items: T[];
//   totalCount: number;
//   page: number;
//   pageSize: number;
//   totalPages: number;
// }

// export interface PagedQuery {
//   page: number;
//   pageSize: number;
//   search?: string;
//   orderDescending: boolean;
// }

export interface PaginatedResponse<T> {
  items: T[];
  totalCount: number;
  page: number;
  pageSize: number;
  totalPages: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
}