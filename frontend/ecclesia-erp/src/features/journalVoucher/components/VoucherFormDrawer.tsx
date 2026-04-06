// ============================================
// features/journalVoucher/components/VoucherFormDrawer.tsx
// ============================================

import React, { useState, useEffect } from 'react';
import { useForm } from 'react-hook-form';
import { isBalanced, VOUCHER_TYPE_LABELS } from '../journalVoucher.types';
import type { VoucherType, JournalVoucherLineInput, CreateVoucherRequest } from '../journalVoucher.types';
import { LinesEditor }    from './LinesEditor';
import type { Account }        from '../../account/account.types';
import type { AccountingPeriod } from '../../accountingPeriod/accountingPeriod.types';
// import { Rostro }         from '../../rostro/rostro.types';
// import { Community }      from '../../community/community.types';
import styles from './VoucherFormDrawer.module.css';

interface HeaderForm {
  type:               VoucherType;
  date:               string;
  description:        string;
  accountingPeriodId: string;
  rostroId:           string;
  communityId:        string;
}

const TYPES: VoucherType[] = ['INCOME', 'EXPENSE', 'ADJUSTMENT', 'REVERSAL'];

interface Props {
  open:      boolean;
  accounts:  Account[];
  periods:   AccountingPeriod[];
  // rostros:   Rostro[];
  // communities: Community[];
  rostros:   any;
  communities: any;
  isLoading: boolean;
  error:     string | null;
  onClose:   () => void;
  onSubmit:  (data: CreateVoucherRequest) => void;
}

