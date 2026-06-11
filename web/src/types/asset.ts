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

export type WorkOrderPriority = 'Low' | 'Medium' | 'High'

export interface Technician {
  technicianId: string
  name: string
  email: string
}

export interface AssignWorkOrderPayload {
  workOrderId: string
  technicianId: string
}

export interface WorkOrder {
  workOrderId: string
  assetId: string
  title: string
  status: WorkOrderStatus
  priority: WorkOrderPriority
  description?: string
  assignedTechnicianId?: string
  assignedTechnicianName?: string
  createdAt: string
  completedAt?: string
  completionNotes?: string
  dueDate?: string
}

export interface CreateWorkOrderPayload {
  assetId: string
  title: string
  priority: WorkOrderPriority
  description?: string
  dueDate?: string
}

export interface WorkOrderListResponse {
  data: WorkOrder[]
  total: number
  page: number
  pageSize: number
}

export interface AssetListResponse {
  data: Asset[]
  total: number
  page: number
  pageSize: number
}
