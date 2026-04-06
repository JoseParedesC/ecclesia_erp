// ============================================
// features/Community/hooks/useCommunities.ts
// ============================================

import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { ComminityService } from '../community.service';
import type { CreateComminityRequest, UpdateComminityRequest } from '../community.types';

export const COMMUNITY_KEY = ['community'] as const;

export const useCommunities = () =>
  useQuery({
    queryKey: COMMUNITY_KEY,
    queryFn: ComminityService.getAll,
  });

export const useCreateComminity = () => {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (data: CreateComminityRequest) => ComminityService.create(data),
    onSuccess: () => qc.invalidateQueries({ queryKey: COMMUNITY_KEY }),
  });
};

export const useUpdateComminity = () => {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: ({ id, data }: { id: string; data: UpdateComminityRequest }) =>
      ComminityService.update(id, data),
    onSuccess: () => qc.invalidateQueries({ queryKey: COMMUNITY_KEY }),
  });
};

export const useDeleteComminity = () => {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (id: string) => ComminityService.remove(id),
    onSuccess: () => qc.invalidateQueries({ queryKey: COMMUNITY_KEY }),
  });
};
