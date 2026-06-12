import 'dart:io';
import 'package:flutter/material.dart';
import 'package:image_picker/image_picker.dart';
import '../l10n/app_localizations.dart';
import '../models/checklist_step.dart';
import '../models/work_order.dart';
import '../services/work_order_service.dart';
import '../widgets/work_order_completion_form.dart';

class ChecklistWizardScreen extends StatefulWidget {
  final WorkOrder workOrder;
  final void Function(WorkOrder updated)? onWorkOrderUpdated;
  final String? token;

  const ChecklistWizardScreen({
    super.key,
    required this.workOrder,
    this.onWorkOrderUpdated,
    this.token,
  });

  @override
  State<ChecklistWizardScreen> createState() => _ChecklistWizardScreenState();
}

class _ChecklistWizardScreenState extends State<ChecklistWizardScreen> {
  List<ChecklistStep> _steps = [];
  int _currentIndex = 0;
  bool _loading = true;
  File? _capturedPhoto;
  late final _service = WorkOrderService(token: widget.token);
  late WorkOrder _workOrder;

  @override
  void initState() {
    super.initState();
    _workOrder = widget.workOrder;
    _load();
  }

  Future<void> _load() async {
    if (_workOrder.status == WorkOrderStatus.assigned) {
      try {
        final updated = await _service.changeStatus(_workOrder.workOrderId, 'InProgress');
        setState(() => _workOrder = updated);
        widget.onWorkOrderUpdated?.call(updated);
      } catch (_) {}
    }
    try {
      final steps = await _service.getChecklist(_workOrder.workOrderId);
      setState(() {
        _steps = steps;
        _loading = false;
      });
    } catch (_) {
      setState(() => _loading = false);
    }
  }

  Future<void> _takePhoto() async {
    final picker = ImagePicker();
    final picked = await picker.pickImage(source: ImageSource.camera, imageQuality: 80);
    if (picked != null) setState(() => _capturedPhoto = File(picked.path));
  }

  Future<void> _pickFromGallery() async {
    final picker = ImagePicker();
    final picked = await picker.pickImage(source: ImageSource.gallery, imageQuality: 80);
    if (picked != null) setState(() => _capturedPhoto = File(picked.path));
  }

