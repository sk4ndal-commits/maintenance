import 'package:flutter/material.dart';
import 'package:flutter_localizations/flutter_localizations.dart';
import 'l10n/app_localizations.dart';
import 'screens/login_screen.dart';
import 'screens/main_shell.dart';
import 'screens/asset_detail_screen.dart';
import 'services/auth_provider.dart';

void main() {
  runApp(MaintenanceApp());
}

class MaintenanceApp extends StatefulWidget {
  const MaintenanceApp({super.key});

  @override
  State<MaintenanceApp> createState() => _MaintenanceAppState();
}

class _MaintenanceAppState extends State<MaintenanceApp> {
  final AuthProvider _auth = AuthProvider();

  @override
  void initState() {
    super.initState();
    _auth.addListener(() => setState(() {}));
  }

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
      home: _auth.isAuthenticated
          ? MainShell(auth: _auth)
          : LoginScreen(auth: _auth),
      onGenerateRoute: (settings) {
        if (!_auth.isAuthenticated) {
          return MaterialPageRoute(builder: (_) => LoginScreen(auth: _auth));
        }
        if (settings.name != null && settings.name!.startsWith('/assets/')) {
          final assetId = settings.name!.replaceFirst('/assets/', '');
          return MaterialPageRoute(
            builder: (_) => AssetDetailScreen(assetId: assetId),
          );
        }
        return MaterialPageRoute(builder: (_) => MainShell(auth: _auth));
      },
    );
  }
}
