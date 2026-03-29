// ============================================
// features/users/components/AssignRoleModal.tsx
// ============================================

import React, { useEffect } from 'react';
import { useForm } from 'react-hook-form';
import type { User, Role } from '../user.types';
import styles from './AssignRoleModal.module.css';

interface FormValues { roleId: string }

interface Props {
  open:      boolean;
  user:      User | null;
  roles:     Role[];
  isLoading: boolean;
  error:     string | null;
  onClose:   () => void;
  onSubmit:  (userId: string, roleId: string) => void;
}

export const AssignRoleModal: React.FC<Props> = ({
  open, user, roles, isLoading, error, onClose, onSubmit,
}) => {
  const { register, handleSubmit, reset } = useForm<FormValues>();

  useEffect(() => {
    if (open) reset({ roleId: user?.role?.id ?? '' });
  }, [open, user, reset]);

  if (!open || !user) return null;

  const submit = handleSubmit(({ roleId }) => {
    if (roleId) onSubmit(user.id, roleId);
  });

  // avatar initials
  const initials = user.name
    .split(' ')
    .slice(0, 2)
    .map((w) => w[0])
    .join('')
    .toUpperCase();

  return (
    <div className={styles.overlay} role="dialog" aria-modal aria-label="Asignar rol">
      <div className={styles.modal}>

        <div className={styles.header}>
          <div className={styles.titleRow}>
            <div className={styles.icon} aria-hidden>
              <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth={2} strokeLinecap="round" strokeLinejoin="round">
                <path d="M12 22s8-4 8-10V5l-8-3-8 3v7c0 6 8 10 8 10z" />
              </svg>
            </div>
            <h2>Asignar rol</h2>
          </div>
          <button className={styles.closeBtn} onClick={onClose} aria-label="Cerrar">
            <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth={2} strokeLinecap="round">
              <line x1="18" y1="6" x2="6" y2="18" /><line x1="6" y1="6" x2="18" y2="18" />
            </svg>
          </button>
        </div>

        {/* User chip */}
        <div className={styles.userChip}>
          <div className={styles.avatar}>{initials}</div>
          <div className={styles.userInfo}>
            <span className={styles.userName}>{user.name}</span>
            <span className={styles.userMeta}>{user.email} · @{user.username}</span>
          </div>
          {user.role && (
            <span className={styles.currentRole}>{user.role.name}</span>
          )}
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

        <form className={styles.form} onSubmit={submit} noValidate>
          <div className={styles.field}>
            <label className={styles.label} htmlFor="ar-role">Selecciona el nuevo rol</label>
            <div className={styles.roleList}>
              {roles.map((r) => (
                <label key={r.id} className={styles.roleOption}>
                  <input
                    type="radio"
                    value={r.id}
                    className={styles.radio}
                    {...register('roleId', { required: true })}
                  />
                  <div className={styles.roleCard}>
                    <div className={styles.roleIcon} aria-hidden>
                      <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth={2} strokeLinecap="round" strokeLinejoin="round">
                        <path d="M12 22s8-4 8-10V5l-8-3-8 3v7c0 6 8 10 8 10z" />
                      </svg>
                    </div>
                    <span className={styles.roleName}>{r.name}</span>
                    {user.role?.id === r.id && (
                      <span className={styles.currentTag}>actual</span>
                    )}
                  </div>
                </label>
              ))}
            </div>
          </div>

          <div className={styles.actions}>
            <button type="button" className={styles.cancelBtn} onClick={onClose}>Cancelar</button>
            <button type="submit" className={styles.submitBtn} disabled={isLoading} aria-busy={isLoading}>
              {isLoading
                ? <><span className={styles.spinner} aria-hidden />Asignando…</>
                : 'Asignar rol'
              }
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};
