import 'dart:convert';
import 'package:http/http.dart' as http;
import '../models/asset.dart';
import '../models/history_event.dart';
import '../models/work_order.dart';

class AssetService {
  static const String _base = 'http://localhost:5000';

  final String? token;
  AssetService({this.token});

  Map<String, String> get _headers => {
    'Content-Type': 'application/json',
    if (token != null) 'Authorization': 'Bearer $token',
  };

  Future<List<Asset>> getAll({int page = 1, int pageSize = 20}) async {
    final res = await http.get(
      Uri.parse('$_base/api/assets?page=$page&pageSize=$pageSize'),
      headers: _headers,
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
    final res = await http.get(Uri.parse('$_base/api/assets/$id'), headers: _headers);
    if (res.statusCode != 200) {
      throw Exception('Failed to load asset: ${res.statusCode}');
    }
    return Asset.fromJson(jsonDecode(res.body) as Map<String, dynamic>);
  }

  Future<List<HistoryEvent>> getHistory(String assetId,
      {String? search, String? eventType}) async {
    final params = <String, String>{'page': '1', 'pageSize': '100'};
    if (search != null && search.isNotEmpty) params['search'] = search;
    if (eventType != null && eventType.isNotEmpty) params['eventType'] = eventType;
    final uri = Uri.parse('$_base/api/assets/$assetId/history')
        .replace(queryParameters: params);
    final res = await http.get(uri, headers: _headers);
    if (res.statusCode != 200) throw Exception('API error ${res.statusCode}');
    final body = jsonDecode(res.body) as Map<String, dynamic>;
    return (body['data'] as List)
        .map((e) => HistoryEvent.fromJson(e as Map<String, dynamic>))
        .toList();
  }

  Future<List<WorkOrder>> getWorkOrders(String id, {int limit = 10}) async {
    final res = await http.get(
      Uri.parse('$_base/api/assets/$id/work-orders?limit=$limit'),
      headers: _headers,
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
