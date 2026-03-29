// ============================================
// features/accountingPeriod/components/CreatePeriodModal.tsx
// ============================================

import React, { useEffect } from 'react';
import { useForm } from 'react-hook-form';
import type { CreatePeriodRequest } from '../accountingPeriod.types';
import { MONTH_NAMES } from '../utils/periodUtils';
import styles from './CreatePeriodModal.module.css';

interface Props {
  open: boolean;
  defaultYear: number;
  defaultMonth: number;
  isLoading: boolean;
  error: string | null;
  onClose: () => void;
  onSubmit: (data: CreatePeriodRequest) => void;
}

const currentYear = new Date().getFullYear();
const YEARS = Array.from({ length: 5 }, (_, i) => currentYear - 1 + i);

export const CreatePeriodModal: React.FC<Props> = ({
  open, defaultYear, defaultMonth, isLoading, error, onClose, onSubmit,
}) => {
  const { register, handleSubmit, reset, formState: { errors } } =
    useForm<CreatePeriodRequest>({ mode: 'onBlur' });

  useEffect(() => {
    if (open) reset({ year: defaultYear, month: defaultMonth });
  }, [open, defaultYear, defaultMonth, reset]);

  if (!open) return null;

  return (
    <div className={styles.overlay} role="dialog" aria-modal aria-label="Nuevo período contable">
      <div className={styles.modal}>
        <div className={styles.header}>
          <div className={styles.titleRow}>
            <div className={styles.icon} aria-hidden>
              <svg width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth={2} strokeLinecap="round" strokeLinejoin="round">
                <rect x="3" y="4" width="18" height="18" rx="2" ry="2" />
                <line x1="16" y1="2" x2="16" y2="6" />
                <line x1="8"  y1="2" x2="8"  y2="6" />
                <line x1="3"  y1="10" x2="21" y2="10" />
              </svg>
            </div>
            <h2>Abrir nuevo período</h2>
          </div>
          <button className={styles.closeBtn} onClick={onClose} aria-label="Cerrar">
            <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth={2} strokeLinecap="round">
              <line x1="18" y1="6" x2="6" y2="18" /><line x1="6" y1="6" x2="18" y2="18" />
            </svg>
          </button>
        </div>

        {error && (
          <div className={styles.errorBanner} role="alert">
            <svg width="13" height="13" viewBox="0 0 16 16" fill="none">
              <circle cx="8" cy="8" r="7" stroke="var(--color-error)" strokeWidth="1.5" />
              <path d="M8 5v4M8 11v.5" stroke="var(--color-error)" strokeWidth="1.5" strokeLinecap="round" />
            </svg>
            {error}
          </div>
        )}

        <form className={styles.form} onSubmit={handleSubmit(onSubmit)} noValidate>
          <div className={styles.row}>
            <div className={styles.field}>
              <label className={styles.label} htmlFor="month">Mes</label>
              <select
                id="month"
                className={`${styles.input} ${styles.select} ${errors.month ? styles.inputErr : ''}`}
                {...register('month', { required: true, valueAsNumber: true })}
              >
                {MONTH_NAMES.map((name, i) => (
                  <option key={i + 1} value={i + 1}>{name}</option>
                ))}
              </select>
            </div>

            <div className={styles.field}>
              <label className={styles.label} htmlFor="year">Año</label>
              <select
                id="year"
                className={`${styles.input} ${styles.select} ${errors.year ? styles.inputErr : ''}`}
                {...register('year', { required: true, valueAsNumber: true })}
              >
                {YEARS.map((y) => (
                  <option key={y} value={y}>{y}</option>
                ))}
              </select>
            </div>
          </div>

          <p className={styles.hint}>
            Al abrir el período quedará en estado <strong>Abierto</strong> y permitirá registrar asientos contables.
          </p>

          <div className={styles.actions}>
            <button type="button" className={styles.cancelBtn} onClick={onClose}>Cancelar</button>
            <button type="submit" className={styles.submitBtn} disabled={isLoading} aria-busy={isLoading}>
              {isLoading
                ? <><span className={styles.spinner} aria-hidden />Abriendo…</>
                : 'Abrir período'
              }
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};
