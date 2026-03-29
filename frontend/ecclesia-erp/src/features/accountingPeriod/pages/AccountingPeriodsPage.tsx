// ============================================
// features/accountingPeriod/pages/AccountingPeriodsPage.tsx
// ============================================

import React, { useState, useMemo } from 'react';
import {
  useAccountingPeriods,
  useCreatePeriod,
  useClosePeriod,
} from '../hooks/useAccountingPeriods';
import { CreatePeriodModal }  from '../components/CreatePeriodModal';
import { ClosePeriodModal }   from '../components/ClosePeriodModal';
import { PeriodStatusBadge }  from '../components/PeriodStatusBadge';
import type { AccountingPeriod }   from '../accountingPeriod.types';
import { formatPeriod, nextPeriod, sortPeriods } from '../utils/periodUtils';
import styles from './AccountingPeriodsPage.module.css';

export const AccountingPeriodsPage: React.FC = () => {
  const { data: raw = [], isLoading, isError } = useAccountingPeriods();
  const createMut = useCreatePeriod();
  const closeMut  = useClosePeriod();

  const [createOpen, setCreateOpen]     = useState(false);
  const [closingPeriod, setClosingPeriod] = useState<AccountingPeriod | null>(null);
  const [createError, setCreateError]   = useState<string | null>(null);

  const periods  = useMemo(() => sortPeriods(raw), [raw]);
  const next     = useMemo(() => nextPeriod(raw), [raw]);
  const openCount  = periods.filter((p) => p.status === 'OPEN').length;
  const closedCount = periods.filter((p) => p.status === 'CLOSED').length;
  const currentPeriod = periods.find((p) => p.status === 'OPEN') ?? null;

  const handleCreate = (data: { year: number; month: number }) => {
    setCreateError(null);
    createMut.mutate(data, {
      onSuccess: () => setCreateOpen(false),
      onError:   (e) => setCreateError(e.message),
    });
  };

  const handleClose = () => {
    if (!closingPeriod) return;
    closeMut.mutate(closingPeriod.id, {
      onSuccess: () => setClosingPeriod(null),
      onError:   (e) => { setClosingPeriod(null); alert(e.message); },
    });
  };

  return (
    <div className={styles.page}>

      {/* ── Header ── */}
      <div className={styles.pageHeader}>
        <div className={styles.headerLeft}>
          <h1 className={styles.title}>Períodos contables</h1>
          <p className={styles.subtitle}>Control del ciclo contable mensual</p>
        </div>
        <button className={styles.newBtn} onClick={() => { setCreateError(null); setCreateOpen(true); }}>
          <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth={2.5} strokeLinecap="round" aria-hidden>
            <line x1="12" y1="5" x2="12" y2="19" /><line x1="5" y1="12" x2="19" y2="12" />
          </svg>
          Abrir período
        </button>
      </div>

      {/* ── Summary cards ── */}
      <div className={styles.summaryGrid}>
        <div className={styles.summaryCard}>
          <span className={styles.summaryLabel}>Período activo</span>
          <span className={styles.summaryValue}>
            {currentPeriod
              ? formatPeriod(currentPeriod.year, currentPeriod.month)
              : '—'
            }
          </span>
          {currentPeriod && <PeriodStatusBadge status="OPEN" />}
        </div>
        <div className={styles.summaryCard}>
          <span className={styles.summaryLabel}>Períodos abiertos</span>
          <span className={styles.summaryValue}>{isLoading ? '…' : openCount}</span>
          <span className={styles.summaryHint}>Permiten nuevos asientos</span>
        </div>
        <div className={styles.summaryCard}>
          <span className={styles.summaryLabel}>Períodos cerrados</span>
          <span className={styles.summaryValue}>{isLoading ? '…' : closedCount}</span>
          <span className={styles.summaryHint}>Solo lectura</span>
        </div>
        <div className={styles.summaryCard}>
          <span className={styles.summaryLabel}>Total períodos</span>
          <span className={styles.summaryValue}>{isLoading ? '…' : periods.length}</span>
          <span className={styles.summaryHint}>Histórico completo</span>
        </div>
      </div>

      {/* ── Table ── */}
      <div className={styles.tableWrap}>
        {isLoading ? (
          <div className={styles.stateBox}>
            <span className={styles.spinner} aria-label="Cargando" />
            <p>Cargando períodos…</p>
          </div>
        ) : isError ? (
          <div className={styles.stateBox}>
            <svg width="30" height="30" viewBox="0 0 24 24" fill="none" stroke="var(--color-error)" strokeWidth={1.5} strokeLinecap="round" aria-hidden>
              <circle cx="12" cy="12" r="10" /><line x1="12" y1="8" x2="12" y2="12" /><line x1="12" y1="16" x2="12.01" y2="16" />
            </svg>
            <p>Error al cargar los períodos.</p>
          </div>
        ) : periods.length === 0 ? (
          <div className={styles.stateBox}>
            <svg width="36" height="36" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth={1.2} strokeLinecap="round" aria-hidden>
              <rect x="3" y="4" width="18" height="18" rx="2" />
              <line x1="16" y1="2" x2="16" y2="6" />
              <line x1="8"  y1="2" x2="8"  y2="6" />
              <line x1="3"  y1="10" x2="21" y2="10" />
            </svg>
            <p>No hay períodos registrados.</p>
            <button className={styles.emptyAction} onClick={() => setCreateOpen(true)}>
              Abrir primer período
            </button>
          </div>
        ) : (
          <table className={styles.table}>
            <thead>
              <tr className={styles.thead}>
                <th className={styles.th}>Período</th>
                <th className={styles.th}>Estado</th>
                <th className={styles.th}>Fecha de cierre</th>
                <th className={styles.th}>Cerrado por</th>
                <th className={styles.th}>Creado</th>
                <th className={styles.th} style={{ textAlign: 'right' }}>Acciones</th>
              </tr>
            </thead>
            <tbody>
              {periods.map((p) => (
                <tr
                  key={p.id}
                  className={`${styles.row} ${p.status === 'OPEN' ? styles.rowOpen : styles.rowClosed}`}
                >
                  <td className={styles.td}>
                    <div className={styles.periodCell}>
                      {p.status === 'OPEN' && <div className={styles.activeDot} aria-hidden />}
                      <span className={`${styles.periodName} ${p.status === 'OPEN' ? styles.periodNameOpen : ''}`}>
                        {formatPeriod(p.year, p.month)}
                      </span>
                      <span className={styles.periodYear}>{p.year}</span>
                    </div>
                  </td>

                  <td className={styles.td}>
                    <PeriodStatusBadge status={p.status} />
                  </td>

                  <td className={styles.td}>
                    <span className={styles.meta}>
                      {p.closedAt
                        ? new Date(p.closedAt).toLocaleDateString('es-CO', { day: '2-digit', month: 'short', year: 'numeric' })
                        : <span className={styles.none}>—</span>
                      }
                    </span>
                  </td>

                  <td className={styles.td}>
                    <span className={styles.meta}>{p.closedBy ?? <span className={styles.none}>—</span>}</span>
                  </td>

                  <td className={styles.td}>
                    <span className={styles.meta}>
                      {new Date(p.createdAt).toLocaleDateString('es-CO', { day: '2-digit', month: 'short', year: 'numeric' })}
                    </span>
                  </td>

                  <td className={styles.td} style={{ textAlign: 'right' }}>
                    {p.status === 'OPEN' ? (
                      <button
                        className={styles.closeBtn}
                        onClick={() => setClosingPeriod(p)}
                        aria-label={`Cerrar período ${formatPeriod(p.year, p.month)}`}
                      >
                        <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth={2} strokeLinecap="round" strokeLinejoin="round" aria-hidden>
                          <rect x="3" y="11" width="18" height="11" rx="2" ry="2" />
                          <path d="M7 11V7a5 5 0 0110 0v4" />
                        </svg>
                        Cerrar
                      </button>
                    ) : (
                      <span className={styles.closedTag}>Cerrado</span>
                    )}
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </div>

      {/* ── Modals ── */}
      <CreatePeriodModal
        open={createOpen}
        defaultYear={next.year}
        defaultMonth={next.month}
        isLoading={createMut.isPending}
        error={createError}
        onClose={() => setCreateOpen(false)}
        onSubmit={handleCreate}
      />

      <ClosePeriodModal
        open={!!closingPeriod}
        period={closingPeriod}
        isLoading={closeMut.isPending}
        onConfirm={handleClose}
        onCancel={() => setClosingPeriod(null)}
      />
    </div>
  );
};
