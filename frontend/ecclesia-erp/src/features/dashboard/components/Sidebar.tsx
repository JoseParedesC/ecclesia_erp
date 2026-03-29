// ============================================
// features/dashboard/components/Sidebar.tsx
// ============================================

import React from 'react';
import { NavLink, useLocation } from 'react-router-dom';
import { navItems } from '../config/navItems';
import type { NavItem } from '../config/navItems';
import styles from './Sidebar.module.css';

interface SidebarProps {
  collapsed: boolean;
  onToggle: () => void;
  openGroups: (key: string) => boolean;
  onToggleGroup: (key: string) => void;
}

const NavIcon: React.FC<{ d: string }> = ({ d }) => (
  <svg
    className={styles.icon}
    viewBox="0 0 24 24"
    fill="none"
    stroke="currentColor"
    strokeWidth={1.6}
    strokeLinecap="round"
    strokeLinejoin="round"
    aria-hidden
  >
    <path d={d} />
  </svg>
);

const ChevronIcon: React.FC<{ open: boolean }> = ({ open }) => (
  <svg
    className={`${styles.chevron} ${open ? styles.chevronOpen : ''}`}
    viewBox="0 0 24 24"
    fill="none"
    stroke="currentColor"
    strokeWidth={2}
    strokeLinecap="round"
    strokeLinejoin="round"
    aria-hidden
  >
    <path d="M9 18l6-6-6-6" />
  </svg>
);

interface NavGroupProps {
  item: NavItem;
  collapsed: boolean;
  isOpen: boolean;
  onToggle: () => void;
}

const NavGroup: React.FC<NavGroupProps> = ({ item, collapsed, isOpen, onToggle }) => {
  const location = useLocation();
  const isActive = item.children?.some((c) => location.pathname.startsWith(c.path)) ?? false;

  return (
    <li className={styles.group}>
      <button
        className={`${styles.groupBtn} ${isActive ? styles.groupBtnActive : ''}`}
        onClick={onToggle}
        title={collapsed ? item.label : undefined}
        aria-expanded={isOpen}
      >
        <NavIcon d={item.icon} />
        {!collapsed && <span className={styles.label}>{item.label}</span>}
        {!collapsed && <ChevronIcon open={isOpen} />}
      </button>

      {!collapsed && (
        <ul className={`${styles.subList} ${isOpen ? styles.subListOpen : ''}`}>
          {item.children!.map((child) => (
            <li key={child.key}>
              <NavLink
                to={child.path}
                className={({ isActive }) =>
                  `${styles.subLink} ${isActive ? styles.subLinkActive : ''}`
                }
              >
                <NavIcon d={child.icon} />
                <span className={styles.label}>{child.label}</span>
              </NavLink>
            </li>
          ))}
        </ul>
      )}
    </li>
  );
};

export const Sidebar: React.FC<SidebarProps> = ({
  collapsed,
  onToggle,
  openGroups,
  onToggleGroup,
}) => (
  <aside className={`${styles.sidebar} ${collapsed ? styles.collapsed : ''}`} aria-label="Navegación principal">
    {/* ── Brand ── */}
    <div className={styles.brand}>
      <div className={styles.logoMark} aria-hidden>
        <svg width="22" height="22" viewBox="0 0 36 36" fill="none">
          <path d="M18 3L33 12V24L18 33L3 24V12L18 3Z" stroke="#c9a84c" strokeWidth="1.5" fill="none" />
          <path d="M18 10L27 15.5V24L18 29L9 24V15.5L18 10Z" fill="#c9a84c" fillOpacity="0.18" stroke="#c9a84c" strokeWidth="1" />
          <line x1="18" y1="10" x2="18" y2="29" stroke="#c9a84c" strokeWidth="1" strokeOpacity="0.5" />
          <line x1="9" y1="15.5" x2="27" y2="15.5" stroke="#c9a84c" strokeWidth="1" strokeOpacity="0.5" />
        </svg>
      </div>
      {!collapsed && <span className={styles.brandName}>Ecclesia</span>}
    </div>

    <div className={styles.divider} />

    {/* ── Nav ── */}
    <nav className={styles.nav}>
      <ul className={styles.navList}>
        {navItems.map((item) =>
          item.children ? (
            <NavGroup
              key={item.key}
              item={item}
              collapsed={collapsed}
              isOpen={openGroups(item.key)}
              onToggle={() => onToggleGroup(item.key)}
            />
          ) : (
            <li key={item.key}>
              <NavLink
                to={item.path}
                end
                title={collapsed ? item.label : undefined}
                className={({ isActive }) =>
                  `${styles.link} ${isActive ? styles.linkActive : ''}`
                }
              >
                <NavIcon d={item.icon} />
                {!collapsed && <span className={styles.label}>{item.label}</span>}
              </NavLink>
            </li>
          ),
        )}
      </ul>
    </nav>

    {/* ── Toggle button ── */}
    <button
      className={styles.toggleBtn}
      onClick={onToggle}
      aria-label={collapsed ? 'Expandir menú' : 'Colapsar menú'}
    >
      <svg
        style={{ transform: collapsed ? 'rotate(180deg)' : 'rotate(0deg)', transition: 'transform .3s' }}
        width="16"
        height="16"
        viewBox="0 0 24 24"
        fill="none"
        stroke="currentColor"
        strokeWidth={2}
        strokeLinecap="round"
        strokeLinejoin="round"
        aria-hidden
      >
        <path d="M15 18l-6-6 6-6" />
      </svg>
      {!collapsed && <span className={styles.label}>Colapsar</span>}
    </button>
  </aside>
);
