// ============================================
// features/journalVoucher/pages/JournalVouchersPage.tsx
// ============================================

import React, { useState } from 'react';
import {
  useJournalVouchers, useCreateVoucher,
  usePostVoucher, useCancelVoucher, useJournalVoucherById,
} from '../hooks/useJournalVouchers';
import { VoucherFormDrawer }   from '../components/VoucherFormDrawer';
import { VoucherDetailModal }  from '../components/VoucherDetailModal';
import { VoucherStatusBadge }  from '../components/VoucherStatusBadge';
import { VoucherTypeBadge }    from '../components/VoucherTypeBadge';
import { Pagination }          from '../../../shared/components/Pagination';
import { useDebounce }         from '../../../shared/hooks/useDebounce';
import { useAccounts }         from '../../account/hook/useAccounts';
import { useAccountingPeriods } from '../../accountingPeriod/hooks/useAccountingPeriods';
// import { useRostros }          from '../../rostro/hooks/useRostro';
// import { useCommunities }      from '../../community/hooks/useCommunity';
import type {
  VoucherType, VoucherStatus,
  CreateVoucherRequest,
} from '../journalVoucher.types';
import styles from './JournalVouchersPage.module.css';

const PAGE_SIZE = 15;
const fmt = (n: number) =>
  new Intl.NumberFormat('es-CO', { minimumFractionDigits: 2, maximumFractionDigits: 2 }).format(n);

const TYPE_OPTS: { value: VoucherType | ''; label: string }[] = [
  { value: '',           label: 'Todos los tipos' },
  { value: 'INCOME',     label: 'Ingreso' },
  { value: 'EXPENSE',    label: 'Gasto' },
  { value: 'ADJUSTMENT', label: 'Ajuste' },
  { value: 'REVERSAL',   label: 'Reverso' },
];

const STATUS_OPTS: { value: VoucherStatus | ''; label: string }[] = [
  { value: '',          label: 'Todos los estados' },
  { value: 'DRAFT',     label: 'Borrador' },
  { value: 'POSTED',    label: 'Publicado' },
  { value: 'CANCELLED', label: 'Cancelado' },
];

