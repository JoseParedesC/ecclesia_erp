// ============================================
// features/dashboard/config/navItems.ts
// ============================================

export interface NavItem {
  key: string;
  label: string;
  icon: string;  // SVG path d="" string
  path: string;
  children?: NavItem[];
}

export const icons = {
  home: 'M3 12l2-2m0 0l7-7 7 7M5 10v10a1 1 0 001 1h3m10-11l2 2m-2-2v10a1 1 0 01-1 1h-3m-6 0a1 1 0 001-1v-4a1 1 0 011-1h2a1 1 0 011 1v4a1 1 0 001 1m-6 0h6',
  user: 'M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z',
  usersGroup: 'M18 18.72a9.094 9.094 0 0 0 3.741-.479 3 3 0 0 0-4.682-2.72m.94 3.198.001.031c0 .225-.012.447-.037.666A11.944 11.944 0 0 1 12 21c-2.17 0-4.207-.576-5.963-1.584A6.062 6.062 0 0 1 6 18.719m12 0a5.971 5.971 0 0 0-.941-3.197m0 0A5.995 5.995 0 0 0 12 12.75a5.995 5.995 0 0 0-5.058 2.772m0 0a3 3 0 0 0-4.681 2.72 8.986 8.986 0 0 0 3.74.477m.94-3.197a5.971 5.971 0 0 0-.94 3.197M15 6.75a3 3 0 1 1-6 0 3 3 0 0 1 6 0Zm6 3a2.25 2.25 0 1 1-4.5 0 2.25 2.25 0 0 1 4.5 0Zm-13.5 0a2.25 2.25 0 1 1-4.5 0 2.25 2.25 0 0 1 4.5 0Z',
  accounting: 'M9 7h6m0 10v-3m-3 3h.01M9 17h.01M9 14h.01M12 14h.01M15 11h.01M12 11h.01M9 11h.01M7 21h10a2 2 0 002-2V5a2 2 0 00-2-2H7a2 2 0 00-2 2v14a2 2 0 002 2z',
  stack: 'M4 6h16M4 10h16M4 14h16M4 18h16',
  calendar: 'M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z',
  voucher: 'M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z',
  finance: 'M12 8c-1.657 0-3 .895-3 2s1.343 2 3 2 3 .895 3 2-1.343 2-3 2m0-8c1.11 0 2.08.402 2.599 1M12 8V7m0 1v8m0 0v1m0-1c-1.11 0-2.08-.402-2.599-1M21 12a9 9 0 11-18 0 9 9 0 0118 0z',
  arrowUp: 'M7 11l5-5m0 0l5 5m-5-5v12',
  arrowDown: 'M19 14l-7 7m0 0l-7-7m7 7V3',
  window: 'M3 10h18M7 15h1m4 0h1m-7 4h12a3 3 0 003-3V8a3 3 0 00-3-3H6a3 3 0 00-3 3v8a3 3 0 003 3z',
  barDiagram: 'M9 19v-6a2 2 0 00-2-2H5a2 2 0 00-2 2v6a2 2 0 002 2h2a2 2 0 002-2zm0 0V9a2 2 0 012-2h2a2 2 0 012 2v10m-6 0a2 2 0 002 2h2a2 2 0 002-2m0 0V5a2 2 0 012-2h2a2 2 0 012 2v14a2 2 0 01-2 2h-2a2 2 0 01-2-2z',
  community: 'M17 20h5v-2a3 3 0 00-5.356-1.857M17 20H7m10 0v-2c0-.656-.126-1.283-.356-1.857M7 20H2v-2a3 3 0 015.356-1.857M7 20v-2c0-.656.126-1.283.356-1.857m0 0a5.002 5.002 0 019.288 0M15 7a3 3 0 11-6 0 3 3 0 016 0z',
  bank: 'M19 21V5a2 2 0 00-2-2H7a2 2 0 00-2 2v16m14 0h2m-2 0h-5m-9 0H3m2 0h5M9 7h1m-1 4h1m4-4h1m-1 4h1m-5 10v-5a1 1 0 011-1h2a1 1 0 011 1v5m-4 0h4',
  locked: 'M16.5 10.5V6.75a4.5 4.5 0 1 0-9 0v3.75m-.75 11.25h10.5a2.25 2.25 0 0 0 2.25-2.25v-6.75a2.25 2.25 0 0 0-2.25-2.25H6.75a2.25 2.25 0 0 0-2.25 2.25v6.75a2.25 2.25 0 0 0 2.25 2.25Z',
  engine: 'M10.325 4.317c.426-1.756 2.924-1.756 3.35 0a1.724 1.724 0 002.573 1.066c1.543-.94 3.31.826 2.37 2.37a1.724 1.724 0 001.065 2.572c1.756.426 1.756 2.924 0 3.35a1.724 1.724 0 00-1.066 2.573c.94 1.543-.826 3.31-2.37 2.37a1.724 1.724 0 00-2.572 1.065c-.426 1.756-2.924 1.756-3.35 0a1.724 1.724 0 00-2.573-1.066c-1.543.94-3.31-.826-2.37-2.37a1.724 1.724 0 00-1.065-2.572c-1.756-.426-1.756-2.924 0-3.35a1.724 1.724 0 001.066-2.573c-.94-1.543.826-3.31 2.37-2.37.996.608 2.296.07 2.572-1.065z M15 12a3 3 0 11-6 0 3 3 0 016 0z',
  success: 'M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z'
};

