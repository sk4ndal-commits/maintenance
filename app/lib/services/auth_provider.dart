import 'dart:convert';
import 'package:flutter/foundation.dart';

class AuthProvider extends ChangeNotifier {
  String? _token;
  Map<String, dynamic>? _claims;

  String? get token => _token;

  String get role =>
      _claims?['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] as String? ?? '';

  String get userId =>
      _claims?['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'] as String? ?? '';

  String get userName =>
      _claims?['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'] as String? ?? '';

  bool get isAuthenticated => _token != null;
  bool get isAdmin => role == 'Admin';
  bool get isPlanner => role == 'Admin' || role == 'Planner';
  bool get isTechnician => role == 'Technician';

  void setToken(String token) {
    _token = token;
    _claims = _decodeJwt(token);
    notifyListeners();
  }

  void logout() {
    _token = null;
    _claims = null;
    notifyListeners();
  }

  static Map<String, dynamic>? _decodeJwt(String token) {
    try {
      final parts = token.split('.');
      if (parts.length != 3) return null;
      var payload = parts[1];
      // Pad base64 string
      while (payload.length % 4 != 0) {
        payload += '=';
      }
      final decoded = utf8.decode(base64Url.decode(payload));
      return jsonDecode(decoded) as Map<String, dynamic>;
    } catch (_) {
      return null;
    }
  }
}
