// ============================================
// features/journalVoucher/components/VoucherTypeBadge.tsx
// ============================================

import React from 'react';
import { VOUCHER_TYPE_LABELS } from '../journalVoucher.types';
import type { VoucherType } from '../journalVoucher.types';
import styles from './VoucherTypeBadge.module.css';

interface Props { type: VoucherType }

export const VoucherTypeBadge: React.FC<Props> = ({ type }) => (
  <span className={`${styles.badge} ${styles[type.toLowerCase()]}`}>
    {VOUCHER_TYPE_LABELS[type]}
  </span>
);
