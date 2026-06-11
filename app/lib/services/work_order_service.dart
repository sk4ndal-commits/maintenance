import 'dart:convert';
import 'package:http/http.dart' as http;
import '../models/checklist_step.dart';
import '../models/work_order.dart';

class WorkOrderService {
  static const _base = 'http://localhost:5000';

  Future<List<WorkOrder>> getByTechnician(String technicianId, {int page = 1, int pageSize = 50}) async {
    final res = await http.get(Uri.parse(
        '$_base/api/work-orders?technicianId=$technicianId&page=$page&pageSize=$pageSize'));
    if (res.statusCode != 200) throw Exception('API error ${res.statusCode}');
    final json = jsonDecode(res.body) as Map<String, dynamic>;
    return (json['data'] as List).map((e) => WorkOrder.fromJson(e as Map<String, dynamic>)).toList();
  }

  Future<List<WorkOrder>> getAll({int page = 1, int pageSize = 50}) async {
    final res = await http.get(Uri.parse('$_base/api/work-orders?page=$page&pageSize=$pageSize'));
    if (res.statusCode != 200) throw Exception('API error ${res.statusCode}');
    final json = jsonDecode(res.body) as Map<String, dynamic>;
    return (json['data'] as List).map((e) => WorkOrder.fromJson(e as Map<String, dynamic>)).toList();
  }

  Future<WorkOrder> changeStatus(String workOrderId, String newStatus) async {
    final res = await http.put(
      Uri.parse('$_base/api/work-orders/$workOrderId/status'),
      headers: {'Content-Type': 'application/json'},
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
    final res = await http.get(Uri.parse('$_base/api/work-orders/$workOrderId/checklist'));
    if (res.statusCode != 200) throw Exception('API error ${res.statusCode}');
    final list = jsonDecode(res.body) as List;
    return list.map((e) => ChecklistStep.fromJson(e as Map<String, dynamic>)).toList();
  }

  Future<ChecklistStep> toggleStep(String workOrderId, String stepId, bool isCompleted) async {
    final res = await http.put(
      Uri.parse('$_base/api/work-orders/$workOrderId/checklist/$stepId'),
      headers: {'Content-Type': 'application/json'},
      body: jsonEncode({'workOrderId': workOrderId, 'stepId': stepId, 'isCompleted': isCompleted}),
    );
    if (res.statusCode != 200) throw Exception('API error ${res.statusCode}');
    return ChecklistStep.fromJson(jsonDecode(res.body) as Map<String, dynamic>);
  }
}
