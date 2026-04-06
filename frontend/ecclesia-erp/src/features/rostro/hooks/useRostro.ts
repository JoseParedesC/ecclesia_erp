// ============================================
// features/rostro/hooks/useRostros.ts
// ============================================

import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { RostroService } from '../rostro.service';
import type { CreateRostroRequest, UpdateRostroRequest } from '../rostro.types';

export const rostro_KEY = ['rostro'] as const;

export const useRostros = () =>
  useQuery({
    queryKey: rostro_KEY,
    queryFn: RostroService.getAll,
  });

export const useCreateRostro = () => {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (data: CreateRostroRequest) => RostroService.create(data),
    onSuccess: () => qc.invalidateQueries({ queryKey: rostro_KEY }),
  });
};

export const useUpdateRostro = () => {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: ({ id, data }: { id: string; data: UpdateRostroRequest }) =>
      RostroService.update(id, data),
    onSuccess: () => qc.invalidateQueries({ queryKey: rostro_KEY }),
  });
};

export const useDeleteRostro = () => {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (id: string) => RostroService.remove(id),
    onSuccess: () => qc.invalidateQueries({ queryKey: rostro_KEY }),
  });
};
