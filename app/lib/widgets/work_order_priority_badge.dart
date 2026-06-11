import 'package:flutter/material.dart';
import '../l10n/app_localizations.dart';
import '../models/work_order.dart';

class WorkOrderPriorityBadge extends StatelessWidget {
  final WorkOrderPriority priority;
  const WorkOrderPriorityBadge({super.key, required this.priority});

  @override
  Widget build(BuildContext context) {
    final l10n = AppLocalizations.of(context)!;
    final (bg, fg, label) = switch (priority) {
      WorkOrderPriority.high   => (const Color(0xFFfee2e2), const Color(0xFFb91c1c), l10n.priorityHigh),
      WorkOrderPriority.medium => (const Color(0xFFfef9c3), const Color(0xFFa16207), l10n.priorityMedium),
      WorkOrderPriority.low    => (const Color(0xFFf3f4f6), const Color(0xFF6b7280), l10n.priorityLow),
    };
    return Container(
      padding: const EdgeInsets.symmetric(horizontal: 8, vertical: 2),
      color: bg,
      child: Text(label, style: TextStyle(color: fg, fontSize: 11, fontWeight: FontWeight.w600)),
    );
  }
}
