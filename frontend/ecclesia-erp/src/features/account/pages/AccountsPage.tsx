// ============================================
// features/account/pages/AccountsPage.tsx
// ============================================

import React, { useState, useMemo } from 'react';
import {
  useAccounts,
  useCreateAccount,
  useUpdateAccount,
  useDeleteAccount,
} from '../hook/useAccounts';
import { AccountFormModal } from '../components/AccountFormModal';
import { DeleteConfirmModal } from '../components/DeleteConfirmModal';
import { AccountTreeRow } from '../components/AccountTreeRow';
import { AccountTypeBadge } from '../components/AccountTypeBadge';
import { filterAccounts } from '../utils/accountTree';
import type { Account, AccountFilters, AccountType } from '../account.types';
import styles from './AccountsPage.module.css';

const TYPE_OPTIONS: { value: AccountType | ''; label: string }[] = [
  { value: '',          label: 'Todos los tipos' },
  { value: 'ASSET',     label: 'Activo' },
  { value: 'LIABILITY', label: 'Pasivo' },
  { value: 'EQUITY',    label: 'Patrimonio' },
  { value: 'INCOME',    label: 'Ingreso' },
  { value: 'EXPENSE',   label: 'Gasto' },
];

export const AccountsPage: React.FC = () => {
  // ── Data ──
  const { data: flat = [], isLoading, isError } = useAccounts();
  const createMut  = useCreateAccount();
  const updateMut  = useUpdateAccount();
  const deleteMut  = useDeleteAccount();

  // ── UI state ──
  const [filters, setFilters]         = useState<AccountFilters>({ search: '', type: '' });
  const [formOpen, setFormOpen]       = useState(false);
  const [editing, setEditing]         = useState<Account | null>(null);
  const [deleting, setDeleting]       = useState<Account | null>(null);
  const [mutError, setMutError]       = useState<string | null>(null);

  // ── Derived tree ──
  const tree = useMemo(() => filterAccounts(flat, filters), [flat, filters]);

  // ── Handlers ──
  const openCreate = () => { setEditing(null); setMutError(null); setFormOpen(true); };
  const openEdit   = (a: Account) => { setEditing(a); setMutError(null); setFormOpen(true); };
  const closeForm  = () => { setFormOpen(false); setEditing(null); };

  const handleSubmit = (data: Parameters<typeof createMut.mutate>[0], id?: string) => {
    setMutError(null);
    if (id) {
      updateMut.mutate(
        { id, data: { name: data.name, type: data.type, parentAccountId: data.parentAccountId } },
        { onSuccess: closeForm, onError: (e) => setMutError(e.message) },
      );
    } else {
      createMut.mutate(data, {
        onSuccess: closeForm,
        onError: (e) => setMutError(e.message),
      });
    }
  };

  const handleDelete = () => {
    if (!deleting) return;
    deleteMut.mutate(deleting.id, {
      onSuccess: () => setDeleting(null),
      onError:   (e) => { setDeleting(null); alert(e.message); },
    });
  };

  // ── Summary counts ──
  const counts = useMemo(() => {
    const c: Record<string, number> = {};
    flat.forEach((a) => { c[a.type] = (c[a.type] ?? 0) + 1; });
    return c;
  }, [flat]);

  return (
    <div className={styles.page}>
      {/* ── Page header ── */}
      <div className={styles.pageHeader}>
        <div className={styles.headerLeft}>
          <h1 className={styles.title}>Plan de cuentas</h1>
          <p className={styles.subtitle}>Catálogo jerárquico de cuentas contables</p>
        </div>
        <button className={styles.newBtn} onClick={openCreate}>
          <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth={2.5} strokeLinecap="round" aria-hidden>
            <line x1="12" y1="5" x2="12" y2="19" /><line x1="5" y1="12" x2="19" y2="12" />
          </svg>
          Nueva cuenta
        </button>
      </div>

      {/* ── Summary chips ── */}
      <div className={styles.summaryRow}>
        {(['ASSET','LIABILITY','EQUITY','INCOME','EXPENSE'] as AccountType[]).map((t) => (
          <button
            key={t}
            className={`${styles.chip} ${filters.type === t ? styles.chipActive : ''}`}
            onClick={() => setFilters((f) => ({ ...f, type: f.type === t ? '' : t }))}
          >
            <AccountTypeBadge type={t} />
            <span className={styles.chipCount}>{counts[t] ?? 0}</span>
          </button>
        ))}
      </div>

      {/* ── Toolbar ── */}
      <div className={styles.toolbar}>
        <div className={styles.searchWrap}>
          <svg className={styles.searchIcon} width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth={2} strokeLinecap="round" aria-hidden>
            <circle cx="11" cy="11" r="8" /><line x1="21" y1="21" x2="16.65" y2="16.65" />
          </svg>
          <input
            className={styles.searchInput}
            type="search"
            placeholder="Buscar por código o nombre…"
            value={filters.search}
            onChange={(e) => setFilters((f) => ({ ...f, search: e.target.value }))}
          />
        </div>
        <select
          className={styles.typeFilter}
          value={filters.type}
          onChange={(e) => setFilters((f) => ({ ...f, type: e.target.value as AccountType | '' }))}
        >
          {TYPE_OPTIONS.map((o) => (
            <option key={o.value} value={o.value}>{o.label}</option>
          ))}
        </select>
        <span className={styles.totalBadge}>{flat.length} cuentas</span>
      </div>

      {/* ── Table ── */}
      <div className={styles.tableWrap}>
        {isLoading ? (
          <div className={styles.stateBox}>
            <span className={styles.spinner} aria-label="Cargando" />
            <p>Cargando plan de cuentas…</p>
          </div>
        ) : isError ? (
          <div className={styles.stateBox}>
            <svg width="32" height="32" viewBox="0 0 24 24" fill="none" stroke="var(--color-error)" strokeWidth={1.5} strokeLinecap="round" aria-hidden>
              <circle cx="12" cy="12" r="10" /><line x1="12" y1="8" x2="12" y2="12" /><line x1="12" y1="16" x2="12.01" y2="16" />
            </svg>
            <p>Error al cargar las cuentas.</p>
          </div>
        ) : tree.length === 0 ? (
          <div className={styles.stateBox}>
            <svg width="36" height="36" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth={1.2} strokeLinecap="round" aria-hidden>
              <path d="M9 7h6m0 10v-3m-3 3h.01M9 17h.01M9 14h.01M12 14h.01M15 11h.01M12 11h.01M9 11h.01M7 21h10a2 2 0 002-2V5a2 2 0 00-2-2H7a2 2 0 00-2 2v14a2 2 0 002 2z" />
            </svg>
            <p>{filters.search || filters.type ? 'Sin resultados para los filtros aplicados.' : 'Aún no hay cuentas registradas.'}</p>
            {!filters.search && !filters.type && (
              <button className={styles.emptyAction} onClick={openCreate}>Crear primera cuenta</button>
            )}
          </div>
        ) : (
          <table className={styles.table}>
            <thead>
              <tr className={styles.thead}>
                <th className={styles.th}>Código</th>
                <th className={styles.th}>Nombre</th>
                <th className={styles.th}>Tipo</th>
                <th className={styles.th} style={{ textAlign: 'right' }}>Acciones</th>
              </tr>
            </thead>
            <tbody>
              {tree.map((node) => (
                <AccountTreeRow
                  key={node.id}
                  node={node}
                  depth={0}
                  onEdit={openEdit}
                  onDelete={setDeleting}
                />
              ))}
            </tbody>
          </table>
        )}
      </div>

      {/* ── Modals ── */}
      <AccountFormModal
        open={formOpen}
        editing={editing}
        accounts={flat}
        onClose={closeForm}
        onSubmit={handleSubmit}
        isLoading={createMut.isPending || updateMut.isPending}
        error={mutError}
      />

      <DeleteConfirmModal
        open={!!deleting}
        title="Eliminar cuenta"
        description={`¿Eliminar "${deleting?.name}" (${deleting?.code})? Esta acción no se puede deshacer.`}
        isLoading={deleteMut.isPending}
        onConfirm={handleDelete}
        onCancel={() => setDeleting(null)}
      />
    </div>
  );
};
