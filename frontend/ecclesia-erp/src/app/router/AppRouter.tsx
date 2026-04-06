// ============================================
// app/router/AppRouter.tsx
// ============================================

import React from 'react';
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { LoginPage }              from '../../features/auth/pages/LoginPage';
import { DashboardLayout }        from '../../features/dashboard/layout/DashboardLayout';
import { DashboardHome }          from '../../features/dashboard/pages/DashboardHome';
import { AccountsPage }           from '../../features/account/pages/AccountsPage';
import { AccountingPeriodsPage }  from '../../features/accountingPeriod/pages/AccountingPeriodsPage';
import { UsersPage }              from '../../features/users/pages/UsersPage';
// import { RolesPage }              from '../../features/roles/pages/RolesPage';
// import { RostrosPage }            from '../../features/rostro/pages/RostrosPage';
// import { CommunitiesPage }        from '../../features/community/pages/CommunitiesPage';
// import { CashAccountsPage }       from '../../features/cashAccount/pages/CashAccountsPage';
// import { MembersPage }            from '../../features/memberInfo/pages/MembersPage';
import { JournalVouchersPage }    from '../../features/journalVoucher/pages/JournalVouchersPage';
import { authService }            from '../../features/auth/auth.service';

const PrivateRoute: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const token = authService.getToken();
  return token ? <>{children}</> : <Navigate to="/login" replace />;
};

export const AppRouter: React.FC = () => (
  <BrowserRouter>
    <Routes>
      <Route path="/login" element={<LoginPage />} />
      <Route
        path="/"
        element={
          <PrivateRoute>
            <DashboardLayout />
          </PrivateRoute>
        }
      >
        {/* Dashboard */}
        <Route index                           element={<DashboardHome />} />

        {/* Sprint 1 — Contabilidad */}
        <Route path="accounting/accounts"      element={<AccountsPage />} />
        <Route path="accounting/periods"       element={<AccountingPeriodsPage />} />

        {/* Sprint 2 — Estructura + Comunidad */}
        {/* <Route path="community/rostros"        element={<RostrosPage />} />
        <Route path="community/communities"    element={<CommunitiesPage />} />
        <Route path="community/members"        element={<MembersPage />} />
        <Route path="finance/cash"             element={<CashAccountsPage />} /> */}

        {/* Sprint 3 — Motor contable */}
        <Route path="accounting/vouchers"      element={<JournalVouchersPage />} />

        {/* Settings */}
        <Route path="settings/users"           element={<UsersPage />} />
        {/* <Route path="settings/roles"           element={<RolesPage />} /> */}
      </Route>
      <Route path="*" element={<Navigate to="/" replace />} />
    </Routes>
  </BrowserRouter>
);