  Future<void> _completeStep() async {
    final step = _steps[_currentIndex];

    if (_capturedPhoto != null) {
      try {
        await _service.uploadDocument(
          _workOrder.workOrderId,
          _capturedPhoto!,
          'Foto für: ${step.label}',
        );
      } catch (_) {}
    }

    try {
      final updated = await _service.toggleStep(_workOrder.workOrderId, step.id, true);
      setState(() {
        _steps[_currentIndex] = updated;
        _capturedPhoto = null;
      });
      if (_currentIndex < _steps.length - 1) {
        setState(() => _currentIndex++);
      } else {
        _showCompletionForm();
      }
    } catch (e) {
      if (mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(content: Text(e.toString()), backgroundColor: Colors.red),
        );
      }
    }
  }

  void _showCompletionForm() {
    showModalBottomSheet(
      context: context,
      isScrollControlled: true,
      builder: (_) => Padding(
        padding: EdgeInsets.only(bottom: MediaQuery.of(context).viewInsets.bottom),
        child: WorkOrderCompletionForm(
          workOrder: _workOrder,
          onComplete: (notes) async {
            Navigator.pop(context);
            try {
              final completed = await _service.completeWorkOrder(_workOrder.workOrderId, notes);
              widget.onWorkOrderUpdated?.call(completed);
              if (mounted) Navigator.pop(context, true);
            } catch (e) {
              if (mounted) {
                ScaffoldMessenger.of(context).showSnackBar(
                  SnackBar(content: Text(e.toString()), backgroundColor: Colors.red),
                );
              }
            }
          },
          onCancel: () => Navigator.pop(context),
        ),
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    final l10n = AppLocalizations.of(context)!;

    if (_loading) {
      return Scaffold(
        appBar: AppBar(title: Text(_workOrder.title)),
        body: const Center(child: CircularProgressIndicator()),
      );
    }

    if (_steps.isEmpty) {
      return Scaffold(
        appBar: AppBar(title: Text(_workOrder.title)),
        body: Center(
          child: Column(
            mainAxisSize: MainAxisSize.min,
            children: [
              Text(l10n.checklistEmpty),
              const SizedBox(height: 16),
              ElevatedButton(
                onPressed: _showCompletionForm,
                child: Text(l10n.completionSubmit),
              ),
            ],
          ),
        ),
      );
    }

    final step = _steps[_currentIndex];
    final progress = (_currentIndex + 1) / _steps.length;
    final canProceed = !step.requiresPhoto || _capturedPhoto != null;

    return Scaffold(
      appBar: AppBar(
        title: Text(_workOrder.title),
        bottom: PreferredSize(
          preferredSize: const Size.fromHeight(4),
          child: LinearProgressIndicator(value: progress),
        ),
      ),
      body: SafeArea(
        child: Padding(
          padding: const EdgeInsets.all(24),
          child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Text(
              l10n.checklistStepOf(_currentIndex + 1, _steps.length),
              style: const TextStyle(fontSize: 13, color: Colors.grey),
            ),
            const SizedBox(height: 16),
            Text(
              step.label,
              style: const TextStyle(fontSize: 20, fontWeight: FontWeight.bold),
            ),
            const SizedBox(height: 8),
            Wrap(
              spacing: 8,
              children: [
                if (step.isMandatory)
                  Container(
                    padding: const EdgeInsets.symmetric(horizontal: 8, vertical: 2),
                    color: const Color(0xFFfee2e2),
                    child: Text(
                      l10n.checklistMandatory,
                      style: const TextStyle(
                          fontSize: 11,
                          color: Color(0xFFb91c1c),
                          fontWeight: FontWeight.w600),
                    ),
                  ),
                if (step.requiresPhoto)
                  Container(
                    padding: const EdgeInsets.symmetric(horizontal: 8, vertical: 2),
                    color: const Color(0xFFdbeafe),
                    child: Text(
                      l10n.checklistRequiresPhoto,
                      style: const TextStyle(
                          fontSize: 11,
                          color: Color(0xFF1d4ed8),
                          fontWeight: FontWeight.w600),
                    ),
                  ),
              ],
            ),
            const SizedBox(height: 24),
            if (step.requiresPhoto || _capturedPhoto != null) ...[
              if (_capturedPhoto != null)
                ClipRRect(
                  borderRadius: BorderRadius.circular(8),
                  child: Image.file(_capturedPhoto!,
                      height: 200, width: double.infinity, fit: BoxFit.cover),
                )
              else
                Container(
                  height: 200,
                  width: double.infinity,
                  color: const Color(0xFFf3f4f6),
                  child: const Icon(Icons.camera_alt, size: 48, color: Colors.grey),
                ),
              const SizedBox(height: 12),
              Wrap(
                spacing: 8,
                children: [
                  OutlinedButton.icon(
                    icon: const Icon(Icons.camera_alt),
                    label: Text(l10n.documentsCamera),
                    onPressed: _takePhoto,
                  ),
                  OutlinedButton.icon(
                    icon: const Icon(Icons.photo_library),
                    label: Text(l10n.documentsGallery),
                    onPressed: _pickFromGallery,
                  ),
                ],
              ),
              const SizedBox(height: 24),
            ],
            const Spacer(),
            Row(
              children: [
                if (_currentIndex > 0)
                  OutlinedButton(
                    onPressed: () => setState(() {
                      _currentIndex--;
                      _capturedPhoto = null;
                    }),
                    child: Text(l10n.back),
                  ),
                const Spacer(),
                ElevatedButton(
                  style: ElevatedButton.styleFrom(
                    backgroundColor: _currentIndex < _steps.length - 1
                        ? const Color(0xFF1e3a5f)
                        : const Color(0xFF15803d),
                    foregroundColor: Colors.white,
                    minimumSize: const Size(140, 48),
                  ),
                  onPressed: canProceed ? _completeStep : null,
                  child: Text(
                    _currentIndex < _steps.length - 1
                        ? l10n.checklistNext
                        : l10n.completionSubmit,
                  ),
                ),
              ],
            ),
          ],
        ),
      ),
      ),
    );
  }
}
