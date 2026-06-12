import 'package:flutter/material.dart';
import '../l10n/app_localizations.dart';
import '../models/work_order.dart';
import '../services/work_order_service.dart';
import '../widgets/status_transition_button.dart';
import '../widgets/work_order_card.dart';
import 'asset_detail_screen.dart';

const _primaryColor = Color(0xFF1e3a5f);

class MyTasksScreen extends StatefulWidget {
  final String? token;
  const MyTasksScreen({super.key, this.token});

  @override
  State<MyTasksScreen> createState() => _MyTasksScreenState();
}

class _MyTasksScreenState extends State<MyTasksScreen> {
  late final _service = WorkOrderService(token: widget.token);
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
                return WorkOrderCard(
                  workOrder: wo,
                  onAssetTap: () => Navigator.push(
                    ctx,
                    MaterialPageRoute(builder: (_) => AssetDetailScreen(assetId: wo.assetId, token: widget.token)),
                  ),
                  onTransition: (newStatus) async {
                    try {
                      final updated = await _service.changeStatus(
                        wo.workOrderId,
                        statusToApiString(newStatus),
                      );
                      setState(() {
                        final idx = _allWorkOrders.indexWhere((w) => w.workOrderId == wo.workOrderId);
                        if (idx != -1) _allWorkOrders[idx] = updated;
                        _applyFilter();
                      });
                    } catch (e) {
                      if (context.mounted) {
                        ScaffoldMessenger.of(context).showSnackBar(
                          SnackBar(content: Text(e.toString()), backgroundColor: Colors.red),
                        );
                      }
                    }
                  },
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

}
