class ChecklistStep {
  final String id;
  final String workOrderId;
  final String label;
  final bool isMandatory;
  final bool requiresPhoto;
  final bool isCompleted;
  final String? completedAt;

  const ChecklistStep({
    required this.id,
    required this.workOrderId,
    required this.label,
    required this.isMandatory,
    required this.requiresPhoto,
    required this.isCompleted,
    this.completedAt,
  });

  factory ChecklistStep.fromJson(Map<String, dynamic> j) => ChecklistStep(
        id: j['id'] as String,
        workOrderId: j['workOrderId'] as String,
        label: j['label'] as String,
        isMandatory: j['isMandatory'] as bool,
        requiresPhoto: j['requiresPhoto'] as bool? ?? false,
        isCompleted: j['isCompleted'] as bool,
        completedAt: j['completedAt'] as String?,
      );

  ChecklistStep copyWith({bool? isCompleted}) => ChecklistStep(
        id: id,
        workOrderId: workOrderId,
        label: label,
        isMandatory: isMandatory,
        requiresPhoto: requiresPhoto,
        isCompleted: isCompleted ?? this.isCompleted,
        completedAt: completedAt,
      );
}
