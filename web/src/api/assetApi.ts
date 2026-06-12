import type { Asset, AssetListResponse, CreateAssetPayload, UpdateAssetPayload, WorkOrder } from '../types/asset'

export interface HistoryEvent {
  id: string
  timestamp: string
  eventType: string
  title: string
  details?: string
  workOrderId?: string
  workOrderTitle?: string
  actor?: string
}

export interface HistoryResponse {
  data: HistoryEvent[]
  total: number
  page: number
  pageSize: number
}

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

export const assetApi = {
  create(payload: CreateAssetPayload): Promise<Asset> {
    return request<Asset>('/api/assets', {
      method: 'POST',
      body: JSON.stringify(payload),
    })
  },

  getAll(page = 1, pageSize = 20): Promise<AssetListResponse> {
    return request<AssetListResponse>(`/api/assets?page=${page}&pageSize=${pageSize}`)
  },

  getById(id: string): Promise<Asset> {
    return request<Asset>(`/api/assets/${id}`)
  },

  update(id: string, payload: UpdateAssetPayload): Promise<Asset> {
    return request<Asset>(`/api/assets/${id}`, {
      method: 'PUT',
      body: JSON.stringify(payload),
    })
  },

  getWorkOrders(id: string, limit = 10): Promise<WorkOrder[]> {
    return request<WorkOrder[]>(`/api/assets/${id}/work-orders?limit=${limit}`)
  },

  async downloadQrCode(id: string): Promise<string> {
    const token = localStorage.getItem('jwt')
    const res = await fetch(`${BASE_URL}/api/assets/${id}/qr-code`, {
      headers: token ? { Authorization: `Bearer ${token}` } : {},
    })
    if (!res.ok) throw new Error(`QR error ${res.status}`)
    const blob = await res.blob()
    return URL.createObjectURL(blob)
  },

  delete(id: string): Promise<void> {
    return request<void>(`/api/assets/${id}`, { method: 'DELETE' })
  },

  getHistory(
    id: string,
    params?: { eventType?: string; search?: string; page?: number; pageSize?: number }
  ): Promise<HistoryResponse> {
    const qs = new URLSearchParams()
    if (params?.eventType) qs.set('eventType', params.eventType)
    if (params?.search) qs.set('search', params.search)
    if (params?.page) qs.set('page', String(params.page))
    if (params?.pageSize) qs.set('pageSize', String(params.pageSize))
    return request<HistoryResponse>(`/api/assets/${id}/history?${qs}`)
  },
}
