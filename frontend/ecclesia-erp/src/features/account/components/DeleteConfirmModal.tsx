// ============================================
// features/account/components/DeleteConfirmModal.tsx
// ============================================

import React from 'react';
import styles from './DeleteConfirmModal.module.css';

interface Props {
  open: boolean;
  title: string;
  description: string;
  isLoading: boolean;
  onConfirm: () => void;
  onCancel: () => void;
}

export const DeleteConfirmModal: React.FC<Props> = ({
  open, title, description, isLoading, onConfirm, onCancel,
}) => {
  if (!open) return null;

  return (
    <div className={styles.overlay} role="dialog" aria-modal aria-label="Confirmar eliminación">
      <div className={styles.modal}>
        <div className={styles.iconWrap} aria-hidden>
          <svg width="22" height="22" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth={1.8} strokeLinecap="round" strokeLinejoin="round">
            <path d="M10.29 3.86L1.82 18a2 2 0 001.71 3h16.94a2 2 0 001.71-3L13.71 3.86a2 2 0 00-3.42 0z" />
            <line x1="12" y1="9" x2="12" y2="13" />
            <line x1="12" y1="17" x2="12.01" y2="17" />
          </svg>
        </div>
        <h2 className={styles.title}>{title}</h2>
        <p className={styles.desc}>{description}</p>
        <div className={styles.actions}>
          <button className={styles.cancelBtn} onClick={onCancel} disabled={isLoading}>
            Cancelar
          </button>
          <button className={styles.confirmBtn} onClick={onConfirm} disabled={isLoading} aria-busy={isLoading}>
            {isLoading ? 'Eliminando…' : 'Sí, eliminar'}
          </button>
        </div>
      </div>
    </div>
  );
};
