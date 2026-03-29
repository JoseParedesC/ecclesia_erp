// ============================================
// features/dashboard/hooks/useSidebar.ts
// ============================================

import { useState, useCallback } from 'react';

export const useSidebar = () => {
  const [collapsed, setCollapsed] = useState(false);
  const [openGroups, setOpenGroups] = useState<Set<string>>(new Set(['accounting']));

  const toggle = useCallback(() => setCollapsed((v) => !v), []);

  const toggleGroup = useCallback((key: string) => {
    setOpenGroups((prev) => {
      const next = new Set(prev);
      if(next.has(key)) next.delete(key)
        else next.add(key);
      return next;
    });
  }, []);

  const isGroupOpen = useCallback(
    (key: string) => !collapsed && openGroups.has(key),
    [collapsed, openGroups],
  );

  return { collapsed, toggle, toggleGroup, isGroupOpen };
};
