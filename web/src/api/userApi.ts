import type { User, CreateUserPayload, UpdateUserPayload } from '../types/asset'

const BASE_URL = import.meta.env.VITE_API_BASE_URL ?? ''

function authHeaders(): HeadersInit {
  const token = localStorage.getItem('jwt')
  return {
    'Content-Type': 'application/json',
    ...(token ? { Authorization: `Bearer ${token}` } : {}),
  }
}

async function request<T>(path: string, init?: RequestInit): Promise<T> {
  const res = await fetch(`${BASE_URL}${path}`, {
    headers: authHeaders(),
    ...init,
  })
  if (!res.ok) throw new Error(`API error ${res.status}`)
  return res.json()
}

async function requestNoContent(path: string, init?: RequestInit): Promise<void> {
  const res = await fetch(`${BASE_URL}${path}`, {
    headers: authHeaders(),
    ...init,
  })
  if (!res.ok) throw new Error(`API error ${res.status}`)
}

export const userApi = {
  register: (name: string, email: string, password: string): Promise<void> =>
    requestNoContent('/api/users/register', {
      method: 'POST',
      body: JSON.stringify({ name, email, password }),
    }),

  login: (email: string, password: string): Promise<{ token: string }> =>
    request<{ token: string }>('/api/users/login', {
      method: 'POST',
      body: JSON.stringify({ email, password }),
    }),

  getAll: (): Promise<User[]> =>
    request<User[]>('/api/users'),

  create: (data: CreateUserPayload): Promise<User> =>
    request<User>('/api/users', {
      method: 'POST',
      body: JSON.stringify(data),
    }),

  update: (id: string, data: UpdateUserPayload): Promise<void> =>
    requestNoContent(`/api/users/${id}`, {
      method: 'PUT',
      body: JSON.stringify(data),
    }),

  setActive: (id: string, isActive: boolean): Promise<void> =>
    requestNoContent(`/api/users/${id}/active`, {
      method: 'PATCH',
      body: JSON.stringify({ isActive }),
    }),

  resetPassword: (id: string, newPassword: string): Promise<void> =>
    requestNoContent(`/api/users/${id}/password`, {
      method: 'PATCH',
      body: JSON.stringify({ newPassword }),
    }),
}
