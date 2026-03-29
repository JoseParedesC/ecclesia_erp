// ============================================
// features/auth/pages/LoginPage.tsx
// ============================================

import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useLoginForm } from '../hooks/useLoginForm';
import { useLogin } from '../hooks/useLogin';
import styles from './LoginPage.module.css';

export const LoginPage: React.FC = () => {
  const [showPassword, setShowPassword] = useState(false);
  const navigate = useNavigate();

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useLoginForm();

  const { mutate: login, isPending, error, isError } = useLogin(() => {
    navigate('/', { replace: true });
  });

  const onSubmit = handleSubmit((data) => login(data));

  return (
    <div className={styles.root}>
      <div className={styles.bgOrb1} aria-hidden />
      <div className={styles.bgOrb2} aria-hidden />
      <div className={styles.bgGrid} aria-hidden />

      <main className={styles.card} role="main">
        <header className={styles.header}>
          <div className={styles.logoMark} aria-hidden>
            <svg width="36" height="36" viewBox="0 0 36 36" fill="none">
              <path d="M18 3L33 12V24L18 33L3 24V12L18 3Z" stroke="var(--color-gold)" strokeWidth="1.5" fill="none" />
              <path d="M18 10L27 15.5V24L18 29L9 24V15.5L18 10Z" fill="var(--color-gold)" fillOpacity="0.12" stroke="var(--color-gold)" strokeWidth="1" />
              <line x1="18" y1="10" x2="18" y2="29" stroke="var(--color-gold)" strokeWidth="1" strokeOpacity="0.5" />
              <line x1="9" y1="15.5" x2="27" y2="15.5" stroke="var(--color-gold)" strokeWidth="1" strokeOpacity="0.5" />
              <line x1="9" y1="24" x2="27" y2="24" stroke="var(--color-gold)" strokeWidth="1" strokeOpacity="0.5" />
            </svg>
          </div>
          <h1 className={styles.title}>Ecclesia</h1>
          <p className={styles.subtitle}>Sistema de Gestión</p>
        </header>

        <div className={styles.divider} aria-hidden />

        {isError && (
          <div className={styles.errorBanner} role="alert">
            <svg width="16" height="16" viewBox="0 0 16 16" fill="none" aria-hidden>
              <circle cx="8" cy="8" r="7" stroke="var(--color-error)" strokeWidth="1.5" />
              <path d="M8 5v4M8 11v.5" stroke="var(--color-error)" strokeWidth="1.5" strokeLinecap="round" />
            </svg>
            <span>{error?.message ?? 'Error al iniciar sesión'}</span>
          </div>
        )}

        <form className={styles.form} onSubmit={onSubmit} noValidate>
          <div className={styles.fieldGroup}>
            <label htmlFor="email" className={styles.label}>Correo electrónico</label>
            <div className={styles.inputWrapper}>
              <svg className={styles.inputIcon} width="16" height="16" viewBox="0 0 16 16" fill="none" aria-hidden>
                <rect x="1.5" y="3.5" width="13" height="9" rx="1.5" stroke="currentColor" strokeWidth="1.2" />
                <path d="M1.5 5.5L8 9.5L14.5 5.5" stroke="currentColor" strokeWidth="1.2" strokeLinejoin="round" />
              </svg>
              <input
                id="email"
                type="email"
                autoComplete="email"
                placeholder="tu@correo.com"
                className={`${styles.input} ${errors.email ? styles.inputError : ''}`}
                aria-invalid={!!errors.email}
                aria-describedby={errors.email ? 'email-error' : undefined}
                {...register('email', {
                  required: 'El correo es obligatorio',
                  pattern: { value: /^[^\s@]+@[^\s@]+\.[^\s@]+$/, message: 'Ingresa un correo válido' },
                })}
              />
            </div>
            {errors.email && <p id="email-error" className={styles.fieldError} role="alert">{errors.email.message}</p>}
          </div>

          <div className={styles.fieldGroup}>
            <label htmlFor="password" className={styles.label}>Contraseña</label>
            <div className={styles.inputWrapper}>
              <svg className={styles.inputIcon} width="16" height="16" viewBox="0 0 16 16" fill="none" aria-hidden>
                <rect x="3" y="7" width="10" height="7.5" rx="1.5" stroke="currentColor" strokeWidth="1.2" />
                <path d="M5.5 7V5a2.5 2.5 0 015 0v2" stroke="currentColor" strokeWidth="1.2" strokeLinecap="round" />
                <circle cx="8" cy="10.5" r="1" fill="currentColor" />
              </svg>
              <input
                id="password"
                type={showPassword ? 'text' : 'password'}
                autoComplete="current-password"
                placeholder="••••••••"
                className={`${styles.input} ${errors.password ? styles.inputError : ''}`}
                aria-invalid={!!errors.password}
                aria-describedby={errors.password ? 'password-error' : undefined}
                {...register('password', {
                  required: 'La contraseña es obligatoria',
                  minLength: { value: 6, message: 'Mínimo 6 caracteres' },
                })}
              />
              <button type="button" className={styles.togglePassword} onClick={() => setShowPassword((v) => !v)} aria-label={showPassword ? 'Ocultar contraseña' : 'Mostrar contraseña'}>
                {showPassword ? (
                  <svg width="16" height="16" viewBox="0 0 16 16" fill="none">
                    <path d="M1 8s2.5-5 7-5 7 5 7 5-2.5 5-7 5-7-5-7-5z" stroke="currentColor" strokeWidth="1.2" />
                    <circle cx="8" cy="8" r="2" stroke="currentColor" strokeWidth="1.2" />
                    <line x1="2" y1="14" x2="14" y2="2" stroke="currentColor" strokeWidth="1.2" strokeLinecap="round" />
                  </svg>
                ) : (
                  <svg width="16" height="16" viewBox="0 0 16 16" fill="none">
                    <path d="M1 8s2.5-5 7-5 7 5 7 5-2.5 5-7 5-7-5-7-5z" stroke="currentColor" strokeWidth="1.2" />
                    <circle cx="8" cy="8" r="2" stroke="currentColor" strokeWidth="1.2" />
                  </svg>
                )}
              </button>
            </div>
            {errors.password && <p id="password-error" className={styles.fieldError} role="alert">{errors.password.message}</p>}
          </div>

          <button type="submit" className={styles.submitBtn} disabled={isPending} aria-busy={isPending}>
            {isPending ? (
              <><span className={styles.spinner} aria-hidden />Verificando…</>
            ) : (
              'Ingresar al sistema'
            )}
          </button>
        </form>

        <footer className={styles.footer}>
          <span>Ecclesia ERP · Sistema de Gestión Eclesial</span>
        </footer>
      </main>
    </div>
  );
};
