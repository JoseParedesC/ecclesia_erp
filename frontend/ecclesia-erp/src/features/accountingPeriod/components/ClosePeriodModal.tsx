// ============================================
// features/accountingPeriod/components/ClosePeriodModal.tsx
// ============================================

import React from 'react';
import type { AccountingPeriod } from '../accountingPeriod.types';
import { formatPeriod } from '../utils/periodUtils';
import styles from './ClosePeriodModal.module.css';

interface Props {
  open: boolean;
  period: AccountingPeriod | null;
  isLoading: boolean;
  onConfirm: () => void;
  onCancel: () => void;
}

export const ClosePeriodModal: React.FC<Props> = ({
  open, period, isLoading, onConfirm, onCancel,
}) => {
  if (!open || !period) return null;

  return (
    <div className={styles.overlay} role="dialog" aria-modal aria-label="Cerrar período">
      <div className={styles.modal}>
        <div className={styles.iconWrap} aria-hidden>
          <svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth={1.6} strokeLinecap="round" strokeLinejoin="round">
            <rect x="3" y="11" width="18" height="11" rx="2" ry="2" />
            <path d="M7 11V7a5 5 0 0110 0v4" />
          </svg>
        </div>

        <h2 className={styles.title}>Cerrar período contable</h2>
        <p className={styles.period}>{formatPeriod(period.year, period.month)} {period.year}</p>

        <div className={styles.warningBox}>
          <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth={2} strokeLinecap="round" strokeLinejoin="round" aria-hidden>
            <path d="M10.29 3.86L1.82 18a2 2 0 001.71 3h16.94a2 2 0 001.71-3L13.71 3.86a2 2 0 00-3.42 0z" />
            <line x1="12" y1="9" x2="12" y2="13" /><line x1="12" y1="17" x2="12.01" y2="17" />
          </svg>
          <p>Esta acción es <strong>irreversible</strong>. Una vez cerrado el período no se podrán registrar ni modificar asientos contables en él.</p>
        </div>

        <div className={styles.actions}>
          <button className={styles.cancelBtn} onClick={onCancel} disabled={isLoading}>
            Cancelar
          </button>
          <button className={styles.confirmBtn} onClick={onConfirm} disabled={isLoading} aria-busy={isLoading}>
            {isLoading
              ? <><span className={styles.spinner} aria-hidden />Cerrando…</>
              : <>
                  <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth={2} strokeLinecap="round" strokeLinejoin="round" aria-hidden>
                    <rect x="3" y="11" width="18" height="11" rx="2" ry="2" />
                    <path d="M7 11V7a5 5 0 0110 0v4" />
                  </svg>
                  Sí, cerrar período
                </>
            }
          </button>
        </div>
      </div>
    </div>
  );
};
