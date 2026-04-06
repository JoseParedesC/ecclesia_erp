// ============================================
// features/journalVoucher/components/VoucherStatusBadge.tsx
// ============================================

import React from 'react';
import type { VoucherStatus } from '../journalVoucher.types';
import { VOUCHER_STATUS_LABELS } from '../journalVoucher.types';
import styles from './VoucherStatusBadge.module.css';

interface Props { status: VoucherStatus }

export const VoucherStatusBadge: React.FC<Props> = ({ status }) => (
  <span className={`${styles.badge} ${styles[status.toLowerCase()]}`}>
    <span className={styles.dot} aria-hidden />
    {VOUCHER_STATUS_LABELS[status]}
  </span>
);
