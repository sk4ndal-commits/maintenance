export interface Asset {
  assetId: string
  name: string
  type: string
  location: string
  description?: string
  qrCodePayload: string
  createdAt: string
}

export interface CreateAssetPayload {
  name: string
  type: string
  location: string
  description?: string
}

export interface AssetListResponse {
  data: Asset[]
  total: number
  page: number
  pageSize: number
}
