// ============================================
// features/accountingPeriod/utils/periodUtils.ts
// ============================================

import type { AccountingPeriod } from '../accountingPeriod.types';

export const MONTH_NAMES = [
  'Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio',
  'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre',
];

export const formatPeriod = (year: number, month: number): string =>
  `${MONTH_NAMES[month - 1]} ${year}`;

/** Returns {year, month} for the period that would come after the latest existing one */
export const nextPeriod = (periods: AccountingPeriod[]): { year: number; month: number } => {
  if (!periods.length) {
    const now = new Date();
    return { year: now.getFullYear(), month: now.getMonth() + 1 };
  }
  const latest = periods.reduce((a, b) =>
    a.year > b.year || (a.year === b.year && a.month > b.month) ? a : b,
  );
  const month = latest.month === 12 ? 1 : latest.month + 1;
  const year  = latest.month === 12 ? latest.year + 1 : latest.year;
  return { year, month };
};

export const sortPeriods = (periods: AccountingPeriod[]): AccountingPeriod[] =>
  [...periods].sort((a, b) =>
    b.year !== a.year ? b.year - a.year : b.month - a.month,
  );
