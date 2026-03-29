// ============================================
// features/auth/index.ts
// ============================================

export { LoginPage } from './pages/LoginPage';
export { useLogin } from './auth/hooks/useLogin';
export { useLoginForm } from './auth/hooks/useLoginForm';
export { authService } from './auth/auth.service';
export type { LoginRequest, LoginResponse, AuthUser } from './auth/auth.types';
export type { Account, AccountType, CreateAccountRequest, UpdateAccountRequest } from './account/account.types';
export { AccountingPeriodsPage }    from './accountingPeriod/pages/AccountingPeriodsPage';
export { accountingPeriodService }  from './accountingPeriod/accountingPeriod.service';
export { useAccountingPeriods, useCurrentPeriod, useCreatePeriod, useClosePeriod } from './accountingPeriod/hooks/useAccountingPeriods';
export { PeriodStatusBadge }        from './accountingPeriod/components/PeriodStatusBadge';
export { formatPeriod }             from './accountingPeriod/utils/periodUtils';
export type { AccountingPeriod, PeriodStatus, CreatePeriodRequest } from './accountingPeriod/accountingPeriod.types';