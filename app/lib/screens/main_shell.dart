import 'package:flutter/material.dart';
import '../l10n/app_localizations.dart';
import '../services/auth_provider.dart';
import 'asset_list_screen.dart';
import 'my_tasks_screen.dart';
import 'qr_scan_screen.dart';

const _primaryColor = Color(0xFF1e3a5f);

class MainShell extends StatefulWidget {
  final AuthProvider auth;
  const MainShell({super.key, required this.auth});

  @override
  State<MainShell> createState() => _MainShellState();
}

class _MainShellState extends State<MainShell> {
  int _selectedIndex = 0;

  List<Widget> _screens() {
    final token = widget.auth.token;
    final screens = <Widget>[
      AssetListScreen(token: token),
      QrScanScreen(token: token),
    ];
    if (widget.auth.isPlanner || widget.auth.isTechnician) {
      screens.add(MyTasksScreen(token: token, userId: widget.auth.userId));
    }
    return screens;
  }

  List<BottomNavigationBarItem> _navItems(AppLocalizations l10n) {
    final items = <BottomNavigationBarItem>[
      BottomNavigationBarItem(
        icon: const Icon(Icons.inventory_2_outlined),
        activeIcon: const Icon(Icons.inventory_2),
        label: l10n.navAssets,
      ),
      BottomNavigationBarItem(
        icon: const Icon(Icons.qr_code_scanner_outlined),
        activeIcon: const Icon(Icons.qr_code_scanner),
        label: l10n.navScan,
      ),
    ];
    if (widget.auth.isPlanner || widget.auth.isTechnician) {
      items.add(BottomNavigationBarItem(
        icon: const Icon(Icons.assignment_outlined),
        activeIcon: const Icon(Icons.assignment),
        label: l10n.navMyTasks,
      ));
    }
    return items;
  }

  @override
  Widget build(BuildContext context) {
    final l10n = AppLocalizations.of(context)!;
    final screens = _screens();
    final safeIndex = _selectedIndex < screens.length ? _selectedIndex : 0;

    return Scaffold(
      appBar: AppBar(
        title: Text(l10n.appTitle),
        backgroundColor: _primaryColor,
        foregroundColor: Colors.white,
        elevation: 0,
        actions: [
          IconButton(
            icon: const Icon(Icons.logout),
            tooltip: 'Abmelden',
            onPressed: () => widget.auth.logout(),
          ),
        ],
      ),
      body: SafeArea(child: screens[safeIndex]),
      bottomNavigationBar: BottomNavigationBar(
        currentIndex: safeIndex,
        onTap: (i) => setState(() => _selectedIndex = i),
        selectedItemColor: _primaryColor,
        items: _navItems(l10n),
      ),
    );
  }
}
