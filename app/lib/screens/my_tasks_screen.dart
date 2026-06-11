import 'package:flutter/material.dart';
import '../l10n/app_localizations.dart';
import '../models/work_order.dart';
import '../services/work_order_service.dart';
import 'asset_detail_screen.dart';

const _primaryColor = Color(0xFF1e3a5f);

class MyTasksScreen extends StatefulWidget {
  const MyTasksScreen({super.key});

  @override
  State<MyTasksScreen> createState() => _MyTasksScreenState();
}

class _MyTasksScreenState extends State<MyTasksScreen> {
  final _service = WorkOrderService();
  List<WorkOrder> _allWorkOrders = [];
  List<WorkOrder> _workOrders = [];
  WorkOrderStatus? _filterStatus;
  bool _loading = true;
  String? _error;

  @override
  void initState() {
    super.initState();
    _load();
  }

  Future<void> _load() async {
    setState(() { _loading = true; _error = null; });
    try {
      final result = await _service.getAll();
      setState(() { _allWorkOrders = result; _loading = false; });
      _applyFilter();
    } catch (e) {
      setState(() { _error = e.toString(); _loading = false; });
    }
  }

  void _applyFilter() {
    setState(() {
      _workOrders = _filterStatus == null
          ? List.of(_allWorkOrders)
          : _allWorkOrders.where((w) => w.status == _filterStatus).toList();
    });
  }

  void _setFilter(WorkOrderStatus? status) {
    _filterStatus = _filterStatus == status ? null : status;
    _applyFilter();
  }

  @override
  Widget build(BuildContext context) {
    final l10n = AppLocalizations.of(context)!;
    if (_loading) return const Center(child: CircularProgressIndicator());
    if (_error != null) {
      return Center(
        child: Column(
          mainAxisSize: MainAxisSize.min,
          children: [
            Text(_error!, style: const TextStyle(color: Color(0xFFb91c1c))),
            const SizedBox(height: 12),
            ElevatedButton(
              onPressed: _load,
              style: ElevatedButton.styleFrom(backgroundColor: _primaryColor, foregroundColor: Colors.white, shape: const RoundedRectangleBorder()),
              child: Text(l10n.retry),
            ),
          ],
        ),
      );
    }
    final filterBar = SingleChildScrollView(
      scrollDirection: Axis.horizontal,
      padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 8),
      child: Row(
        children: WorkOrderStatus.values.map((s) {
          final selected = _filterStatus == s;
          return Padding(
            padding: const EdgeInsets.only(right: 8),
            child: FilterChip(
              label: Text(_statusLabel(s, l10n)),
              selected: selected,
              onSelected: (_) => _setFilter(s),
              selectedColor: _primaryColor.withAlpha(30),
              checkmarkColor: _primaryColor,
              shape: const RoundedRectangleBorder(),
            ),
          );
        }).toList(),
      ),
    );

    if (_workOrders.isEmpty) {
      return Column(
        children: [
          filterBar,
          Expanded(child: Center(child: Text(l10n.myTasksEmpty, style: const TextStyle(color: Colors.grey)))),
        ],
      );
    }
    return RefreshIndicator(
      onRefresh: _load,
      child: Column(
        children: [
          filterBar,
          Expanded(
            child: ListView.builder(
              itemCount: _workOrders.length,
              itemBuilder: (ctx, i) {
                final wo = _workOrders[i];
                return Card(
                  shape: const RoundedRectangleBorder(),
                  margin: const EdgeInsets.symmetric(horizontal: 16, vertical: 6),
                  child: ListTile(
                    leading: _statusBadge(wo.status),
                    title: Text(wo.title, style: const TextStyle(fontWeight: FontWeight.bold)),
                    subtitle: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        Text(wo.priority),
                        if (wo.assignedTechnicianName != null)
                          Text(wo.assignedTechnicianName!, style: const TextStyle(fontSize: 12, color: Colors.grey)),
                      ],
                    ),
                    trailing: const Icon(Icons.chevron_right),
                    onTap: () => Navigator.push(
                      ctx,
                      MaterialPageRoute(builder: (_) => AssetDetailScreen(assetId: wo.assetId)),
                    ),
                  ),
                );
              },
            ),
          ),
        ],
      ),
    );
  }

  String _statusLabel(WorkOrderStatus status, AppLocalizations l10n) {
    return switch (status) {
      WorkOrderStatus.open => l10n.statusOpen,
      WorkOrderStatus.assigned => l10n.statusAssigned,
      WorkOrderStatus.inProgress => l10n.statusInProgress,
      WorkOrderStatus.done => l10n.statusDone,
    };
  }

  Widget _statusBadge(WorkOrderStatus status) {
    final (bg, fg) = switch (status) {
      WorkOrderStatus.done => (const Color(0xFFdcfce7), const Color(0xFF15803d)),
      WorkOrderStatus.inProgress => (const Color(0xFFdbeafe), const Color(0xFF1d4ed8)),
      WorkOrderStatus.assigned => (const Color(0xFFfef9c3), const Color(0xFFa16207)),
      WorkOrderStatus.open => (const Color(0xFFf3f4f6), const Color(0xFF6b7280)),
    };
    return Container(
      padding: const EdgeInsets.symmetric(horizontal: 8, vertical: 4),
      color: bg,
      child: Text(
        status.name,
        style: TextStyle(color: fg, fontSize: 11, fontWeight: FontWeight.bold),
      ),
    );
  }
}
