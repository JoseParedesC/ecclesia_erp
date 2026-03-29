// ============================================
// features/account/components/AccountTreeRow.tsx
// ============================================

import React, { useState } from 'react';
import type { Account } from '../account.types';
import { AccountTypeBadge } from './AccountTypeBadge';
import styles from './AccountTreeRow.module.css';

interface Props {
  node: Account;
  depth: number;
  onEdit: (account: Account) => void;
  onDelete: (account: Account) => void;
}

export const AccountTreeRow: React.FC<Props> = ({ node, depth, onEdit, onDelete }) => {
  const hasChildren = (node.children?.length ?? 0) > 0;
  const [expanded, setExpanded] = useState(depth < 2);

  return (
    <>
      <tr className={`${styles.row} ${hasChildren ? styles.rowParent : ''}`}>
        {/* Code + expand toggle */}
        <td className={styles.codeCell}>
          <div className={styles.codeInner} style={{ paddingLeft: `${depth * 1.25 + 0.75}rem` }}>
            <button
              className={`${styles.expandBtn} ${!hasChildren ? styles.expandBtnHidden : ''}`}
              onClick={() => setExpanded((v) => !v)}
              aria-label={expanded ? 'Colapsar' : 'Expandir'}
              tabIndex={hasChildren ? 0 : -1}
            >
              <svg
                width="12"
                height="12"
                viewBox="0 0 24 24"
                fill="none"
                stroke="currentColor"
                strokeWidth={2.5}
                strokeLinecap="round"
                strokeLinejoin="round"
                style={{ transform: expanded ? 'rotate(90deg)' : 'rotate(0deg)', transition: 'transform 0.2s' }}
              >
                <path d="M9 18l6-6-6-6" />
              </svg>
            </button>
            <span className={`${styles.code} ${hasChildren ? styles.codeParent : ''}`}>
              {node.code}
            </span>
          </div>
        </td>

        {/* Name */}
        <td className={styles.nameCell}>
          <span className={`${styles.name} ${hasChildren ? styles.nameParent : ''}`}>
            {node.name}
          </span>
          {hasChildren && (
            <span className={styles.childCount}>{node.children!.length} subcuenta{node.children!.length !== 1 ? 's' : ''}</span>
          )}
        </td>

        {/* Type */}
        <td className={styles.typeCell}>
          <AccountTypeBadge type={node.type} />
        </td>

        {/* Actions */}
        <td className={styles.actionsCell}>
          <div className={styles.actions}>
            <button
              className={`${styles.actionBtn} ${styles.editBtn}`}
              onClick={() => onEdit(node)}
              aria-label={`Editar ${node.name}`}
              title="Editar"
            >
              <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth={2} strokeLinecap="round" strokeLinejoin="round">
                <path d="M11 4H4a2 2 0 00-2 2v14a2 2 0 002 2h14a2 2 0 002-2v-7" />
                <path d="M18.5 2.5a2.121 2.121 0 013 3L12 15l-4 1 1-4 9.5-9.5z" />
              </svg>
            </button>
            <button
              className={`${styles.actionBtn} ${styles.deleteBtn}`}
              onClick={() => onDelete(node)}
              aria-label={`Eliminar ${node.name}`}
              title="Eliminar"
              disabled={hasChildren}
            >
              <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth={2} strokeLinecap="round" strokeLinejoin="round">
                <polyline points="3 6 5 6 21 6" />
                <path d="M19 6l-1 14a2 2 0 01-2 2H8a2 2 0 01-2-2L5 6" />
                <path d="M10 11v6M14 11v6" />
                <path d="M9 6V4a1 1 0 011-1h4a1 1 0 011 1v2" />
              </svg>
            </button>
          </div>
        </td>
      </tr>

      {/* Children */}
      {hasChildren && expanded &&
        node.children!.map((child) => (
          <AccountTreeRow
            key={child.id}
            node={child}
            depth={depth + 1}
            onEdit={onEdit}
            onDelete={onDelete}
          />
        ))
      }
    </>
  );
};
