import 'package:flutter/material.dart';
import '../l10n/app_localizations.dart';
import '../models/asset.dart';
import '../models/work_order.dart';
import '../services/asset_service.dart';
import '../widgets/work_order_card.dart';

const _primaryColor = Color(0xFF1e3a5f);

class AssetDetailScreen extends StatefulWidget {
  final String assetId;
  const AssetDetailScreen({super.key, required this.assetId});

  @override
  State<AssetDetailScreen> createState() => _AssetDetailScreenState();
}

class _AssetDetailScreenState extends State<AssetDetailScreen> {
  final _service = AssetService();
  Asset? _asset;
  List<WorkOrder> _workOrders = [];
  bool _loading = true;
  String? _error;

  @override
  void initState() {
    super.initState();
    _load();
  }

  Future<void> _load() async {
    try {
      final asset = await _service.getById(widget.assetId);
      final orders = await _service.getWorkOrders(widget.assetId);
      setState(() {
        _asset = asset;
        _workOrders = orders;
        _loading = false;
      });
    } catch (e) {
      setState(() {
        _error = e.toString();
        _loading = false;
      });
    }
  }

  @override
  Widget build(BuildContext context) {
    final l10n = AppLocalizations.of(context)!;

    if (_loading) {
      return Scaffold(
        appBar: AppBar(
          backgroundColor: _primaryColor,
          foregroundColor: Colors.white,
          elevation: 0,
        ),
        body: Center(
          child: Column(
            mainAxisSize: MainAxisSize.min,
            children: [
              const CircularProgressIndicator(color: _primaryColor),
              const SizedBox(height: 12),
              Text(l10n.assetsLoading,
                  style: const TextStyle(color: Colors.grey)),
            ],
          ),
        ),
      );
    }

    if (_error != null) {
      return Scaffold(
        appBar: AppBar(
          title: Text(l10n.detailError),
          backgroundColor: _primaryColor,
          foregroundColor: Colors.white,
          elevation: 0,
        ),
        body: Center(
          child: Padding(
            padding: const EdgeInsets.all(24),
            child: Column(
              mainAxisSize: MainAxisSize.min,
              children: [
                const Icon(Icons.error_outline,
                    color: Color(0xFFb91c1c), size: 40),
                const SizedBox(height: 12),
                Text(
                  _error!,
                  style: const TextStyle(color: Color(0xFFb91c1c)),
                  textAlign: TextAlign.center,
                ),
              ],
            ),
          ),
        ),
      );
    }

    final asset = _asset!;
    return Scaffold(
      appBar: AppBar(
        title: Text(asset.name),
        backgroundColor: _primaryColor,
        foregroundColor: Colors.white,
        elevation: 0,
      ),
      body: SafeArea(
        child: ListView(
        padding: const EdgeInsets.all(16),
        children: [
          // Asset info card
          Card(
            shape: const RoundedRectangleBorder(),
            elevation: 1,
            child: Padding(
              padding: const EdgeInsets.all(16),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  _TypeBadge(type: asset.type),
                  const SizedBox(height: 12),
                  _InfoRow(
                      label: l10n.detailLocation,
                      value: '📍 ${asset.location}'),
                  if (asset.description != null) ...[
                    const SizedBox(height: 8),
                    _InfoRow(
                        label: l10n.detailDescription,
                        value: asset.description!),
                  ],
                  const SizedBox(height: 8),
                  _InfoRow(
                    label: l10n.detailCreated,
                    value: asset.createdAt.length >= 10
                        ? asset.createdAt.substring(0, 10)
                        : asset.createdAt,
                  ),
                  const SizedBox(height: 8),
                  _InfoRow(
                      label: l10n.detailQrCode, value: asset.qrCodePayload),
                ],
              ),
            ),
          ),
          const SizedBox(height: 24),
          Text(
            l10n.detailWorkOrders,
            style: const TextStyle(fontSize: 18, fontWeight: FontWeight.bold),
          ),
          const SizedBox(height: 8),
          if (_workOrders.isEmpty)
            Text(
              l10n.detailNoWorkOrders,
              style: const TextStyle(color: Colors.grey),
            ),
          ..._workOrders.map(
            (wo) => WorkOrderCard(workOrder: wo),
          ),
        ],
        ),
      ),
    );
  }
}

class _InfoRow extends StatelessWidget {
  final String label;
  final String value;
  const _InfoRow({required this.label, required this.value});

  @override
  Widget build(BuildContext context) {
    return Row(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        SizedBox(
          width: 110,
          child: Text(
            label,
            style: const TextStyle(
              fontWeight: FontWeight.w600,
              fontSize: 13,
              color: Color(0xFF6b7280),
            ),
          ),
        ),
        Expanded(child: Text(value)),
      ],
    );
  }
}

class _TypeBadge extends StatelessWidget {
  final String type;
  const _TypeBadge({required this.type});

  @override
  Widget build(BuildContext context) {
    return Container(
      padding: const EdgeInsets.symmetric(horizontal: 8, vertical: 2),
      color: const Color(0xFFdbeafe),
      child: Text(
        type,
        style: const TextStyle(
          fontSize: 11,
          fontWeight: FontWeight.w600,
          color: Color(0xFF1d4ed8),
        ),
      ),
    );
  }
}

