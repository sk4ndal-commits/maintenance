import 'package:flutter/material.dart';
import 'package:flutter_localizations/flutter_localizations.dart';
import 'l10n/app_localizations.dart';
import 'screens/main_shell.dart';
import 'screens/asset_detail_screen.dart';

void main() {
  runApp(const MaintenanceApp());
}

class MaintenanceApp extends StatelessWidget {
  const MaintenanceApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Maintenance System',
      localizationsDelegates: const [
        AppLocalizations.delegate,
        GlobalMaterialLocalizations.delegate,
        GlobalWidgetsLocalizations.delegate,
        GlobalCupertinoLocalizations.delegate,
      ],
      supportedLocales: const [
        Locale('de'),
        Locale('en'),
      ],
      locale: const Locale('de'),
      theme: ThemeData(
        colorScheme: ColorScheme.fromSeed(seedColor: const Color(0xFF1e3a5f)),
        useMaterial3: true,
        cardTheme: const CardThemeData(
          shape: RoundedRectangleBorder(),
        ),
      ),
      onGenerateRoute: (settings) {
        if (settings.name != null && settings.name!.startsWith('/assets/')) {
          final assetId = settings.name!.replaceFirst('/assets/', '');
          return MaterialPageRoute(
            builder: (_) => AssetDetailScreen(assetId: assetId),
          );
        }
        return MaterialPageRoute(
          builder: (_) => const MainShell(),
        );
      },
      initialRoute: '/',
    );
  }
}
