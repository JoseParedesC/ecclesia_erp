// ============================================
// features/journalVoucher/components/LinesEditor.tsx
// ============================================

import React from 'react';
import type { JournalVoucherLineInput, LineType } from '../journalVoucher.types';
import { lineTotals, isBalanced } from '../journalVoucher.types';
import type { Account } from '../../account/account.types';
import styles from './LinesEditor.module.css';

const fmt = (n: number) =>
  new Intl.NumberFormat('es-CO', { minimumFractionDigits: 2, maximumFractionDigits: 2 }).format(n);

interface Props {
  lines:    JournalVoucherLineInput[];
  accounts: Account[];
  onChange: (lines: JournalVoucherLineInput[]) => void;
}

export const LinesEditor: React.FC<Props> = ({ lines, accounts, onChange }) => {
  const { debit, credit } = lineTotals(lines);
  const balanced          = isBalanced(lines);
  const diff              = Math.abs(debit - credit);

  const update = (idx: number, patch: Partial<JournalVoucherLineInput>) => {
    onChange(lines.map((l, i) => i === idx ? { ...l, ...patch } : l));
  };

  const addLine = (type: LineType) => {
    onChange([...lines, { accountId: '', lineType: type, amount: 0 }]);
  };

  const removeLine = (idx: number) => {
    onChange(lines.filter((_, i) => i !== idx));
  };

  // Sorted accounts for the selector
  const sortedAccounts = [...accounts].sort((a, b) =>
    a.code.localeCompare(b.code, undefined, { numeric: true }),
  );

  return (
    <div className={styles.wrap}>
      {/* Table */}
      <div className={styles.tableScroll}>
        <table className={styles.table}>
          <thead>
            <tr className={styles.thead}>
              <th className={styles.th} style={{ width: 36 }}>#</th>
              <th className={styles.th}>Cuenta contable</th>
              <th className={styles.th} style={{ width: 100 }}>Tipo</th>
              <th className={styles.th} style={{ width: 150 }}>Monto</th>
              <th className={styles.th} style={{ width: 36 }} />
            </tr>
          </thead>
          <tbody>
            {lines.length === 0 && (
              <tr>
                <td colSpan={5} className={styles.emptyRow}>
                  Agrega al menos una línea de débito y una de crédito
                </td>
              </tr>
            )}
            {lines.map((line, idx) => (
              <tr key={idx} className={`${styles.row} ${line.lineType === 'DEBIT' ? styles.rowDebit : styles.rowCredit}`}>
                <td className={styles.td}>
                  <span className={`${styles.lineNum} ${line.lineType === 'DEBIT' ? styles.lineNumD : styles.lineNumC}`}>
                    {idx + 1}
                  </span>
                </td>

                {/* Account selector */}
                <td className={styles.td}>
                  <select
                    className={`${styles.select} ${!line.accountId ? styles.selectEmpty : ''}`}
                    value={line.accountId}
                    onChange={e => update(idx, { accountId: e.target.value })}
                    aria-label={`Cuenta línea ${idx + 1}`}
                  >
                    <option value="">— Seleccionar cuenta —</option>
                    {sortedAccounts.map(a => (
                      <option key={a.id} value={a.id}>
                        {a.code} — {a.name}
                      </option>
                    ))}
                  </select>
                </td>

                {/* Line type */}
                <td className={styles.td}>
                  <select
                    className={styles.typeSelect}
                    value={line.lineType}
                    onChange={e => update(idx, { lineType: e.target.value as LineType })}
                    aria-label={`Tipo línea ${idx + 1}`}
                  >
                    <option value="DEBIT">Débito</option>
                    <option value="CREDIT">Crédito</option>
                  </select>
                </td>

                {/* Amount */}
                <td className={styles.td}>
                  <input
                    type="number"
                    className={styles.amountInput}
                    min={0}
                    step={0.01}
                    value={line.amount || ''}
                    placeholder="0.00"
                    onChange={e => update(idx, { amount: parseFloat(e.target.value) || 0 })}
                    aria-label={`Monto línea ${idx + 1}`}
                  />
                </td>

                {/* Remove */}
                <td className={styles.td}>
                  <button
                    type="button"
                    className={styles.removeBtn}
                    onClick={() => removeLine(idx)}
                    aria-label={`Eliminar línea ${idx + 1}`}
                    title="Eliminar línea"
                  >
                    <svg width="12" height="12" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth={2.5} strokeLinecap="round">
                      <line x1="18" y1="6" x2="6" y2="18" /><line x1="6" y1="6" x2="18" y2="18" />
                    </svg>
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      {/* Add line buttons */}
      <div className={styles.addRow}>
        <button type="button" className={`${styles.addBtn} ${styles.addDebit}`} onClick={() => addLine('DEBIT')}>
          <svg width="12" height="12" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth={2.5} strokeLinecap="round" aria-hidden>
            <line x1="12" y1="5" x2="12" y2="19" /><line x1="5" y1="12" x2="19" y2="12" />
          </svg>
          Débito
        </button>
        <button type="button" className={`${styles.addBtn} ${styles.addCredit}`} onClick={() => addLine('CREDIT')}>
          <svg width="12" height="12" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth={2.5} strokeLinecap="round" aria-hidden>
            <line x1="12" y1="5" x2="12" y2="19" /><line x1="5" y1="12" x2="19" y2="12" />
          </svg>
          Crédito
        </button>
      </div>

      {/* Balance footer */}
      <div className={`${styles.balanceBar} ${balanced && lines.length > 0 ? styles.balanceOk : lines.length > 0 ? styles.balanceErr : ''}`}>
        <div className={styles.balanceItem}>
          <span className={styles.balanceLabel}>Total débitos</span>
          <span className={`${styles.balanceVal} ${styles.debitVal}`}>{fmt(debit)}</span>
        </div>
        <div className={styles.balanceDivider} aria-hidden />
        <div className={styles.balanceItem}>
          <span className={styles.balanceLabel}>Total créditos</span>
          <span className={`${styles.balanceVal} ${styles.creditVal}`}>{fmt(credit)}</span>
        </div>
        <div className={styles.balanceDivider} aria-hidden />
        <div className={styles.balanceItem}>
          {balanced && lines.length > 0 ? (
            <span className={styles.balancedTag}>
              <svg width="12" height="12" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth={2.5} strokeLinecap="round" strokeLinejoin="round" aria-hidden>
                <polyline points="20 6 9 17 4 12" />
              </svg>
              Balanceado
            </span>
          ) : lines.length > 0 ? (
            <span className={styles.unbalancedTag}>
              <svg width="12" height="12" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth={2} strokeLinecap="round" aria-hidden>
                <line x1="12" y1="8" x2="12" y2="12" /><line x1="12" y1="16" x2="12.01" y2="16" />
              </svg>
              Diferencia: {fmt(diff)}
            </span>
          ) : (
            <span className={styles.balanceLabel}>Sin líneas</span>
          )}
        </div>
      </div>
    </div>
  );
};
