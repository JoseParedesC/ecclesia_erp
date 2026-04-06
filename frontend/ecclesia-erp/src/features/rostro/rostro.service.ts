// ============================================
// features/Rostro/Rostro.service.ts
// ============================================

import { apiClient } from '../../shared/services/apiClient';
import type {
  Rostro,
  CreateRostroRequest,
  UpdateRostroRequest,
} from './rostro.types';

const BASE = '/api/Rostros';

export const RostroService = {
  getAll(): Promise<Rostro[]> {
    return apiClient.get<Rostro[]>(BASE);
  },

  getById(id: string): Promise<Rostro> {
    return apiClient.get<Rostro>(`${BASE}/${id}`);
  },

  create(data: CreateRostroRequest): Promise<Rostro> {
    return apiClient.post<Rostro>(BASE, data);
  },

  update(id: string, data: UpdateRostroRequest): Promise<Rostro> {
    return apiClient.put<Rostro>(`${BASE}/${id}`, data);
  },

  remove(id: string): Promise<void> {
    return apiClient.delete<void>(`${BASE}/${id}`);
  },
};
