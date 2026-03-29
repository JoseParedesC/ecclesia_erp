// ============================================
// features/auth/auth.service.ts
// ============================================

import { apiClient } from '../../shared/services/apiClient';
import type { LoginRequest, LoginResponse } from './auth.types';

export const authService = {
  login(credentials: LoginRequest): Promise<LoginResponse> {
    return apiClient.post<LoginResponse>('/api/auth/login', credentials);
  },

  saveToken(token: string): void {
    localStorage.setItem('ecclesia_token', token);
  },

  getToken(): string | null {
    return localStorage.getItem('ecclesia_token');
  },

  clearToken(): void {
    localStorage.removeItem('ecclesia_token');
  },
};
