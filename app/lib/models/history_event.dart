class HistoryEvent {
  final String id;
  final String timestamp;
  final String eventType;
  final String title;
  final String? details;
  final String? workOrderId;
  final String? workOrderTitle;
  final String? actor;

  const HistoryEvent({
    required this.id,
    required this.timestamp,
    required this.eventType,
    required this.title,
    this.details,
    this.workOrderId,
    this.workOrderTitle,
    this.actor,
  });

  factory HistoryEvent.fromJson(Map<String, dynamic> j) => HistoryEvent(
        id: j['id'] as String,
        timestamp: j['timestamp'] as String,
        eventType: j['eventType'] as String,
        title: j['title'] as String,
        details: j['details'] as String?,
        workOrderId: j['workOrderId'] as String?,
        workOrderTitle: j['workOrderTitle'] as String?,
        actor: j['actor'] as String?,
      );
}
