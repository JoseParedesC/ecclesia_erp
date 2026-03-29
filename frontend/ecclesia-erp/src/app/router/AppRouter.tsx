// ============================================
// app/router/AppRouter.tsx
// ============================================

import React from 'react';
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { LoginPage }               from '../../features/auth/pages/LoginPage';
import { DashboardLayout }         from '../../features/dashboard/layout/DashboardLayout';
import { DashboardHome }           from '../../features/dashboard/pages/DashboardHome';
import { AccountsPage }            from '../../features/account/pages/AccountsPage';
import { AccountingPeriodsPage }   from '../../features/accountingPeriod/pages/AccountingPeriodsPage';
import { authService }             from '../../features/auth/auth.service';
import { UsersPage } from '../../features';

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
        <Route index                         element={<DashboardHome />} />
        <Route path="accounting/accounts"    element={<AccountsPage />} />
        <Route path="accounting/periods"     element={<AccountingPeriodsPage />} />
        <Route path="settings/users"         element={<UsersPage />} />
        <Route path="settings/roles"         element={<AccountingPeriodsPage />} />
        <Route path="settings/permissions"   element={<AccountingPeriodsPage />} />
      </Route>
      <Route path="*" element={<Navigate to="/" replace />} />
    </Routes>
  </BrowserRouter>
);
