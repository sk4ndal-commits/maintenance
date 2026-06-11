import 'package:flutter/material.dart';
import '../l10n/app_localizations.dart';
import '../models/checklist_step.dart';
import '../services/work_order_service.dart';

class ChecklistWidget extends StatefulWidget {
  final String workOrderId;
  final ValueChanged<bool> onMandatoryComplete;

  const ChecklistWidget({
    super.key,
    required this.workOrderId,
    required this.onMandatoryComplete,
  });

  @override
  State<ChecklistWidget> createState() => _ChecklistWidgetState();
}

class _ChecklistWidgetState extends State<ChecklistWidget> {
  List<ChecklistStep> _steps = [];
  bool _loading = true;

  @override
  void initState() {
    super.initState();
    _load();
  }

  Future<void> _load() async {
    final steps = await WorkOrderService().getChecklist(widget.workOrderId);
    setState(() {
      _steps = steps;
      _loading = false;
    });
    _notifyMandatoryStatus(steps);
  }

  void _notifyMandatoryStatus(List<ChecklistStep> steps) {
    final allMandatoryDone =
        steps.where((s) => s.isMandatory).every((s) => s.isCompleted);
    widget.onMandatoryComplete(allMandatoryDone);
  }

  Future<void> _toggle(ChecklistStep step, bool value) async {
    try {
      final updated =
          await WorkOrderService().toggleStep(step.workOrderId, step.id, value);
      setState(() {
        final idx = _steps.indexWhere((s) => s.id == updated.id);
        if (idx != -1) _steps[idx] = updated;
      });
      _notifyMandatoryStatus(_steps);
    } catch (e) {
      if (mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(content: Text(e.toString()), backgroundColor: Colors.red),
        );
      }
    }
  }

  @override
  Widget build(BuildContext context) {
    final l10n = AppLocalizations.of(context)!;

    if (_loading) return const Center(child: CircularProgressIndicator());
    if (_steps.isEmpty) return const SizedBox.shrink();

    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Padding(
          padding: const EdgeInsets.symmetric(vertical: 8),
          child: Text(
            l10n.checklistTitle,
            style: const TextStyle(fontWeight: FontWeight.bold, fontSize: 14),
          ),
        ),
        ..._steps.map((step) => CheckboxListTile(
              dense: true,
              value: step.isCompleted,
              onChanged: (val) => _toggle(step, val ?? false),
              title: Row(
                children: [
                  Expanded(child: Text(step.label)),
                  if (step.isMandatory)
                    Container(
                      margin: const EdgeInsets.only(left: 4),
                      padding: const EdgeInsets.symmetric(
                          horizontal: 6, vertical: 1),
                      color: const Color(0xFFfee2e2),
                      child: Text(
                        l10n.checklistMandatory,
                        style: const TextStyle(
                          fontSize: 10,
                          color: Color(0xFFb91c1c),
                          fontWeight: FontWeight.w600,
                        ),
                      ),
                    ),
                ],
              ),
              secondary: step.isCompleted
                  ? const Icon(Icons.check_circle,
                      color: Colors.green, size: 20)
                  : const Icon(Icons.radio_button_unchecked,
                      color: Colors.grey, size: 20),
            )),
      ],
    );
  }
}
