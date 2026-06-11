import 'package:flutter/material.dart';
import '../models/work_order.dart';

List<WorkOrderStatus> allowedTransitions(WorkOrderStatus current) {
  return switch (current) {
    WorkOrderStatus.assigned   => [WorkOrderStatus.inProgress],
    WorkOrderStatus.inProgress => [WorkOrderStatus.done],
    _                          => [],
  };
}

String statusToApiString(WorkOrderStatus s) => switch (s) {
  WorkOrderStatus.inProgress => 'InProgress',
  WorkOrderStatus.done       => 'Done',
  _                          => s.name,
};

class StatusTransitionButton extends StatelessWidget {
  final WorkOrder workOrder;
  final Future<void> Function(WorkOrderStatus) onTransition;

  const StatusTransitionButton({
    super.key,
    required this.workOrder,
    required this.onTransition,
  });

  @override
  Widget build(BuildContext context) {
    final next = allowedTransitions(workOrder.status);
    if (next.isEmpty) return const SizedBox.shrink();

    return Column(
      crossAxisAlignment: CrossAxisAlignment.stretch,
      children: next.map((s) {
        final label = switch (s) {
          WorkOrderStatus.inProgress => 'Starten',
          WorkOrderStatus.done       => 'Abschließen',
          _                          => s.name,
        };
        final color = switch (s) {
          WorkOrderStatus.inProgress => Colors.blue,
          WorkOrderStatus.done       => Colors.green,
          _                          => Colors.grey,
        };
        return ElevatedButton(
          style: ElevatedButton.styleFrom(backgroundColor: color, foregroundColor: Colors.white),
          onPressed: () => onTransition(s),
          child: Text(label),
        );
      }).toList(),
    );
  }
}
