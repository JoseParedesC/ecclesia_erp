// ============================================
// features/account/components/AccountTypeBadge.tsx
// ============================================

import React from 'react';
import type { AccountType } from '../account.types';
import styles from './AccountTypeBadge.module.css';

const LABELS: Record<AccountType, string> = {
  ASSET:     'Activo',
  LIABILITY: 'Pasivo',
  EQUITY:    'Patrimonio',
  INCOME:    'Ingreso',
  EXPENSE:   'Gasto',
};

interface Props {
  type: AccountType;
}

export const AccountTypeBadge: React.FC<Props> = ({ type }) => (
  <span className={`${styles.badge} ${styles[type.toLowerCase()]}`}>
    {LABELS[type]}
  </span>
);