export const navItems: NavItem[] = [
  {
    key: 'overview',
    label: 'Resumen',
    path: '/',
    icon: icons.home,
  },
  {
    key: 'accounting',
    label: 'Contabilidad',
    path: '/accounting',
    icon: icons.accounting,
    children: [
      {
        key: 'accounts',
        label: 'Plan de cuentas',
        path: '/accounting/accounts',
        icon: icons.stack,
      },
      {
        key: 'periods',
        label: 'Períodos',
        path: '/accounting/periods',
        icon: icons.calendar,
      },
      {
        key: 'vouchers',
        label: 'Asientos',
        path: '/accounting/vouchers',
        icon: icons.voucher,
      },
    ],
  },
  {
    key: 'finance',
    label: 'Finanzas',
    path: '/finance',
    icon: icons.finance,
    children: [
      {
        key: 'income',
        label: 'Ingresos',
        path: '/finance/income',
        icon: icons.arrowUp,
      },
      {
        key: 'expense',
        label: 'Gastos',
        path: '/finance/expense',
        icon: icons.arrowDown,
      },
      {
        key: 'cash',
        label: 'Cajas y bancos',
        path: '/finance/cash',
        icon: icons.window,
      },
      {
        key: 'budget',
        label: 'Presupuesto',
        path: '/finance/budget',
        icon: icons.barDiagram,
      },
    ],
  },
  {
    key: 'community',
    label: 'Comunidad',
    path: '/community',
    icon: icons.community,
    children: [
      {
        key: 'members',
        label: 'Miembros',
        path: '/community/members',
        icon: icons.user,
      },
      {
        key: 'third-parties',
        label: 'Terceros',
        path: '/community/third-parties',
        icon: icons.bank,
      },
    ],
  },
  {
    key: 'approvals',
    label: 'Aprobaciones',
    path: '/approvals',
    icon: icons.success,
  },
  {
    key: 'settings',
    label: 'Configuración',
    path: '/settings',
    icon: icons.engine,
    children: [
      {
        key: 'users',
        label: 'Usuarios',
        path: '/settings/users',
        icon: icons.user,
      },
      {
        key: 'roles',
        label: 'Roles',
        path: '/settings/roles',
        icon: icons.usersGroup,
      },
      {
        key: 'permissions',
        label: 'Permisos',
        path: '/settings/permissions',
        icon: icons.locked,
      },
    ],
  },
];
