enum WorkOrderStatus { open, assigned, inProgress, done }

enum WorkOrderPriority { low, medium, high }

WorkOrderPriority workOrderPriorityFromString(String s) => switch (s) {
  'High'   => WorkOrderPriority.high,
  'Medium' => WorkOrderPriority.medium,
  _        => WorkOrderPriority.low,
};

WorkOrderStatus workOrderStatusFromString(String s) {
  switch (s) {
    case 'Assigned':
      return WorkOrderStatus.assigned;
    case 'InProgress':
      return WorkOrderStatus.inProgress;
    case 'Done':
      return WorkOrderStatus.done;
    case 'Open':
    default:
      return WorkOrderStatus.open;
  }
}

class WorkOrder {
  final String workOrderId;
  final String assetId;
  final String title;
  final WorkOrderStatus status;
  final WorkOrderPriority priority;
  final String? description;
  final String? assignedTechnicianId;
  final String? assignedTechnicianName;
  final String createdAt;
  final String? completedAt;
  final String? dueDate;
  final String? completionNotes;

  const WorkOrder({
    required this.workOrderId,
    required this.assetId,
    required this.title,
    required this.status,
    required this.priority,
    this.dueDate,
    this.description,
    this.assignedTechnicianId,
    this.assignedTechnicianName,
    required this.createdAt,
    this.completedAt,
    this.completionNotes,
  });

  factory WorkOrder.fromJson(Map<String, dynamic> j) => WorkOrder(
        workOrderId: j['workOrderId'] as String,
        assetId: j['assetId'] as String,
        title: j['title'] as String,
        status: workOrderStatusFromString(j['status'] as String),
        priority: workOrderPriorityFromString(j['priority'] as String),
        dueDate: j['dueDate'] as String?,
        description: j['description'] as String?,
        assignedTechnicianId: j['assignedTechnicianId'] as String?,
        assignedTechnicianName: j['assignedTechnicianName'] as String?,
        createdAt: j['createdAt'] as String,
        completedAt: j['completedAt'] as String?,
        completionNotes: j['completionNotes'] as String?,
      );
}
