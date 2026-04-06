// ============================================
// features/journalVoucher/components/VoucherDetailModal.tsx
// ============================================

import React from 'react';
import type { JournalVoucher } from '../journalVoucher.types';
import { VoucherStatusBadge } from './VoucherStatusBadge';
import { VoucherTypeBadge }   from './VoucherTypeBadge';
import styles from './VoucherDetailModal.module.css';

const fmt = (n: number) =>
  new Intl.NumberFormat('es-CO', { minimumFractionDigits: 2, maximumFractionDigits: 2 }).format(n);

interface Props {
  open:     boolean;
  voucher:  JournalVoucher | null;
  onClose:  () => void;
  onPost:   (id: string) => void;
  onCancel: (id: string) => void;
  isPostPending:   boolean;
  isCancelPending: boolean;
}

export const VoucherDetailModal: React.FC<Props> = ({
  open, voucher, onClose, onPost, onCancel, isPostPending, isCancelPending,
}) => {
  if (!open || !voucher) return null;

  // const debits  = voucher.lines.filter(l => l.lineType === 'DEBIT');
  // const credits = voucher.lines.filter(l => l.lineType === 'CREDIT');

  return (
    <div className={styles.overlay} role="dialog" aria-modal aria-label={`Asiento ${voucher.voucherNumber}`}>
      <div className={styles.modal}>

        {/* Header */}
        <div className={styles.header}>
          <div className={styles.headerLeft}>
            <div>
              <div className={styles.voucherNum}>{voucher.voucherNumber}</div>
              <div className={styles.headerBadges}>
                <VoucherTypeBadge type={voucher.type} />
                <VoucherStatusBadge status={voucher.status} />
              </div>
            </div>
          </div>
          <button className={styles.closeBtn} onClick={onClose} aria-label="Cerrar">
            <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth={2} strokeLinecap="round">
              <line x1="18" y1="6" x2="6" y2="18" /><line x1="6" y1="6" x2="18" y2="18" />
            </svg>
          </button>
        </div>

        {/* Meta grid */}
        <div className={styles.metaGrid}>
          {[
            { label: 'Fecha',      value: new Date(voucher.date + 'T00:00:00').toLocaleDateString('es-CO', { weekday: 'long', day: '2-digit', month: 'long', year: 'numeric' }) },
            { label: 'Rostro',     value: voucher.rostroName },
            { label: 'Comunidad',  value: voucher.communityName },
            { label: 'Creado por', value: voucher.createdBy ?? '—' },
          ].map(m => (
            <div key={m.label} className={styles.metaItem}>
              <span className={styles.metaLabel}>{m.label}</span>
              <span className={styles.metaValue}>{m.value}</span>
            </div>
          ))}
        </div>

        {voucher.description && (
          <div className={styles.description}>
            <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth={2} strokeLinecap="round" strokeLinejoin="round" aria-hidden>
              <path d="M14 2H6a2 2 0 00-2 2v16a2 2 0 002 2h12a2 2 0 002-2V8z" /><polyline points="14 2 14 8 20 8" /><line x1="16" y1="13" x2="8" y2="13" /><line x1="16" y1="17" x2="8" y2="17" /><polyline points="10 9 9 9 8 9" />
            </svg>
            {voucher.description}
          </div>
        )}

        {/* Lines table */}
        <div className={styles.linesWrap}>
          <table className={styles.linesTable}>
            <thead>
              <tr className={styles.linesHead}>
                <th className={styles.lt}>Cuenta</th>
                <th className={styles.lt} style={{ textAlign: 'right', width: 130 }}>Débito</th>
                <th className={styles.lt} style={{ textAlign: 'right', width: 130 }}>Crédito</th>
              </tr>
            </thead>
            <tbody>
              {voucher.lines.map(line => (
                <tr key={line.id} className={styles.lineRow}>
                  <td className={styles.ld}>
                    <span className={styles.accountCode}>{line.accountCode}</span>
                    <span className={styles.accountName}>{line.accountName}</span>
                  </td>
                  <td className={styles.ld} style={{ textAlign: 'right' }}>
                    {line.lineType === 'DEBIT'
                      ? <span className={styles.debitAmt}>{fmt(line.amount)}</span>
                      : <span className={styles.zero}>—</span>
                    }
                  </td>
                  <td className={styles.ld} style={{ textAlign: 'right' }}>
                    {line.lineType === 'CREDIT'
                      ? <span className={styles.creditAmt}>{fmt(line.amount)}</span>
                      : <span className={styles.zero}>—</span>
                    }
                  </td>
                </tr>
              ))}
            </tbody>
            <tfoot>
              <tr className={styles.totalsRow}>
                <td className={styles.ld}><span className={styles.totalsLabel}>Totales</span></td>
                <td className={styles.ld} style={{ textAlign: 'right' }}><span className={styles.debitAmt}>{fmt(voucher.totalDebit)}</span></td>
                <td className={styles.ld} style={{ textAlign: 'right' }}><span className={styles.creditAmt}>{fmt(voucher.totalCredit)}</span></td>
              </tr>
            </tfoot>
          </table>
        </div>

        {/* Footer actions */}
        <div className={styles.footer}>
          <button className={styles.closeAction} onClick={onClose}>Cerrar</button>

          {voucher.status === 'DRAFT' && (
            <>
              <button
                className={styles.cancelAction}
                onClick={() => onCancel(voucher.id)}
                disabled={isCancelPending}
              >
                {isCancelPending ? 'Cancelando…' : 'Cancelar asiento'}
              </button>
              <button
                className={styles.postAction}
                onClick={() => onPost(voucher.id)}
                disabled={isPostPending}
                aria-busy={isPostPending}
              >
                {isPostPending ? (
                  <><span className={styles.spinner} aria-hidden />Publicando…</>
                ) : (
                  <>
                    <svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth={2.5} strokeLinecap="round" strokeLinejoin="round" aria-hidden>
                      <polyline points="20 6 9 17 4 12" />
                    </svg>
                    Publicar asiento
                  </>
                )}
              </button>
            </>
          )}
        </div>
      </div>
    </div>
  );
};
