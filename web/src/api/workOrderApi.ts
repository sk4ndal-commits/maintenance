import type { WorkOrder, CreateWorkOrderPayload, WorkOrderListResponse, ChecklistStep, AddChecklistStepPayload } from '../types/asset'

const BASE_URL = import.meta.env.VITE_API_BASE_URL ?? ''

async function request<T>(path: string, init?: RequestInit): Promise<T> {
  const res = await fetch(`${BASE_URL}${path}`, {
    headers: { 'Content-Type': 'application/json' },
    ...init,
  })
  if (!res.ok) throw new Error(`API error ${res.status}`)
  return res.json()
}

export const workOrderApi = {
  create: (payload: CreateWorkOrderPayload): Promise<WorkOrder> =>
    request<WorkOrder>('/api/work-orders', { method: 'POST', body: JSON.stringify(payload) }),

  getAll: (page = 1, pageSize = 20): Promise<WorkOrderListResponse> =>
    request<WorkOrderListResponse>(`/api/work-orders?page=${page}&pageSize=${pageSize}`),

  getById: (id: string): Promise<WorkOrder> =>
    request<WorkOrder>(`/api/work-orders/${id}`),

  assign: (id: string, payload: { workOrderId: string; technicianId: string }): Promise<WorkOrder> =>
    request<WorkOrder>(`/api/work-orders/${id}/assign`, {
      method: 'PUT',
      body: JSON.stringify(payload),
    }),

  getChecklist: (id: string): Promise<ChecklistStep[]> =>
    request<ChecklistStep[]>(`/api/work-orders/${id}/checklist`),

  addChecklistStep: (id: string, payload: AddChecklistStepPayload): Promise<ChecklistStep> =>
    request<ChecklistStep>(`/api/work-orders/${id}/checklist`, {
      method: 'POST',
      body: JSON.stringify(payload),
    }),

  toggleChecklistStep: (id: string, stepId: string, payload: { workOrderId: string; stepId: string; isCompleted: boolean }): Promise<ChecklistStep> =>
    request<ChecklistStep>(`/api/work-orders/${id}/checklist/${stepId}`, {
      method: 'PUT',
      body: JSON.stringify(payload),
    }),
}
