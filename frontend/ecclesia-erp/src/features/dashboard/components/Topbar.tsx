// ============================================
// features/dashboard/components/Topbar.tsx
// ============================================

import React from 'react';
import { useNavigate } from 'react-router-dom';
import { authService } from '../../auth/auth.service';
import styles from './Topbar.module.css';

export const Topbar: React.FC = () => {
  const navigate = useNavigate();

  const handleLogout = () => {
    authService.clearToken();
    navigate('/login', { replace: true });
  };

  return (
    <header className={styles.topbar}>
      <div className={styles.left}>
        <span className={styles.greeting}>Panel de administración</span>
      </div>

      <div className={styles.right}>
        {/* User avatar */}
        <div className={styles.userChip}>
          <div className={styles.avatar} aria-hidden>A</div>
          <span className={styles.userName}>Admin</span>
        </div>

        {/* Logout */}
        <button
          className={styles.logoutBtn}
          onClick={handleLogout}
          aria-label="Cerrar sesión"
          title="Cerrar sesión"
        >
          <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth={1.8} strokeLinecap="round" strokeLinejoin="round" aria-hidden>
            <path d="M17 16l4-4m0 0l-4-4m4 4H7m6 4v1a3 3 0 01-3 3H6a3 3 0 01-3-3V7a3 3 0 013-3h4a3 3 0 013 3v1" />
          </svg>
        </button>
      </div>
    </header>
  );
};
