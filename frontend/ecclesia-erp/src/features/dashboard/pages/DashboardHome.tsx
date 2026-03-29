// ============================================
// features/dashboard/pages/DashboardHome.tsx
// ============================================

import React from 'react';
import styles from './DashboardHome.module.css';

interface StatCard {
  label: string;
  value: string;
  sub: string;
  trend: 'up' | 'down' | 'neutral';
  iconPath: string;
}

const stats: StatCard[] = [
  {
    label: 'Ingresos del período',
    value: '$12.480.000',
    sub: '+8% vs mes anterior',
    trend: 'up',
    iconPath: 'M7 11l5-5m0 0l5 5m-5-5v12',
  },
  {
    label: 'Gastos del período',
    value: '$7.920.000',
    sub: '+3% vs mes anterior',
    trend: 'down',
    iconPath: 'M17 13l-5 5m0 0l-5-5m5 5V6',
  },
  {
    label: 'Balance neto',
    value: '$4.560.000',
    sub: 'Período activo: Mar 2026',
    trend: 'up',
    iconPath: 'M12 8c-1.657 0-3 .895-3 2s1.343 2 3 2 3 .895 3 2-1.343 2-3 2m0-8c1.11 0 2.08.402 2.599 1M12 8V7m0 1v8m0 0v1m0-1c-1.11 0-2.08-.402-2.599-1M21 12a9 9 0 11-18 0 9 9 0 0118 0z',
  },
  {
    label: 'Aprobaciones pendientes',
    value: '5',
    sub: '2 urgentes',
    trend: 'neutral',
    iconPath: 'M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z',
  },
];

export const DashboardHome: React.FC = () => (
  <div className={styles.page}>
    <div className={styles.pageHeader}>
      <h1 className={styles.title}>Resumen general</h1>
      <span className={styles.period}>Marzo 2026</span>
    </div>

    <div className={styles.grid}>
      {stats.map((s) => (
        <div key={s.label} className={styles.card}>
          <div className={styles.cardTop}>
            <span className={styles.cardLabel}>{s.label}</span>
            <div className={`${styles.cardIcon} ${styles[`icon_${s.trend}`]}`}>
              <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth={2} strokeLinecap="round" strokeLinejoin="round" aria-hidden>
                <path d={s.iconPath} />
              </svg>
            </div>
          </div>
          <div className={styles.cardValue}>{s.value}</div>
          <div className={`${styles.cardSub} ${styles[`sub_${s.trend}`]}`}>{s.sub}</div>
        </div>
      ))}
    </div>

    <div className={styles.section}>
      <h2 className={styles.sectionTitle}>Actividad reciente</h2>
      <div className={styles.emptyState}>
        <svg width="32" height="32" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth={1.2} strokeLinecap="round" strokeLinejoin="round" aria-hidden>
          <path d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z" />
        </svg>
        <p>No hay asientos recientes</p>
      </div>
    </div>
  </div>
);
