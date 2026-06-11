import 'dart:io';
import 'package:flutter/material.dart';
import 'package:image_picker/image_picker.dart';
import '../l10n/app_localizations.dart';
import '../models/maintenance_document.dart';
import '../services/work_order_service.dart';

class MaintenanceDocumentWidget extends StatefulWidget {
  final String workOrderId;

  const MaintenanceDocumentWidget({super.key, required this.workOrderId});

  @override
  State<MaintenanceDocumentWidget> createState() => _MaintenanceDocumentWidgetState();
}

class _MaintenanceDocumentWidgetState extends State<MaintenanceDocumentWidget> {
  List<MaintenanceDocument> _docs = [];
  bool _loading = true;
  final _notesController = TextEditingController();

  @override
  void initState() {
    super.initState();
    _load();
  }

  @override
  void dispose() {
    _notesController.dispose();
    super.dispose();
  }

  Future<void> _load() async {
    try {
      final docs = await WorkOrderService().getDocuments(widget.workOrderId);
      setState(() {
        _docs = docs;
        _loading = false;
      });
    } catch (_) {
      setState(() => _loading = false);
    }
  }

  Future<void> _pickAndUpload(ImageSource source) async {
    final picker = ImagePicker();
    final picked = await picker.pickImage(source: source, imageQuality: 80);
    if (picked == null) return;

    try {
      final doc = await WorkOrderService().uploadDocument(
        widget.workOrderId,
        File(picked.path),
        _notesController.text.trim().isEmpty ? null : _notesController.text.trim(),
      );
      setState(() {
        _docs.add(doc);
        _notesController.clear();
      });
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

    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Padding(
          padding: const EdgeInsets.symmetric(vertical: 8),
          child: Text(
            l10n.documentsTitle,
            style: const TextStyle(fontWeight: FontWeight.bold, fontSize: 14),
          ),
        ),
        TextField(
          controller: _notesController,
          decoration: InputDecoration(
            labelText: l10n.documentsNotes,
            border: const OutlineInputBorder(),
            isDense: true,
          ),
          maxLines: 2,
        ),
        const SizedBox(height: 8),
        Row(
          children: [
            ElevatedButton.icon(
              icon: const Icon(Icons.camera_alt, size: 18),
              label: Text(l10n.documentsCamera),
              onPressed: () => _pickAndUpload(ImageSource.camera),
            ),
            const SizedBox(width: 8),
            ElevatedButton.icon(
              icon: const Icon(Icons.photo_library, size: 18),
              label: Text(l10n.documentsGallery),
              onPressed: () => _pickAndUpload(ImageSource.gallery),
            ),
          ],
        ),
        const SizedBox(height: 8),
        if (_loading)
          const Center(child: CircularProgressIndicator())
        else if (_docs.isEmpty)
          Text(l10n.documentsEmpty,
              style: const TextStyle(color: Colors.grey, fontSize: 13))
        else
          ..._docs.map((d) => ListTile(
                dense: true,
                leading: const Icon(Icons.attach_file),
                title: Text(d.fileName),
                subtitle: d.notes != null ? Text(d.notes!) : null,
                trailing: Text(
                  d.uploadedAt.length >= 10 ? d.uploadedAt.substring(0, 10) : d.uploadedAt,
                  style: const TextStyle(fontSize: 11, color: Colors.grey),
                ),
              )),
      ],
    );
  }
}
