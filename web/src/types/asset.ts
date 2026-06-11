export interface Asset {
  assetId: string
  name: string
  type: string
  location: string
  description?: string
  qrCodePayload: string
  createdAt: string
}

export interface UpdateAssetPayload {
  assetId: string
  name: string
  type: string
  location: string
  description?: string
}

export interface CreateAssetPayload {
  name: string
  type: string
  location: string
  description?: string
}

export type WorkOrderStatus = 'Open' | 'Assigned' | 'InProgress' | 'Done'

export interface WorkOrder {
  workOrderId: string
  assetId: string
  title: string
  status: WorkOrderStatus
  priority: string
  createdAt: string
  completedAt?: string
}

export interface AssetListResponse {
  data: Asset[]
  total: number
  page: number
  pageSize: number
}
