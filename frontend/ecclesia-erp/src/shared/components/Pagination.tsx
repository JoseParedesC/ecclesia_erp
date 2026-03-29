// ============================================
// shared/components/Pagination.tsx
// ============================================

import React from 'react';
import styles from './Pagination.module.css';

interface Props {
  page:       number;
  totalPages: number;
  total:      number;
  pageSize:   number;
  onPageChange: (page: number) => void;
}

export const Pagination: React.FC<Props> = ({
  page, totalPages, total, pageSize, onPageChange,
}) => {
  if (totalPages <= 1) return null;

  const from = (page - 1) * pageSize + 1;
  const to   = Math.min(page * pageSize, total);

  // Build page window: always show first, last, current ±1
  const pages = new Set([1, totalPages, page, page - 1, page + 1].filter(
    (p) => p >= 1 && p <= totalPages,
  ));
  const sorted = Array.from(pages).sort((a, b) => a - b);

  return (
    <div className={styles.pagination}>
      <span className={styles.info}>
        {from}–{to} de {total}
      </span>

      <div className={styles.controls}>
        <button
          className={styles.btn}
          disabled={page <= 1}
          onClick={() => onPageChange(page - 1)}
          aria-label="Página anterior"
        >
          <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth={2} strokeLinecap="round" strokeLinejoin="round">
            <polyline points="15 18 9 12 15 6" />
          </svg>
        </button>

        {sorted.map((p, i) => {
          const prev = sorted[i - 1];
          const gap  = prev !== undefined && p - prev > 1;
          return (
            <React.Fragment key={p}>
              {gap && <span className={styles.ellipsis}>…</span>}
              <button
                className={`${styles.btn} ${p === page ? styles.btnActive : ''}`}
                onClick={() => onPageChange(p)}
                aria-current={p === page ? 'page' : undefined}
              >
                {p}
              </button>
            </React.Fragment>
          );
        })}

        <button
          className={styles.btn}
          disabled={page >= totalPages}
          onClick={() => onPageChange(page + 1)}
          aria-label="Página siguiente"
        >
          <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth={2} strokeLinecap="round" strokeLinejoin="round">
            <polyline points="9 18 15 12 9 6" />
          </svg>
        </button>
      </div>
    </div>
  );
};
