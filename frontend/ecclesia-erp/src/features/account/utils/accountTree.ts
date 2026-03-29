// ============================================
// features/account/utils/accountTree.ts
// ============================================

import type { Account, AccountFilters } from '../account.types';

/** Builds a nested tree from a flat list */
export function buildTree(flat: Account[]): Account[] {
  const map = new Map<string, Account>();
  flat.forEach((a) => map.set(a.id, { ...a, children: [] }));

  const roots: Account[] = [];
  map.forEach((node) => {
    if (node.parentAccountId && map.has(node.parentAccountId)) {
      map.get(node.parentAccountId)!.children!.push(node);
    } else {
      roots.push(node);
    }
  });

  // Sort by code at every level
  const sort = (nodes: Account[]) => {
    nodes.sort((a, b) => a.code.localeCompare(b.code, undefined, { numeric: true }));
    nodes.forEach((n) => n.children?.length && sort(n.children));
  };
  sort(roots);
  return roots;
}

/** Flatten tree back to array (for search) */
export function flattenTree(nodes: Account[]): Account[] {
  return nodes.flatMap((n) => [n, ...flattenTree(n.children ?? [])]);
}

/** Filter flat list by search + type, then rebuild tree */
export function filterAccounts(flat: Account[], filters: AccountFilters): Account[] {
  const { search = '', type = '' } = filters;
  const q = search.toLowerCase().trim();

  if (!q && !type) return buildTree(flat);

  // Collect matching ids + all their ancestors
  const matchingIds = new Set<string>();

  const idMap = new Map(flat.map((a) => [a.id, a]));

  const addAncestors = (id: string | null) => {
    if (!id) return;
    const a = idMap.get(id);
    if (!a) return;
    matchingIds.add(id);
    addAncestors(a.parentAccountId);
  };

  flat.forEach((a) => {
    const matchesSearch = !q || a.code.toLowerCase().includes(q) || a.name.toLowerCase().includes(q);
    const matchesType = !type || a.type === type;
    if (matchesSearch && matchesType) {
      matchingIds.add(a.id);
      addAncestors(a.parentAccountId);
    }
  });

  return buildTree(flat.filter((a) => matchingIds.has(a.id)));
}
