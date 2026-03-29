// ============================================
// features/users/components/UserFormModal.tsx
// ============================================

import React, { useEffect } from 'react';
import { useForm } from 'react-hook-form';
import type { User, Role, CreateUserRequest } from '../user.types';
import styles from './UserFormModal.module.css';

interface FormValues {
  name:     string;
  email:    string;
  username: string;
  password: string;
  roleId:   string;
}

interface Props {
  open:      boolean;
  editing:   User | null;
  roles:     Role[];
  isLoading: boolean;
  error:     string | null;
  onClose:   () => void;
  onSubmit:  (data: CreateUserRequest, id?: string) => void;
}

export const UserFormModal: React.FC<Props> = ({
  open, editing, roles, isLoading, error, onClose, onSubmit,
}) => {
  const isEdit = !!editing;

  const {
    register, handleSubmit, reset,
    formState: { errors },
  } = useForm<FormValues>({ mode: 'onBlur' });

  useEffect(() => {
    if (open) {
      reset({
        name:     editing?.name     ?? '',
        email:    editing?.email    ?? '',
        username: editing?.username ?? '',
        password: '',
        roleId:   editing?.role?.id ?? '',
      });
    }
  }, [open, editing, reset]);

  if (!open) return null;

  const submit = handleSubmit(({ name, email, username, password, roleId }) => {
    onSubmit(
      { name, email, username, password, roleId: roleId || null },
      editing?.id,
    );
  });

  return (
    <div className={styles.overlay} role="dialog" aria-modal aria-label={isEdit ? 'Editar usuario' : 'Nuevo usuario'}>
      <div className={styles.modal}>

        {/* Header */}
        <div className={styles.header}>
          <div className={styles.titleRow}>
            <div className={styles.icon} aria-hidden>
              <svg width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth={2} strokeLinecap="round" strokeLinejoin="round">
                <path d="M20 21v-2a4 4 0 00-4-4H8a4 4 0 00-4 4v2" />
                <circle cx="12" cy="7" r="4" />
              </svg>
            </div>
            <h2>{isEdit ? 'Editar usuario' : 'Nuevo usuario'}</h2>
          </div>
          <button className={styles.closeBtn} onClick={onClose} aria-label="Cerrar">
            <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth={2} strokeLinecap="round">
              <line x1="18" y1="6"  x2="6"  y2="18" />
              <line x1="6"  y1="6"  x2="18" y2="18" />
            </svg>
          </button>
        </div>

        {/* Error */}
        {error && (
          <div className={styles.errorBanner} role="alert">
            <svg width="13" height="13" viewBox="0 0 16 16" fill="none">
              <circle cx="8" cy="8" r="7" stroke="var(--color-error)" strokeWidth="1.5" />
              <path d="M8 5v4M8 11v.5" stroke="var(--color-error)" strokeWidth="1.5" strokeLinecap="round" />
            </svg>
            {error}
          </div>
        )}

        {/* Form */}
        <form className={styles.form} onSubmit={submit} noValidate>

          {/* Name */}
          <div className={styles.field}>
            <label className={styles.label} htmlFor="u-name">Nombre completo</label>
            <input
              id="u-name"
              className={`${styles.input} ${errors.name ? styles.inputErr : ''}`}
              placeholder="Ej: Juan Pérez"
              {...register('name', { required: 'Requerido', minLength: { value: 2, message: 'Mínimo 2 caracteres' } })}
            />
            {errors.name && <span className={styles.fieldErr}>{errors.name.message}</span>}
          </div>

          {/* Email + Username row */}
          <div className={styles.row}>
            <div className={styles.field}>
              <label className={styles.label} htmlFor="u-email">Correo electrónico</label>
              <input
                id="u-email"
                type="email"
                className={`${styles.input} ${errors.email ? styles.inputErr : ''}`}
                placeholder="correo@iglesia.co"
                {...register('email', {
                  required: 'Requerido',
                  pattern:  { value: /^[^\s@]+@[^\s@]+\.[^\s@]+$/, message: 'Correo inválido' },
                })}
              />
              {errors.email && <span className={styles.fieldErr}>{errors.email.message}</span>}
            </div>

            <div className={styles.field}>
              <label className={styles.label} htmlFor="u-username">Usuario</label>
              <input
                id="u-username"
                className={`${styles.input} ${errors.username ? styles.inputErr : ''}`}
                placeholder="juan.perez"
                {...register('username', {
                  required:  'Requerido',
                  minLength: { value: 3, message: 'Mínimo 3 caracteres' },
                  pattern:   { value: /^[a-zA-Z0-9._-]+$/, message: 'Sin espacios ni caracteres especiales' },
                })}
              />
              {errors.username && <span className={styles.fieldErr}>{errors.username.message}</span>}
            </div>
          </div>

          {/* Password — required only on create */}
          <div className={styles.field}>
            <label className={styles.label} htmlFor="u-password">
              Contraseña
              {isEdit && <span className={styles.optional}> (dejar vacío para no cambiar)</span>}
            </label>
            <input
              id="u-password"
              type="password"
              autoComplete={isEdit ? 'new-password' : 'new-password'}
              className={`${styles.input} ${errors.password ? styles.inputErr : ''}`}
              placeholder={isEdit ? '••••••••' : 'Mínimo 8 caracteres'}
              {...register('password', {
                required:  !isEdit ? 'Requerido' : false,
                minLength: { value: 8, message: 'Mínimo 8 caracteres' },
                validate:  (v) => (!isEdit && !v) ? 'Requerido' : true,
              })}
            />
            {errors.password && <span className={styles.fieldErr}>{errors.password.message}</span>}
          </div>

          {/* Role */}
          <div className={styles.field}>
            <label className={styles.label} htmlFor="u-role">
              Rol
              <span className={styles.optional}> (opcional)</span>
            </label>
            <select
              id="u-role"
              className={`${styles.input} ${styles.select}`}
              {...register('roleId')}
            >
              <option value="">— Sin rol asignado —</option>
              {roles.map((r) => (
                <option key={r.id} value={r.id}>{r.name}</option>
              ))}
            </select>
          </div>

          {/* Actions */}
          <div className={styles.actions}>
            <button type="button" className={styles.cancelBtn} onClick={onClose}>Cancelar</button>
            <button type="submit" className={styles.submitBtn} disabled={isLoading} aria-busy={isLoading}>
              {isLoading
                ? <><span className={styles.spinner} aria-hidden />Guardando…</>
                : isEdit ? 'Guardar cambios' : 'Crear usuario'
              }
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};
