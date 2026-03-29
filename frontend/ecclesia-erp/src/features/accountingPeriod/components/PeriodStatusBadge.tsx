// ============================================
// features/accountingPeriod/components/PeriodStatusBadge.tsx
// ============================================

import React from 'react';
import type { PeriodStatus } from '../accountingPeriod.types';
import styles from './PeriodStatusBadge.module.css';

interface Props { status: PeriodStatus }

export const PeriodStatusBadge: React.FC<Props> = ({ status }) => (
  <span className={`${styles.badge} ${styles[status.toLowerCase()]}`}>
    <span className={styles.dot} aria-hidden />
    {status === 'OPEN' ? 'Abierto' : 'Cerrado'}
  </span>
);
