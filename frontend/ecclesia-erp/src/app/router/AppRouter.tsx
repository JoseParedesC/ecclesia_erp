// ============================================
// app/router/AppRouter.tsx
// ============================================

import React from 'react';
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { LoginPage } from '../../features/pages/LoginPage';
import { DashboardLayout } from '../../features/dashboard/layout/DashboardLayout';
import { DashboardHome } from '../../features/dashboard/pages/DashboardHome';
import { AccountsPage } from '../../features/account/pages/AccountsPage';
import { authService } from '../../features/auth/auth.service';

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
        <Route index                        element={<DashboardHome />} />
        <Route path="accounting/accounts"   element={<AccountsPage />} />
      </Route>
      <Route path="*" element={<Navigate to="/" replace />} />
    </Routes>
  </BrowserRouter>
);