export const JournalVouchersPage: React.FC = () => {
  // ── Filters ──
  const [search,   setSearch]   = useState('');
  const [type,     setType]     = useState<VoucherType | ''>('');
  const [status,   setStatus]   = useState<VoucherStatus | ''>('');
  const [periodId, setPeriodId] = useState('');
  const [page,     setPage]     = useState(1);
  const debouncedSearch = useDebounce(search, 400);

  // ── Data ──
  const { data, isLoading, isError, isFetching } = useJournalVouchers({
    page, pageSize: PAGE_SIZE,
    search: debouncedSearch, type, status, periodId,
  });
  const { data: accounts   = [] } = useAccounts();
  const { data: periods    = [] } = useAccountingPeriods();
  // const { data: rostros    = [] } = useRostros();
  // const { data: communities= [] } = useCommunities();

  // ── Mutations ──
  const createMut = useCreateVoucher();
  const postMut   = usePostVoucher();
  const cancelMut = useCancelVoucher();

  // ── UI State ──
  const [drawerOpen,   setDrawerOpen]   = useState(false);
  const [viewingId,    setViewingId]    = useState<string | null>(null);
  const [drawerError,  setDrawerError]  = useState<string | null>(null);

  const { data: viewing } = useJournalVoucherById(viewingId ?? '');

  const vouchers   = data?.items      ?? [];
  const totalPages = data?.totalPages ?? 1;
  const total      = data?.totalCount ?? 0;

  // ── Handlers ──
  const handleCreate = (req: CreateVoucherRequest) => {
    setDrawerError(null);
    createMut.mutate(req, {
      onSuccess: () => { setDrawerOpen(false); },
      onError:   e  => setDrawerError(e.message),
    });
  };

  const handlePost = (id: string) => {
    postMut.mutate(id, { onSuccess: () => setViewingId(null) });
  };

  const handleCancel = (id: string) => {
    if (!window.confirm('¿Cancelar este asiento? Solo puede cancelarse si está en borrador.')) return;
    cancelMut.mutate(id, { onSuccess: () => setViewingId(null) });
  };

  return (
    <div className={styles.page}>

      {/* ── Header ── */}
      <div className={styles.pageHeader}>
        <div>
          <h1 className={styles.title}>Asientos contables</h1>
          <p className={styles.subtitle}>Registro de partida doble · débitos = créditos</p>
        </div>
        <button className={styles.newBtn} onClick={() => { setDrawerError(null); setDrawerOpen(true); }}>
          <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth={2.5} strokeLinecap="round" aria-hidden>
            <line x1="12" y1="5" x2="12" y2="19" /><line x1="5" y1="12" x2="19" y2="12" />
          </svg>
          Nuevo asiento
        </button>
      </div>

      {/* ── Filters ── */}
      <div className={styles.filters}>
        <div className={styles.searchWrap}>
          <svg className={styles.searchIcon} width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth={2} strokeLinecap="round" aria-hidden>
            <circle cx="11" cy="11" r="8" /><line x1="21" y1="21" x2="16.65" y2="16.65" />
          </svg>
          <input
            className={styles.searchInput}
            type="search"
            placeholder="Buscar por número, descripción…"
            value={search}
            onChange={e => { setSearch(e.target.value); setPage(1); }}
          />
          {isFetching && !isLoading && <span className={styles.fetching} aria-hidden />}
        </div>

        {[
          { value: type,     opts: TYPE_OPTS,   set: (v: string) => { setType(v as VoucherType); setPage(1); } },
          { value: status,   opts: STATUS_OPTS, set: (v: string) => { setStatus(v as VoucherStatus); setPage(1); } },
        ].map((f, i) => (
          <select key={i} className={styles.filterSelect} value={f.value} onChange={e => f.set(e.target.value)}>
            {f.opts.map(o => <option key={o.value} value={o.value}>{o.label}</option>)}
          </select>
        ))}

        <select className={styles.filterSelect} value={periodId} onChange={e => { setPeriodId(e.target.value); setPage(1); }}>
          <option value="">Todos los períodos</option>
          {[...periods].sort((a,b) => b.year-a.year || b.month-a.month).map(p => (
            <option key={p.id} value={p.id}>
              {new Date(0, p.month-1).toLocaleString('es',{month:'long'})} {p.year}
            </option>
          ))}
        </select>

        <span className={styles.total}>{isLoading ? '…' : total} asiento{total !== 1 ? 's' : ''}</span>
      </div>

      {/* ── Table ── */}
      <div className={styles.tableWrap}>
        {isLoading ? (
          <div className={styles.state}><span className={styles.spinner} /><p>Cargando asientos…</p></div>
        ) : isError ? (
          <div className={styles.state}><p style={{ color: 'var(--color-error)' }}>Error al cargar los asientos.</p></div>
        ) : vouchers.length === 0 ? (
          <div className={styles.state}>
            <svg width="36" height="36" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth={1.2} strokeLinecap="round" aria-hidden>
              <path d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z" />
            </svg>
            <p>{search || type || status || periodId ? 'Sin resultados para los filtros.' : 'No hay asientos registrados.'}</p>
            {!search && !type && !status && !periodId && (
              <button className={styles.emptyAction} onClick={() => setDrawerOpen(true)}>Crear primer asiento</button>
            )}
          </div>
        ) : (
          <>
            <table className={styles.table}>
              <thead>
                <tr className={styles.thead}>
                  <th className={styles.th}>Número</th>
                  <th className={styles.th}>Tipo</th>
                  <th className={styles.th}>Estado</th>
                  <th className={styles.th}>Fecha</th>
                  <th className={styles.th}>Comunidad</th>
                  <th className={styles.th} style={{ textAlign: 'right' }}>Débito</th>
                  <th className={styles.th} style={{ textAlign: 'right' }}>Crédito</th>
                  <th className={styles.th} style={{ textAlign: 'right' }}>Acciones</th>
                </tr>
              </thead>
              <tbody>
                {vouchers.map(v => (
                  <tr key={v.id} className={`${styles.row} ${v.status === 'POSTED' ? styles.rowPosted : v.status === 'CANCELLED' ? styles.rowCancelled : ''}`}>
                    <td className={styles.td}>
                      <button className={styles.voucherLink} onClick={() => setViewingId(v.id)}>
                        {v.voucherNumber}
                      </button>
                    </td>
                    <td className={styles.td}><VoucherTypeBadge type={v.type} /></td>
                    <td className={styles.td}><VoucherStatusBadge status={v.status} /></td>
                    <td className={styles.td}>
                      <span className={styles.meta}>
                        {new Date(v.date + 'T00:00:00').toLocaleDateString('es-CO', { day:'2-digit', month:'short', year:'numeric' })}
                      </span>
                    </td>
                    <td className={styles.td}><span className={styles.meta}>{v.communityName}</span></td>
                    <td className={styles.td} style={{ textAlign: 'right' }}>
                      <span className={styles.debit}>{fmt(v.totalDebit)}</span>
                    </td>
                    <td className={styles.td} style={{ textAlign: 'right' }}>
                      <span className={styles.credit}>{fmt(v.totalCredit)}</span>
                    </td>
                    <td className={styles.td} style={{ textAlign: 'right' }}>
                      <div className={styles.rowActions}>
                        <button className={styles.viewBtn} onClick={() => setViewingId(v.id)} title="Ver detalle">
                          <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth={2} strokeLinecap="round" strokeLinejoin="round">
                            <path d="M1 12s4-8 11-8 11 8 11 8-4 8-11 8-11-8-11-8z" />
                            <circle cx="12" cy="12" r="3" />
                          </svg>
                        </button>
                        {v.status === 'DRAFT' && (
                          <button className={styles.postBtn} onClick={() => handlePost(v.id)} disabled={postMut.isPending} title="Publicar">
                            <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth={2.5} strokeLinecap="round" strokeLinejoin="round">
                              <polyline points="20 6 9 17 4 12" />
                            </svg>
                          </button>
                        )}
                      </div>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
            <Pagination page={page} totalPages={totalPages} total={total} pageSize={PAGE_SIZE} onPageChange={setPage} />
          </>
        )}
      </div>

      {/* ── Drawer ── */}
      <VoucherFormDrawer
        open={drawerOpen}
        accounts={accounts}
        periods={periods}
        // rostros={rostros}
        // communities={communities}
        rostros={[]}
        communities={[]}
        isLoading={createMut.isPending}
        error={drawerError}
        onClose={() => setDrawerOpen(false)}
        onSubmit={handleCreate}
      />

      {/* ── Detail modal ── */}
      <VoucherDetailModal
        open={!!viewingId && !!viewing}
        voucher={viewing ?? null}
        onClose={() => setViewingId(null)}
        onPost={handlePost}
        onCancel={handleCancel}
        isPostPending={postMut.isPending}
        isCancelPending={cancelMut.isPending}
      />
    </div>
  );
};
