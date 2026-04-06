// ============================================
// features/journalVoucher/journalVoucher.types.ts
// ============================================

export type VoucherType   = 'INCOME' | 'EXPENSE' | 'ADJUSTMENT' | 'REVERSAL';
export type VoucherStatus = 'DRAFT'  | 'POSTED'  | 'CANCELLED';
export type LineType      = 'DEBIT'  | 'CREDIT';

// ── Line ────────────────────────────────────
export interface JournalVoucherLine {
  id:               string;
  journalVoucherId: string;
  accountId:        string;
  accountCode:      string;   // denormalized for display
  accountName:      string;   // denormalized for display
  lineType:         LineType;
  amount:           number;
}

export interface JournalVoucherLineInput {
  accountId: string;
  lineType:  LineType;
  amount:    number;
}

// ── Voucher ──────────────────────────────────
export interface JournalVoucher {
  id:                string;
  voucherNumber:     string;
  type:              VoucherType;
  status:            VoucherStatus;
  date:              string;          // ISO date "YYYY-MM-DD"
  description:       string | null;
  accountingPeriodId: string;
  rostroId:          string;
  rostroName:        string;
  communityId:       string;
  communityName:     string;
  lines:             JournalVoucherLine[];
  totalDebit:        number;
  totalCredit:       number;
  createdAt:         string;
  createdBy:         string | null;
  updatedAt:         string | null;
  updatedBy:         string | null;
}

// ── Commands ─────────────────────────────────
export interface CreateVoucherRequest {
  type:               VoucherType;
  date:               string;
  description?:       string | null;
  accountingPeriodId: string;
  rostroId:           string;
  communityId:        string;
  lines:              JournalVoucherLineInput[];
}

export interface ListVouchersQuery {
  page:       number;
  pageSize:   number;
  search?:    string;
  type?:      VoucherType | '';
  status?:    VoucherStatus | '';
  periodId?:  string;
  communityId?: string;
}

// ── Labels ───────────────────────────────────
export const VOUCHER_TYPE_LABELS: Record<VoucherType, string> = {
  INCOME:     'Ingreso',
  EXPENSE:    'Gasto',
  ADJUSTMENT: 'Ajuste',
  REVERSAL:   'Reverso',
};

export const VOUCHER_STATUS_LABELS: Record<VoucherStatus, string> = {
  DRAFT:     'Borrador',
  POSTED:    'Publicado',
  CANCELLED: 'Cancelado',
};

// ── Balance validation ────────────────────────
export const isBalanced = (lines: JournalVoucherLineInput[]): boolean => {
  const debit  = lines.filter(l => l.lineType === 'DEBIT').reduce((s, l) => s + l.amount, 0);
  const credit = lines.filter(l => l.lineType === 'CREDIT').reduce((s, l) => s + l.amount, 0);
  return Math.abs(debit - credit) < 0.001;
};

export const lineTotals = (lines: JournalVoucherLineInput[]) => ({
  debit:   lines.filter(l => l.lineType === 'DEBIT').reduce((s, l) => s + l.amount, 0),
  credit:  lines.filter(l => l.lineType === 'CREDIT').reduce((s, l) => s + l.amount, 0),
});
