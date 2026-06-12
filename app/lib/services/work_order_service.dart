import 'dart:convert';
import 'dart:io';
import 'package:http/http.dart' as http;
import '../models/checklist_step.dart';
import '../models/maintenance_document.dart';
import '../models/work_order.dart';

class WorkOrderService {
  static const _base = 'http://localhost:5000';

  final String? token;
  WorkOrderService({this.token});

  Map<String, String> get _headers => {
    'Content-Type': 'application/json',
    if (token != null) 'Authorization': 'Bearer $token',
  };

  Future<List<WorkOrder>> getByTechnician(String technicianId, {int page = 1, int pageSize = 50}) async {
    final res = await http.get(Uri.parse(
        '$_base/api/work-orders?technicianId=$technicianId&page=$page&pageSize=$pageSize'), headers: _headers);
    if (res.statusCode != 200) throw Exception('API error ${res.statusCode}');
    final json = jsonDecode(res.body) as Map<String, dynamic>;
    return (json['data'] as List).map((e) => WorkOrder.fromJson(e as Map<String, dynamic>)).toList();
  }

  Future<List<WorkOrder>> getAll({int page = 1, int pageSize = 50}) async {
    final res = await http.get(Uri.parse('$_base/api/work-orders?page=$page&pageSize=$pageSize'), headers: _headers);
    if (res.statusCode != 200) throw Exception('API error ${res.statusCode}');
    final json = jsonDecode(res.body) as Map<String, dynamic>;
    return (json['data'] as List).map((e) => WorkOrder.fromJson(e as Map<String, dynamic>)).toList();
  }

  Future<WorkOrder> changeStatus(String workOrderId, String newStatus) async {
    final res = await http.put(
      Uri.parse('$_base/api/work-orders/$workOrderId/status'),
      headers: _headers,
      body: jsonEncode({'workOrderId': workOrderId, 'newStatus': newStatus}),
    );
    if (res.statusCode == 422) {
      final body = jsonDecode(res.body) as Map<String, dynamic>;
      throw Exception(body['message'] ?? 'Invalid transition');
    }
    if (res.statusCode != 200) throw Exception('API error ${res.statusCode}');
    return WorkOrder.fromJson(jsonDecode(res.body) as Map<String, dynamic>);
  }

  Future<List<ChecklistStep>> getChecklist(String workOrderId) async {
    final res = await http.get(Uri.parse('$_base/api/work-orders/$workOrderId/checklist'), headers: _headers);
    if (res.statusCode != 200) throw Exception('API error ${res.statusCode}');
    final list = jsonDecode(res.body) as List;
    return list.map((e) => ChecklistStep.fromJson(e as Map<String, dynamic>)).toList();
  }

  Future<ChecklistStep> toggleStep(String workOrderId, String stepId, bool isCompleted) async {
    final res = await http.put(
      Uri.parse('$_base/api/work-orders/$workOrderId/checklist/$stepId'),
      headers: _headers,
      body: jsonEncode({'workOrderId': workOrderId, 'stepId': stepId, 'isCompleted': isCompleted}),
    );
    if (res.statusCode != 200) throw Exception('API error ${res.statusCode}');
    return ChecklistStep.fromJson(jsonDecode(res.body) as Map<String, dynamic>);
  }

  Future<List<MaintenanceDocument>> getDocuments(String workOrderId) async {
    final res = await http.get(Uri.parse('$_base/api/work-orders/$workOrderId/documents'), headers: _headers);
    if (res.statusCode != 200) throw Exception('API error ${res.statusCode}');
    final list = jsonDecode(res.body) as List;
    return list.map((e) => MaintenanceDocument.fromJson(e as Map<String, dynamic>)).toList();
  }

  Future<MaintenanceDocument> uploadDocument(
      String workOrderId, File file, String? notes) async {
    final request = http.MultipartRequest(
      'POST',
      Uri.parse('$_base/api/work-orders/$workOrderId/documents'),
    );
    if (token != null) request.headers['Authorization'] = 'Bearer $token';
    request.files.add(await http.MultipartFile.fromPath('file', file.path));
    if (notes != null && notes.isNotEmpty) request.fields['notes'] = notes;
    request.fields['uploadedBy'] = 'app-technician';

    final streamed = await request.send();
    final res = await http.Response.fromStream(streamed);
    if (res.statusCode != 201) throw Exception('Upload failed: ${res.statusCode}');
    return MaintenanceDocument.fromJson(jsonDecode(res.body) as Map<String, dynamic>);
  }

  Future<WorkOrder> completeWorkOrder(String workOrderId, String? completionNotes) async {
    final res = await http.post(
      Uri.parse('$_base/api/work-orders/$workOrderId/complete'),
      headers: _headers,
      body: jsonEncode({'workOrderId': workOrderId, 'completionNotes': completionNotes}),
    );
    if (res.statusCode == 422) {
      final body = jsonDecode(res.body) as Map<String, dynamic>;
      throw Exception(body['message'] ?? 'Cannot complete work order');
    }
    if (res.statusCode != 200) throw Exception('API error ${res.statusCode}');
    return WorkOrder.fromJson(jsonDecode(res.body) as Map<String, dynamic>);
  }
}
