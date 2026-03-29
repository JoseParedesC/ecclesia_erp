// ============================================
// features/account/components/AccountFormModal.tsx
// ============================================

import React, { useEffect } from 'react';
import { useForm } from 'react-hook-form';
import type { Account, AccountType, CreateAccountRequest } from '../account.types';
import styles from './AccountFormModal.module.css';

const TYPES: { value: AccountType; label: string }[] = [
  { value: 'ASSET',     label: 'Activo' },
  { value: 'LIABILITY', label: 'Pasivo' },
  { value: 'EQUITY',    label: 'Patrimonio' },
  { value: 'INCOME',    label: 'Ingreso' },
  { value: 'EXPENSE',   label: 'Gasto' },
];

interface Props {
  open: boolean;
  editing: Account | null;
  accounts: Account[];       // flat list for parent selector
  onClose: () => void;
  onSubmit: (data: CreateAccountRequest, id?: string) => void;
  isLoading: boolean;
  error?: string | null;
}

export const AccountFormModal: React.FC<Props> = ({
  open, editing, accounts, onClose, onSubmit, isLoading, error,
}) => {
  const {
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<CreateAccountRequest>({
    mode: 'onBlur',
  });

  useEffect(() => {
    if (open) {
      reset({
        code:            editing?.code ?? '',
        name:            editing?.name ?? '',
        type:            editing?.type ?? 'ASSET',
        parentAccountId: editing?.parentAccountId ?? '',
      });
    }
  }, [open, editing, reset]);

  const submit = handleSubmit((data) => {
    onSubmit(
      { ...data, parentAccountId: data.parentAccountId || null },
      editing?.id,
    );
  });

  if (!open) return null;

  // Filter out self and descendants from parent options
  const parentOptions = accounts.filter((a) => a.id !== editing?.id);

  return (
    <div className={styles.overlay} role="dialog" aria-modal aria-label={editing ? 'Editar cuenta' : 'Nueva cuenta'}>
      <div className={styles.modal}>
        {/* Header */}
        <div className={styles.modalHeader}>
          <div className={styles.modalTitle}>
            <div className={styles.modalIcon} aria-hidden>
              {editing ? (
                <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth={2} strokeLinecap="round" strokeLinejoin="round">
                  <path d="M11 4H4a2 2 0 00-2 2v14a2 2 0 002 2h14a2 2 0 002-2v-7" />
                  <path d="M18.5 2.5a2.121 2.121 0 013 3L12 15l-4 1 1-4 9.5-9.5z" />
                </svg>
              ) : (
                <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth={2} strokeLinecap="round" strokeLinejoin="round">
                  <line x1="12" y1="5" x2="12" y2="19" /><line x1="5" y1="12" x2="19" y2="12" />
                </svg>
              )}
            </div>
            <h2>{editing ? 'Editar cuenta' : 'Nueva cuenta'}</h2>
          </div>
          <button className={styles.closeBtn} onClick={onClose} aria-label="Cerrar">
            <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth={2} strokeLinecap="round">
              <line x1="18" y1="6" x2="6" y2="18" /><line x1="6" y1="6" x2="18" y2="18" />
            </svg>
          </button>
        </div>

        {/* Error */}
        {error && (
          <div className={styles.errorBanner} role="alert">
            <svg width="14" height="14" viewBox="0 0 16 16" fill="none">
              <circle cx="8" cy="8" r="7" stroke="var(--color-error)" strokeWidth="1.5" />
              <path d="M8 5v4M8 11v.5" stroke="var(--color-error)" strokeWidth="1.5" strokeLinecap="round" />
            </svg>
            {error}
          </div>
        )}

        {/* Form */}
        <form className={styles.form} onSubmit={submit} noValidate>
          <div className={styles.row}>
            {/* Code */}
            <div className={styles.field} style={{ flex: '0 0 140px' }}>
              <label className={styles.label} htmlFor="code">Código</label>
              <input
                id="code"
                className={`${styles.input} ${errors.code ? styles.inputErr : ''}`}
                placeholder="1.1.01"
                {...register('code', { required: 'Requerido' })}
              />
              {errors.code && <span className={styles.fieldErr}>{errors.code.message}</span>}
            </div>

            {/* Type */}
            <div className={styles.field} style={{ flex: 1 }}>
              <label className={styles.label} htmlFor="type">Tipo</label>
              <select
                id="type"
                className={`${styles.input} ${styles.select}`}
                {...register('type', { required: 'Requerido' })}
              >
                {TYPES.map((t) => (
                  <option key={t.value} value={t.value}>{t.label}</option>
                ))}
              </select>
            </div>
          </div>

          {/* Name */}
          <div className={styles.field}>
            <label className={styles.label} htmlFor="name">Nombre de la cuenta</label>
            <input
              id="name"
              className={`${styles.input} ${errors.name ? styles.inputErr : ''}`}
              placeholder="Ej: Caja general"
              {...register('name', { required: 'Requerido', minLength: { value: 2, message: 'Mínimo 2 caracteres' } })}
            />
            {errors.name && <span className={styles.fieldErr}>{errors.name.message}</span>}
          </div>

          {/* Parent */}
          <div className={styles.field}>
            <label className={styles.label} htmlFor="parentAccountId">Cuenta padre <span className={styles.optional}>(opcional)</span></label>
            <select
              id="parentAccountId"
              className={`${styles.input} ${styles.select}`}
              {...register('parentAccountId')}
            >
              <option value="">— Sin padre (cuenta raíz) —</option>
              {parentOptions
                .sort((a, b) => a.code.localeCompare(b.code, undefined, { numeric: true }))
                .map((a) => (
                  <option key={a.id} value={a.id}>
                    {a.code} — {a.name}
                  </option>
                ))}
            </select>
          </div>

          {/* Actions */}
          <div className={styles.actions}>
            <button type="button" className={styles.cancelBtn} onClick={onClose}>
              Cancelar
            </button>
            <button type="submit" className={styles.submitBtn} disabled={isLoading} aria-busy={isLoading}>
              {isLoading ? (
                <><span className={styles.spinner} aria-hidden /> Guardando…</>
              ) : (
                editing ? 'Guardar cambios' : 'Crear cuenta'
              )}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};
