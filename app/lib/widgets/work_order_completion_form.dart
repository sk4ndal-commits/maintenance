import 'package:flutter/material.dart';
import '../l10n/app_localizations.dart';
import '../models/work_order.dart';

class WorkOrderCompletionForm extends StatefulWidget {
  final WorkOrder workOrder;
  final Future<void> Function(String? notes) onComplete;
  final VoidCallback onCancel;

  const WorkOrderCompletionForm({
    super.key,
    required this.workOrder,
    required this.onComplete,
    required this.onCancel,
  });

  @override
  State<WorkOrderCompletionForm> createState() => _WorkOrderCompletionFormState();
}

class _WorkOrderCompletionFormState extends State<WorkOrderCompletionForm> {
  final _notesController = TextEditingController();
  bool _loading = false;

  @override
  void dispose() {
    _notesController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    final l10n = AppLocalizations.of(context)!;
    return Padding(
      padding: const EdgeInsets.all(16),
      child: Column(
        mainAxisSize: MainAxisSize.min,
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Text(l10n.completionTitle,
              style: const TextStyle(fontWeight: FontWeight.bold, fontSize: 16)),
          const SizedBox(height: 12),
          TextField(
            controller: _notesController,
            decoration: InputDecoration(
              labelText: l10n.completionNotes,
              border: const OutlineInputBorder(),
            ),
            maxLines: 3,
          ),
          const SizedBox(height: 16),
          Row(children: [
            Expanded(
              child: ElevatedButton(
                style: ElevatedButton.styleFrom(backgroundColor: Colors.green),
                onPressed: _loading
                    ? null
                    : () async {
                        setState(() => _loading = true);
                        await widget.onComplete(
                          _notesController.text.trim().isEmpty
                              ? null
                              : _notesController.text.trim(),
                        );
                        if (mounted) setState(() => _loading = false);
                      },
                child: Text(_loading ? l10n.completionSubmitting : l10n.completionSubmit),
              ),
            ),
            const SizedBox(width: 8),
            TextButton(onPressed: widget.onCancel, child: Text(l10n.cancel)),
          ]),
        ],
      ),
    );
  }
}
