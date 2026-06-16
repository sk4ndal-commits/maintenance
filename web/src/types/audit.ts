export interface AuditLog {
  id: string
  action: string
  entityId: string
  userId: string
  details: string
  createdAt: string
}
