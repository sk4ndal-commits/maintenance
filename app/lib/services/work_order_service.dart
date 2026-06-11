import 'dart:convert';
import 'package:http/http.dart' as http;
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
}
