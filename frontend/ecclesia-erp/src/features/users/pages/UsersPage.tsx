// ============================================
// features/users/pages/UsersPage.tsx
// ============================================

import React, { useState } from 'react';
import {
  useUsers, useRoles,
  useCreateUser, useUpdateUser, useDeleteUser, useAssignRole,
} from '../hooks/useUsers';
import { UserFormModal }   from '../components/UserFormModal';
import { AssignRoleModal } from '../components/AssignRoleModal';
import { Pagination }      from '../../../shared/components/Pagination';
import { useDebounce }     from '../../../shared/hooks/useDebounce';
import  type { User, CreateUserRequest } from '../user.types';
import styles from './UsersPage.module.css';

const PAGE_SIZE = 10;

// ── Avatar initials helper ──
const initials = (name: string) =>
  name.split(' ').slice(0, 2).map((w) => w[0]).join('').toUpperCase();

export const UsersPage: React.FC = () => {
  // ── Filters & pagination ──
  const [search, setSearch]   = useState('');
  const [page, setPage]       = useState(1);
  const debouncedSearch       = useDebounce(search, 400);

  // ── Data ──
  const { data, isLoading, isError, isFetching } = useUsers(page, PAGE_SIZE, debouncedSearch);
  const { data: roles = [] }                      = useRoles();

  // ── Mutations ──
  const createMut     = useCreateUser();
  const updateMut     = useUpdateUser();
  const deleteMut     = useDeleteUser();
  const assignRoleMut = useAssignRole();

  // ── Modal state ──
  const [formOpen,   setFormOpen]   = useState(false);
  const [editing,    setEditing]    = useState<User | null>(null);
  const [assigning,  setAssigning]  = useState<User | null>(null);
  // const [deleting,   setDeleting]   = useState<User | null>(null);
  const [formError,  setFormError]  = useState<string | null>(null);
  const [roleError,  setRoleError]  = useState<string | null>(null);

  const users      = data?.items      ?? [];
  const totalPages = data?.totalPages ?? 1;
  const total      = data?.totalCount ?? 0;

  // ── Handlers ──
  const openCreate = () => { setEditing(null); setFormError(null); setFormOpen(true); };
  const openEdit   = (u: User) => { setEditing(u); setFormError(null); setFormOpen(true); };
  const closeForm  = () => { setFormOpen(false); setEditing(null); };

  const handleFormSubmit = (data: CreateUserRequest, id?: string) => {
    setFormError(null);
    if (id) {
      updateMut.mutate(
        { id, data: { name: data.name, email: data.email, username: data.username } },
        { onSuccess: closeForm, onError: (e) => setFormError(e.message) },
      );
    } else {
      createMut.mutate(data, {
        onSuccess: () => { closeForm(); setPage(1); },
        onError:   (e) => setFormError(e.message),
      });
    }
  };

  const handleDelete = (u: User) => {
    if (!window.confirm(`¿Eliminar al usuario "${u.name}"? Esta acción no se puede deshacer.`)) return;
    deleteMut.mutate(u.id);
  };

  const handleAssignRole = (userId: string, roleId: string) => {
    setRoleError(null);
    assignRoleMut.mutate(
      { userId, data: { roleId } },
      {
        onSuccess: () => setAssigning(null),
        onError:   (e) => setRoleError(e.message),
      },
    );
  };

  const handleSearchChange = (v: string) => {
    setSearch(v);
    setPage(1);
  };

  return (
    <div className={styles.page}>

      {/* ── Header ── */}
      <div className={styles.pageHeader}>
        <div className={styles.headerLeft}>
          <h1 className={styles.title}>Usuarios</h1>
          <p className={styles.subtitle}>Gestión de acceso y roles del sistema</p>
        </div>
        <button className={styles.newBtn} onClick={openCreate}>
          <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth={2.5} strokeLinecap="round" aria-hidden>
            <line x1="12" y1="5" x2="12" y2="19" /><line x1="5" y1="12" x2="19" y2="12" />
          </svg>
          Nuevo usuario
        </button>
      </div>

      {/* ── Summary chips ── */}
      <div className={styles.summaryRow}>
        <div className={styles.summaryChip}>
          <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth={1.8} strokeLinecap="round" aria-hidden>
            <path d="M17 21v-2a4 4 0 00-4-4H5a4 4 0 00-4 4v2" /><circle cx="9" cy="7" r="4" />
            <path d="M23 21v-2a4 4 0 00-3-3.87" /><path d="M16 3.13a4 4 0 010 7.75" />
          </svg>
          <span className={styles.summaryValue}>{isLoading ? '…' : total}</span>
          <span className={styles.summaryLabel}>usuarios totales</span>
        </div>
        <div className={styles.summaryChip}>
          <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth={1.8} strokeLinecap="round" aria-hidden>
            <path d="M12 22s8-4 8-10V5l-8-3-8 3v7c0 6 8 10 8 10z" />
          </svg>
          <span className={styles.summaryValue}>{roles.length}</span>
          <span className={styles.summaryLabel}>roles disponibles</span>
        </div>
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
            placeholder="Buscar por nombre, correo o usuario…"
            value={search}
            onChange={(e) => handleSearchChange(e.target.value)}
          />
          {isFetching && !isLoading && (
            <span className={styles.searching} aria-hidden />
          )}
        </div>
      </div>

      {/* ── Table ── */}
      <div className={styles.tableWrap}>
        {isLoading ? (
          <div className={styles.stateBox}>
            <span className={styles.spinner} aria-label="Cargando" />
            <p>Cargando usuarios…</p>
          </div>
        ) : isError ? (
          <div className={styles.stateBox}>
            <svg width="30" height="30" viewBox="0 0 24 24" fill="none" stroke="var(--color-error)" strokeWidth={1.5} strokeLinecap="round" aria-hidden>
              <circle cx="12" cy="12" r="10" /><line x1="12" y1="8" x2="12" y2="12" /><line x1="12" y1="16" x2="12.01" y2="16" />
            </svg>
            <p>Error al cargar los usuarios.</p>
          </div>
        ) : users.length === 0 ? (
          <div className={styles.stateBox}>
            <svg width="36" height="36" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth={1.2} strokeLinecap="round" aria-hidden>
              <path d="M20 21v-2a4 4 0 00-4-4H8a4 4 0 00-4 4v2" /><circle cx="12" cy="7" r="4" />
            </svg>
            <p>{search ? 'Sin resultados para tu búsqueda.' : 'No hay usuarios registrados.'}</p>
            {!search && (
              <button className={styles.emptyAction} onClick={openCreate}>Crear primer usuario</button>
            )}
          </div>
        ) : (
          <>
            <table className={styles.table}>
              <thead>
                <tr className={styles.thead}>
                  <th className={styles.th}>Usuario</th>
                  <th className={styles.th}>Correo</th>
                  <th className={styles.th}>Nombre de usuario</th>
                  <th className={styles.th}>Rol</th>
                  <th className={styles.th}>Creado</th>
                  <th className={styles.th} style={{ textAlign: 'right' }}>Acciones</th>
                </tr>
              </thead>
              <tbody>
                {users.map((u) => (
                  <tr key={u.id} className={styles.row}>
                    {/* Avatar + name */}
                    <td className={styles.td}>
                      <div className={styles.userCell}>
                        <div className={styles.avatar}>{initials(u.name)}</div>
                        <span className={styles.userName}>{u.name}</span>
                      </div>
                    </td>

                    <td className={styles.td}>
                      <span className={styles.meta}>{u.email}</span>
                    </td>

                    <td className={styles.td}>
                      <span className={styles.username}>@{u.username}</span>
                    </td>

                    <td className={styles.td}>
                      {u.role
                        ? <span className={styles.roleBadge}>{u.role.name}</span>
                        : <span className={styles.noRole}>Sin rol</span>
                      }
                    </td>

                    <td className={styles.td}>
                      <span className={styles.meta}>
                        {new Date(u.createdAt).toLocaleDateString('es-CO', {
                          day: '2-digit', month: 'short', year: 'numeric',
                        })}
                      </span>
                    </td>

                    {/* Actions */}
                    <td className={styles.td} style={{ textAlign: 'right' }}>
                      <div className={styles.actions}>
                        <button
                          className={`${styles.actionBtn} ${styles.roleBtn}`}
                          onClick={() => { setRoleError(null); setAssigning(u); }}
                          title="Asignar rol"
                          aria-label={`Asignar rol a ${u.name}`}
                        >
                          <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth={2} strokeLinecap="round" strokeLinejoin="round">
                            <path d="M12 22s8-4 8-10V5l-8-3-8 3v7c0 6 8 10 8 10z" />
                          </svg>
                        </button>
                        <button
                          className={`${styles.actionBtn} ${styles.editBtn}`}
                          onClick={() => openEdit(u)}
                          title="Editar"
                          aria-label={`Editar ${u.name}`}
                        >
                          <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth={2} strokeLinecap="round" strokeLinejoin="round">
                            <path d="M11 4H4a2 2 0 00-2 2v14a2 2 0 002 2h14a2 2 0 002-2v-7" />
                            <path d="M18.5 2.5a2.121 2.121 0 013 3L12 15l-4 1 1-4 9.5-9.5z" />
                          </svg>
                        </button>
                        <button
                          className={`${styles.actionBtn} ${styles.deleteBtn}`}
                          onClick={() => handleDelete(u)}
                          title="Eliminar"
                          aria-label={`Eliminar ${u.name}`}
                          disabled={deleteMut.isPending}
                        >
                          <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth={2} strokeLinecap="round" strokeLinejoin="round">
                            <polyline points="3 6 5 6 21 6" />
                            <path d="M19 6l-1 14a2 2 0 01-2 2H8a2 2 0 01-2-2L5 6" />
                            <path d="M10 11v6M14 11v6M9 6V4a1 1 0 011-1h4a1 1 0 011 1v2" />
                          </svg>
                        </button>
                      </div>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>

            <Pagination
              page={page}
              totalPages={totalPages}
              total={total}
              pageSize={PAGE_SIZE}
              onPageChange={setPage}
            />
          </>
        )}
      </div>

      {/* ── Modals ── */}
      <UserFormModal
        open={formOpen}
        editing={editing}
        roles={roles}
        isLoading={createMut.isPending || updateMut.isPending}
        error={formError}
        onClose={closeForm}
        onSubmit={handleFormSubmit}
      />

      <AssignRoleModal
        open={!!assigning}
        user={assigning}
        roles={roles}
        isLoading={assignRoleMut.isPending}
        error={roleError}
        onClose={() => setAssigning(null)}
        onSubmit={handleAssignRole}
      />
    </div>
  );
};
