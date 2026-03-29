// ============================================
// features/account/account.types.ts
// ============================================

export type AccountType = 'ASSET' | 'LIABILITY' | 'EQUITY' | 'INCOME' | 'EXPENSE';

export interface Account {
  id: string;
  code: string;
  name: string;
  type: AccountType;
  parentAccountId: string | null;
  children?: Account[];
  createdAt: string;
  createdBy: string | null;
  updatedAt: string | null;
  updatedBy: string | null;
}

export interface CreateAccountRequest {
  code: string;
  name: string;
  type: AccountType;
  parentAccountId?: string | null;
}

export interface UpdateAccountRequest {
  name: string;
  type: AccountType;
  parentAccountId?: string | null;
}

export interface AccountFilters {
  search?: string;
  type?: AccountType | '';
}
