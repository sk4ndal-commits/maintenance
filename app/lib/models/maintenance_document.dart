class MaintenanceDocument {
  final String id;
  final String workOrderId;
  final String fileName;
  final String contentType;
  final int fileSizeBytes;
  final String? notes;
  final String uploadedAt;
  final String uploadedBy;

  const MaintenanceDocument({
    required this.id,
    required this.workOrderId,
    required this.fileName,
    required this.contentType,
    required this.fileSizeBytes,
    this.notes,
    required this.uploadedAt,
    required this.uploadedBy,
  });

  factory MaintenanceDocument.fromJson(Map<String, dynamic> j) => MaintenanceDocument(
        id: j['id'] as String,
        workOrderId: j['workOrderId'] as String,
        fileName: j['fileName'] as String,
        contentType: j['contentType'] as String,
        fileSizeBytes: j['fileSizeBytes'] as int,
        notes: j['notes'] as String?,
        uploadedAt: j['uploadedAt'] as String,
        uploadedBy: j['uploadedBy'] as String,
      );
}
