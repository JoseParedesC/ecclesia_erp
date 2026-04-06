// ============================================
// features/auth/index.ts
// ============================================

export { LoginPage } from './auth/pages/LoginPage';
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
export { UsersPage }    from './users/pages/UsersPage';
export { userService }  from './users/user.service';
export { roleService }  from './users/role.service';
export { useUsers, useRoles, useCreateUser, useUpdateUser, useDeleteUser, useAssignRole } from './users/hooks/useUsers';
export type { User, Role, CreateUserRequest, UpdateUserRequest, AssignRoleRequest, PagedResult } from './users/user.types';
export { JournalVouchersPage }  from './journalVoucher/pages/JournalVouchersPage';
export { journalVoucherService } from './journalVoucher/journalVoucher.service';
export { useJournalVouchers, useCreateVoucher, usePostVoucher, useCancelVoucher } from './journalVoucher/hooks/useJournalVouchers';
export { VoucherStatusBadge }   from './journalVoucher/components/VoucherStatusBadge';
export { VoucherTypeBadge }     from './journalVoucher/components/VoucherTypeBadge';
export { isBalanced, lineTotals, VOUCHER_TYPE_LABELS, VOUCHER_STATUS_LABELS } from './journalVoucher/journalVoucher.types';
export type { JournalVoucher, JournalVoucherLine, VoucherType, VoucherStatus, LineType, CreateVoucherRequest } from './journalVoucher/journalVoucher.types';
