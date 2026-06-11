import 'package:flutter/material.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:app/main.dart';

void main() {
  testWidgets('MaintenanceApp smoke test', (WidgetTester tester) async {
    await tester.pumpWidget(const MaintenanceApp());
    expect(find.byType(MaterialApp), findsOneWidget);
  });
}
