import type { Technician } from '../types/asset'

const BASE_URL = import.meta.env.VITE_API_BASE_URL ?? ''

async function request<T>(path: string, init?: RequestInit): Promise<T> {
  const token = localStorage.getItem('jwt')
  const res = await fetch(`${BASE_URL}${path}`, {
    headers: {
      'Content-Type': 'application/json',
      ...(token ? { Authorization: `Bearer ${token}` } : {}),
    },
    ...init,
  })
  if (!res.ok) throw new Error(`API error ${res.status}`)
  return res.json()
}

export const technicianApi = {
  getAll: (): Promise<Technician[]> =>
    request<Technician[]>('/api/technicians'),

  create: (name: string, email: string): Promise<Technician> =>
    request<Technician>('/api/technicians', {
      method: 'POST',
      body: JSON.stringify({ name, email }),
    }),
}
