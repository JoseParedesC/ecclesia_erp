// ============================================
// shared/services/apiClient.ts
// ============================================

import { authService } from '../../features/auth/auth.service';

// ── Types ──────────────────────────────────

export interface ApiResponse<T> {
  data: T;
  message: string;
  errors: string[];
}

type RequestOptions = Omit<RequestInit, 'body'> & {
  body?: unknown;
};

// ── Errors ─────────────────────────────────

export class ApiError extends Error {
  constructor(
    public readonly status: number,
    message: string,
    public readonly errors: string[] = [],
  ) {
    super(message);
    this.name = 'ApiError';
  }
}

export class UnauthorizedError extends ApiError {
  constructor() {
    super(401, 'Sesión expirada. Por favor inicia sesión nuevamente.');
    this.name = 'UnauthorizedError';
  }
}

export class NetworkError extends ApiError {
  constructor() {
    super(0, 'No se pudo conectar con el servidor. Verifica tu conexión.');
    this.name = 'NetworkError';
  }
}

// ── Helpers ────────────────────────────────

function buildHeaders(options: RequestOptions): HeadersInit {
  const headers: Record<string, string> = {
    'Content-Type': 'application/json',
    ...(options.headers as Record<string, string>),
  };

  const token = authService.getToken();
  if (token) {
    headers['Authorization'] = `Bearer ${token}`;
  }

  return headers;
}

async function parseResponse<T>(res: Response): Promise<T> {
  const text = await res.text();

  if (!text) return undefined as T;

  try {
    const json = JSON.parse(text) as ApiResponse<T>;
    return json.data ?? (json as T);
  } catch {
    throw new ApiError(res.status, 'Respuesta inesperada del servidor.');
  }
}

async function handleError(res: Response): Promise<never> {
  if (res.status === 401) {
    authService.clearToken();
    throw new UnauthorizedError();
  }

  let message = `Error ${res.status}`;
  let errors: string[] = [];

  try {
    const body = await res.json() as Partial<ApiResponse<unknown>>;

    errors = body.errors ?? [];

    // 🔥 FIX AQUÍ
    message = body.message 
      ?? (Array.isArray(errors) && errors.length > 0 ? errors[0] : message);

  } catch {
    // body no es JSON
  }

  throw new ApiError(res.status, message, errors);
}

// ── Core fetch ─────────────────────────────

async function request<T>(
  endpoint: string,
  options: RequestOptions = {},
): Promise<T> {
  const base = import.meta.env.VITE_API_URL;
  const url  = `${base}${endpoint}`;

  const { body, ...rest } = options;

  const config: RequestInit = {
    ...rest,
    headers: buildHeaders(options),
    ...(body !== undefined ? { body: JSON.stringify(body) } : {}),
  };

  let res: Response;

  try {
    res = await fetch(url, config);
  } catch {
    throw new NetworkError();
  }

  if (!res.ok) {
    await handleError(res);
  }

  return parseResponse<T>(res);
}

// ── Public API ─────────────────────────────

export const apiClient = {
  get<T>(endpoint: string, options?: RequestOptions) {
    return request<T>(endpoint, { ...options, method: 'GET' });
  },

  post<T>(endpoint: string, body: unknown, options?: RequestOptions) {
    return request<T>(endpoint, { ...options, method: 'POST', body });
  },

  put<T>(endpoint: string, body: unknown, options?: RequestOptions) {
    return request<T>(endpoint, { ...options, method: 'PUT', body });
  },

  patch<T>(endpoint: string, body: unknown, options?: RequestOptions) {
    return request<T>(endpoint, { ...options, method: 'PATCH', body });
  },

  delete<T>(endpoint: string, options?: RequestOptions) {
    return request<T>(endpoint, { ...options, method: 'DELETE' });
  },
};
