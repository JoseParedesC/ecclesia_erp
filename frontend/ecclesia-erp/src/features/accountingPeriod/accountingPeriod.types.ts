// ============================================
// features/accountingPeriod/accountingPeriod.types.ts
// ============================================

export type PeriodStatus = 'OPEN' | 'CLOSED';

export interface AccountingPeriod {
  id: string;
  year: number;
  month: number;
  status: PeriodStatus;
  closedAt: string | null;
  closedBy: string | null;
  createdAt: string;
  createdBy: string | null;
}

export interface CreatePeriodRequest {
  year: number;
  month: number;
}
