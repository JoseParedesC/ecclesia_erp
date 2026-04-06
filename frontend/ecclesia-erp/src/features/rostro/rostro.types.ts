// ============================================
// features/Rostro/Rostro.types.ts
// ============================================

export type RostroType = 'ASSET' | 'LIABILITY' | 'EQUITY' | 'INCOME' | 'EXPENSE';

export interface Rostro {
  id: string;
  code: string;
  name: string;
  type: RostroType;
  parentRostroId: string | null;
  children?: Rostro[];
  createdAt: string;
  createdBy: string | null;
  updatedAt: string | null;
  updatedBy: string | null;
}

export interface CreateRostroRequest {
  code: string;
  name: string;
  type: RostroType;
  parentRostroId?: string | null;
}

export interface UpdateRostroRequest {
  name: string;
  type: RostroType;
  parentRostroId?: string | null;
}

export interface RostroFilters {
  search?: string;
  type?: RostroType | '';
}
