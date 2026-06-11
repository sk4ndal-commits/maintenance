import 'dart:convert';
import 'package:http/http.dart' as http;
import '../models/asset.dart';
import '../models/work_order.dart';

class AssetService {
  static const String _base = 'http://localhost:5000';

  Future<List<Asset>> getAll({int page = 1, int pageSize = 20}) async {
    final res = await http.get(
      Uri.parse('$_base/api/assets?page=$page&pageSize=$pageSize'),
    );
    if (res.statusCode != 200) {
      throw Exception('Failed to load assets: ${res.statusCode}');
    }
    final json = jsonDecode(res.body) as Map<String, dynamic>;
    return (json['data'] as List<dynamic>)
        .map((e) => Asset.fromJson(e as Map<String, dynamic>))
        .toList();
  }

  Future<Asset> getById(String id) async {
    final res = await http.get(Uri.parse('$_base/api/assets/$id'));
    if (res.statusCode != 200) {
      throw Exception('Failed to load asset: ${res.statusCode}');
    }
    return Asset.fromJson(jsonDecode(res.body) as Map<String, dynamic>);
  }

  Future<List<WorkOrder>> getWorkOrders(String id, {int limit = 10}) async {
    final res = await http.get(
      Uri.parse('$_base/api/assets/$id/work-orders?limit=$limit'),
    );
    if (res.statusCode != 200) {
      throw Exception('Failed to load work orders: ${res.statusCode}');
    }
    final list = jsonDecode(res.body) as List<dynamic>;
    return list
        .map((e) => WorkOrder.fromJson(e as Map<String, dynamic>))
        .toList();
  }
}
