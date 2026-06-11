import type { Asset, AssetListResponse, CreateAssetPayload, UpdateAssetPayload, WorkOrder } from '../types/asset'

const BASE_URL = import.meta.env.VITE_API_BASE_URL ?? 'http://localhost:5000'

async function request<T>(path: string, options?: RequestInit): Promise<T> {
  const res = await fetch(`${BASE_URL}${path}`, {
    headers: { 'Content-Type': 'application/json' },
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
    const res = await fetch(`${BASE_URL}/api/assets/${id}/qr-code`)
    if (!res.ok) throw new Error(`QR error ${res.status}`)
    const blob = await res.blob()
    return URL.createObjectURL(blob)
  },

  delete(id: string): Promise<void> {
    return request<void>(`/api/assets/${id}`, { method: 'DELETE' })
  },
}
