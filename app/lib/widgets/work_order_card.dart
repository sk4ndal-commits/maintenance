import 'package:flutter/material.dart';
import '../l10n/app_localizations.dart';
import '../models/work_order.dart';
import '../screens/checklist_wizard_screen.dart';
import 'work_order_priority_badge.dart';

class WorkOrderCard extends StatefulWidget {
  final WorkOrder workOrder;
  final Future<void> Function(WorkOrderStatus)? onTransition;
  final VoidCallback? onAssetTap;

  const WorkOrderCard({
    super.key,
    required this.workOrder,
    this.onTransition,
    this.onAssetTap,
  });

  @override
  State<WorkOrderCard> createState() => _WorkOrderCardState();
}

class _WorkOrderCardState extends State<WorkOrderCard> {
  @override
  Widget build(BuildContext context) {
    final l10n = AppLocalizations.of(context)!;
    final wo = widget.workOrder;

    final (statusBg, statusFg, statusLabel) = switch (wo.status) {
      WorkOrderStatus.done       => (const Color(0xFFdcfce7), const Color(0xFF15803d), l10n.statusDone),
      WorkOrderStatus.inProgress => (const Color(0xFFdbeafe), const Color(0xFF1d4ed8), l10n.statusInProgress),
      WorkOrderStatus.assigned   => (const Color(0xFFfef9c3), const Color(0xFFa16207), l10n.statusAssigned),
      WorkOrderStatus.open       => (const Color(0xFFf3f4f6), const Color(0xFF6b7280), l10n.statusOpen),
    };

    return Card(
      shape: const RoundedRectangleBorder(),
      margin: const EdgeInsets.symmetric(horizontal: 16, vertical: 6),
      child: InkWell(
        onTap: widget.onTransition != null &&
                (wo.status == WorkOrderStatus.assigned ||
                    wo.status == WorkOrderStatus.inProgress)
            ? () async {
                final result = await Navigator.push<bool>(
                  context,
                  MaterialPageRoute(
                    builder: (_) => ChecklistWizardScreen(
                      workOrder: wo,
                      onWorkOrderUpdated: (updated) {
                        widget.onTransition!(updated.status);
                      },
                    ),
                  ),
                );
                if (result == true) setState(() {});
              }
            : null,
        child: Padding(
        padding: const EdgeInsets.all(12),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Row(
              children: [
                Container(
                  padding: const EdgeInsets.symmetric(horizontal: 8, vertical: 4),
                  color: statusBg,
                  child: Text(
                    statusLabel,
                    style: TextStyle(color: statusFg, fontSize: 11, fontWeight: FontWeight.bold),
                  ),
                ),
                const SizedBox(width: 8),
                Expanded(
                  child: Text(wo.title, style: const TextStyle(fontWeight: FontWeight.bold)),
                ),
                if (widget.onAssetTap != null)
                  IconButton(
                    icon: const Icon(Icons.chevron_right),
                    onPressed: widget.onAssetTap,
                  ),
              ],
            ),
            const SizedBox(height: 4),
            Row(
              children: [
                WorkOrderPriorityBadge(priority: wo.priority),
                if (wo.dueDate != null) ...[
                  const SizedBox(width: 8),
                  Text(wo.dueDate!, style: const TextStyle(fontSize: 12, color: Colors.grey)),
                ],
              ],
            ),
            if (wo.assignedTechnicianName != null)
              Padding(
                padding: const EdgeInsets.only(top: 4),
                child: Text(wo.assignedTechnicianName!,
                    style: const TextStyle(fontSize: 12, color: Colors.grey)),
              ),
            if (widget.onTransition != null) ...[
              const SizedBox(height: 8),
              if (wo.status == WorkOrderStatus.assigned || wo.status == WorkOrderStatus.inProgress)
                SizedBox(
                  width: double.infinity,
                  child: ElevatedButton.icon(
                    icon: const Icon(Icons.play_arrow),
                    label: Text(wo.status == WorkOrderStatus.assigned
                        ? l10n.startWork
                        : l10n.continueWork),
                    style: ElevatedButton.styleFrom(
                      backgroundColor: const Color(0xFF1e3a5f),
                      foregroundColor: Colors.white,
                    ),
                    onPressed: () async {
                      final result = await Navigator.push<bool>(
                        context,
                        MaterialPageRoute(
                          builder: (_) => ChecklistWizardScreen(
                            workOrder: wo,
                            onWorkOrderUpdated: (updated) {
                              widget.onTransition!(updated.status);
                            },
                          ),
                        ),
                      );
                      if (result == true) {
                        setState(() {});
                      }
                    },
                  ),
                ),
              if (wo.status == WorkOrderStatus.done)
                Padding(
                  padding: const EdgeInsets.only(top: 4),
                  child: Text(
                    wo.completedAt != null
                        ? '${l10n.completedAt}: ${wo.completedAt!.substring(0, 10)}'
                        : l10n.statusDone,
                    style: const TextStyle(fontSize: 12, color: Colors.green),
                  ),
                ),
            ],
          ],
        ),
        ),
      ),
    );
  }
}