export const VoucherFormDrawer: React.FC<Props> = ({
  open, accounts, periods, rostros, communities,
  isLoading, error, onClose, onSubmit,
}) => {
  const [lines, setLines] = useState<JournalVoucherLineInput[]>([]);
  const [linesError, setLinesError] = useState<string | null>(null);

  const { register, handleSubmit, watch, reset, formState: { errors } } =
    useForm<HeaderForm>({
      mode: 'onBlur',
      defaultValues: {
        type:               'INCOME',
        date:               new Date().toISOString().slice(0, 10),
        description:        '',
        accountingPeriodId: '',
        rostroId:           '',
        communityId:        '',
      },
    });

  const selectedRostroId = watch('rostroId');
  const filteredCommunities = selectedRostroId
    ? communities.filter(c => c.rostroId === selectedRostroId)
    : communities;

  useEffect(() => {
    if (open) { reset(); setLines([]); setLinesError(null); }
  }, [open, reset]);

  const submit = handleSubmit((header) => {
    setLinesError(null);

    if (lines.length < 2) {
      setLinesError('El asiento debe tener al menos 2 líneas (1 débito + 1 crédito).');
      return;
    }
    if (lines.some(l => !l.accountId)) {
      setLinesError('Todas las líneas deben tener una cuenta seleccionada.');
      return;
    }
    if (lines.some(l => l.amount <= 0)) {
      setLinesError('Todos los montos deben ser mayores que cero.');
      return;
    }
    if (!isBalanced(lines)) {
      setLinesError('El asiento no está balanceado. La suma de débitos debe ser igual a la suma de créditos.');
      return;
    }

    onSubmit({ ...header, description: header.description || null, lines });
  });

  const selectStyle: React.CSSProperties = {
    width: '100%', height: 40, padding: '0 2rem 0 0.75rem',
    background: 'var(--color-deep)', border: '1px solid var(--color-border)',
    borderRadius: 'var(--radius-sm)', color: 'var(--color-text-primary)',
    fontFamily: 'var(--font-body)', fontSize: '0.875rem', fontWeight: 300,
    outline: 'none', appearance: 'none', cursor: 'pointer',
    backgroundImage: "url(\"data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' width='12' height='12' viewBox='0 0 24 24' fill='none' stroke='%23505870' stroke-width='2' stroke-linecap='round'%3E%3Cpath d='M6 9l6 6 6-6'/%3E%3C/svg%3E\")",
    backgroundRepeat: 'no-repeat',
    backgroundPosition: 'right 0.7rem center',
    transition: 'border-color 0.15s',
  };

  const openPeriods = periods.filter(p => p.status === 'OPEN');

  return (
    <>
      {open && <div className={styles.backdrop} onClick={onClose} aria-hidden />}
      <aside className={`${styles.drawer} ${open ? styles.open : ''}`} role="dialog" aria-modal aria-label="Nuevo asiento contable">

        {/* Header */}
        <div className={styles.header}>
          <div className={styles.headerLeft}>
            <div className={styles.icon} aria-hidden>
              <svg width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth={2} strokeLinecap="round" strokeLinejoin="round">
                <path d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z" />
              </svg>
            </div>
            <div>
              <h2 className={styles.headerTitle}>Nuevo asiento</h2>
              <p className={styles.headerSub}>Partida doble — débitos = créditos</p>
            </div>
          </div>
          <button className={styles.closeBtn} onClick={onClose} aria-label="Cerrar">
            <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth={2} strokeLinecap="round">
              <line x1="18" y1="6" x2="6" y2="18" /><line x1="6" y1="6" x2="18" y2="18" />
            </svg>
          </button>
        </div>

        {/* Body */}
        <div className={styles.body}>
          {(error || linesError) && (
            <div className={styles.errorBanner} role="alert">
              <svg width="13" height="13" viewBox="0 0 16 16" fill="none">
                <circle cx="8" cy="8" r="7" stroke="var(--color-error)" strokeWidth="1.5" />
                <path d="M8 5v4M8 11v.5" stroke="var(--color-error)" strokeWidth="1.5" strokeLinecap="round" />
              </svg>
              {linesError ?? error}
            </div>
          )}

          <form id="voucher-form" onSubmit={submit} noValidate>
            {/* Row 1: Type + Date */}
            <div className={styles.row}>
              <div className={styles.field}>
                <label className={styles.label} htmlFor="v-type">Tipo de asiento</label>
                <select id="v-type" style={selectStyle} {...register('type', { required: true })}>
                  {TYPES.map(t => <option key={t} value={t}>{VOUCHER_TYPE_LABELS[t]}</option>)}
                </select>
              </div>
              <div className={styles.field}>
                <label className={styles.label} htmlFor="v-date">Fecha</label>
                <input
                  id="v-date"
                  type="date"
                  className={`${styles.input} ${errors.date ? styles.inputErr : ''}`}
                  {...register('date', { required: 'Requerido' })}
                />
                {errors.date && <span className={styles.fieldErr}>{errors.date.message}</span>}
              </div>
            </div>

            {/* Row 2: Period */}
            <div className={styles.field}>
              <label className={styles.label} htmlFor="v-period">Período contable</label>
              <select
                id="v-period"
                style={{ ...selectStyle, borderColor: errors.accountingPeriodId ? 'rgba(224,82,82,0.45)' : undefined }}
                {...register('accountingPeriodId', { required: 'Requerido' })}
              >
                <option value="">— Seleccionar período abierto —</option>
                {openPeriods.map(p => (
                  <option key={p.id} value={p.id}>
                    {new Date(0, p.month - 1).toLocaleString('es', { month: 'long' })} {p.year}
                  </option>
                ))}
              </select>
              {errors.accountingPeriodId && <span className={styles.fieldErr}>{errors.accountingPeriodId.message}</span>}
            </div>

            {/* Row 3: Rostro + Community */}
            <div className={styles.row}>
              <div className={styles.field}>
                <label className={styles.label} htmlFor="v-rostro">Rostro</label>
                <select
                  id="v-rostro"
                  style={{ ...selectStyle, borderColor: errors.rostroId ? 'rgba(224,82,82,0.45)' : undefined }}
                  {...register('rostroId', { required: 'Requerido' })}
                >
                  <option value="">— Seleccionar —</option>
                  {rostros.map(r => <option key={r.id} value={r.id}>{r.name}</option>)}
                </select>
                {errors.rostroId && <span className={styles.fieldErr}>{errors.rostroId.message}</span>}
              </div>
              <div className={styles.field}>
                <label className={styles.label} htmlFor="v-community">Comunidad</label>
                <select
                  id="v-community"
                  style={{ ...selectStyle, borderColor: errors.communityId ? 'rgba(224,82,82,0.45)' : undefined }}
                  {...register('communityId', { required: 'Requerido' })}
                >
                  <option value="">— Seleccionar —</option>
                  {filteredCommunities.map(c => <option key={c.id} value={c.id}>{c.name}</option>)}
                </select>
                {errors.communityId && <span className={styles.fieldErr}>{errors.communityId.message}</span>}
              </div>
            </div>

            {/* Description */}
            <div className={styles.field}>
              <label className={styles.label} htmlFor="v-desc">
                Descripción <span className={styles.optional}>(opcional)</span>
              </label>
              <textarea
                id="v-desc"
                className={styles.textarea}
                rows={2}
                placeholder="Descripción o referencia del asiento…"
                {...register('description')}
              />
            </div>

            {/* Section divider */}
            <div className={styles.sectionTitle}>
              <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth={2} strokeLinecap="round" strokeLinejoin="round" aria-hidden>
                <line x1="8" y1="6" x2="21" y2="6" />
                <line x1="8" y1="12" x2="21" y2="12" />
                <line x1="8" y1="18" x2="21" y2="18" />
                <line x1="3" y1="6"  x2="3.01" y2="6" />
                <line x1="3" y1="12" x2="3.01" y2="12" />
                <line x1="3" y1="18" x2="3.01" y2="18" />
              </svg>
              Líneas del asiento (partida doble)
            </div>

            {/* Lines editor */}
            <LinesEditor lines={lines} accounts={accounts} onChange={setLines} />
          </form>
        </div>

        {/* Footer */}
        <div className={styles.footer}>
          <button type="button" className={styles.cancelBtn} onClick={onClose}>Cancelar</button>
          <button type="submit" form="voucher-form" className={styles.submitBtn} disabled={isLoading} aria-busy={isLoading}>
            {isLoading
              ? <><span className={styles.spinner} aria-hidden />Guardando…</>
              : <>
                  <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth={2.5} strokeLinecap="round" aria-hidden>
                    <path d="M19 21H5a2 2 0 01-2-2V5a2 2 0 012-2h11l5 5v11a2 2 0 01-2 2z" />
                    <polyline points="17 21 17 13 7 13 7 21" />
                    <polyline points="7 3 7 8 15 8" />
                  </svg>
                  Guardar borrador
                </>
            }
          </button>
        </div>
      </aside>
    </>
  );
};
