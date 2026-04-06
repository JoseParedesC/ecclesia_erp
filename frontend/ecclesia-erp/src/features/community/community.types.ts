// ============================================
// features/Comminity/Comminity.types.ts
// ============================================

export type ComminityType = 'ASSET' | 'LIABILITY' | 'EQUITY' | 'INCOME' | 'EXPENSE';

export interface Comminity {
  id: string;
  code: string;
  name: string;
  type: ComminityType;
  parentComminityId: string | null;
  children?: Comminity[];
  createdAt: string;
  createdBy: string | null;
  updatedAt: string | null;
  updatedBy: string | null;
}

export interface CreateComminityRequest {
  code: string;
  name: string;
  type: ComminityType;
  parentComminityId?: string | null;
}

export interface UpdateComminityRequest {
  name: string;
  type: ComminityType;
  parentComminityId?: string | null;
}

export interface ComminityFilters {
  search?: string;
  type?: ComminityType | '';
}
