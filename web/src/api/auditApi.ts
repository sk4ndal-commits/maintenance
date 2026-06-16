import type { AuditLog } from '../types/audit'

const BASE_URL = import.meta.env.VITE_API_BASE_URL ?? 'http://localhost:5000'

async function request<T>(path: string, options?: RequestInit): Promise<T> {
  const token = localStorage.getItem('jwt')
  const res = await fetch(`${BASE_URL}${path}`, {
    headers: {
      'Content-Type': 'application/json',
      ...(token ? { Authorization: `Bearer ${token}` } : {}),
    },
    ...options,
  })
  if (!res.ok) {
    const text = await res.text()
    throw new Error(`API error ${res.status}: ${text}`)
  }
  return res.json() as Promise<T>
}

export const auditApi = {
  getByEntity: (entityId: string) => request<AuditLog[]>(`/api/audit/entity/${entityId}`),
  getAll: (page = 1) => request<AuditLog[]>(`/api/audit?page=${page}`),
}
