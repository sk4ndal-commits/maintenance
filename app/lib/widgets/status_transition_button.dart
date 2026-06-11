import 'package:flutter/material.dart';
import '../l10n/app_localizations.dart';
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
  final bool canComplete;

  const StatusTransitionButton({
    super.key,
    required this.workOrder,
    required this.onTransition,
    this.canComplete = true,
  });

  @override
  Widget build(BuildContext context) {
    final l10n = AppLocalizations.of(context)!;
    final next = allowedTransitions(workOrder.status);
    if (next.isEmpty) return const SizedBox.shrink();

    return Column(
      crossAxisAlignment: CrossAxisAlignment.stretch,
      children: next.map((s) {
        final label = switch (s) {
          WorkOrderStatus.inProgress => l10n.statusChangeToInProgress,
          WorkOrderStatus.done       => l10n.statusChangeToDone,
          _                          => s.name,
        };
        final color = switch (s) {
          WorkOrderStatus.inProgress => Colors.blue,
          WorkOrderStatus.done       => Colors.green,
          _                          => Colors.grey,
        };
        final disabled = s == WorkOrderStatus.done && !canComplete;
        return ElevatedButton(
          style: ElevatedButton.styleFrom(backgroundColor: color, foregroundColor: Colors.white),
          onPressed: disabled ? null : () => onTransition(s),
          child: Text(label),
        );
      }).toList(),
    );
  }
}
