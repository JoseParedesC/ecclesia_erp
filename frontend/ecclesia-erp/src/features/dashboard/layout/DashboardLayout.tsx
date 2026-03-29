// ============================================
// features/dashboard/layout/DashboardLayout.tsx
// ============================================

import React from 'react';
import { Outlet } from 'react-router-dom';
import { Sidebar } from '../components/Sidebar';
import { Topbar } from '../components/Topbar';
import { useSidebar } from '../hooks/useSidebar';
import styles from './DashboardLayout.module.css';

export const DashboardLayout: React.FC = () => {
  const { collapsed, toggle, toggleGroup, isGroupOpen } = useSidebar();

  return (
    <div className={styles.layout}>
      <Sidebar
        collapsed={collapsed}
        onToggle={toggle}
        openGroups={isGroupOpen}
        onToggleGroup={toggleGroup}
      />
      <div className={styles.main}>
        <Topbar />
        <main className={styles.content}>
          <Outlet />
        </main>
      </div>
    </div>
  );
};
