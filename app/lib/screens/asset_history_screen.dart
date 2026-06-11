import 'package:flutter/material.dart';
import '../l10n/app_localizations.dart';
import '../models/history_event.dart';
import '../services/asset_service.dart';

class AssetHistoryScreen extends StatefulWidget {
  final String assetId;
  final String assetName;

  const AssetHistoryScreen({
    super.key,
    required this.assetId,
    required this.assetName,
  });

  @override
  State<AssetHistoryScreen> createState() => _AssetHistoryScreenState();
}

class _AssetHistoryScreenState extends State<AssetHistoryScreen> {
  List<HistoryEvent> _events = [];
  bool _loading = true;
  final _searchController = TextEditingController();
  String _filterType = '';

  final _eventTypes = [
    '',
    'WorkOrderCreated',
    'WorkOrderAssigned',
    'WorkOrderCompleted',
    'ChecklistStepCompleted',
    'DocumentUploaded',
  ];

  @override
  void initState() {
    super.initState();
    _load();
  }

  @override
  void dispose() {
    _searchController.dispose();
    super.dispose();
  }

  Future<void> _load() async {
    setState(() => _loading = true);
    try {
      final events = await AssetService().getHistory(
        widget.assetId,
        search: _searchController.text.trim().isEmpty
            ? null
            : _searchController.text.trim(),
        eventType: _filterType.isEmpty ? null : _filterType,
      );
      setState(() {
        _events = events;
        _loading = false;
      });
    } catch (e) {
      setState(() => _loading = false);
      if (mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(content: Text(e.toString()), backgroundColor: Colors.red),
        );
      }
    }
  }

  Color _colorFor(String type) => switch (type) {
        'WorkOrderCreated' => const Color(0xFF1e3a5f),
        'WorkOrderCompleted' => Colors.green,
        'WorkOrderAssigned' => Colors.orange,
        'ChecklistStepCompleted' => Colors.blue,
        'DocumentUploaded' => Colors.grey,
        _ => Colors.grey,
      };

  IconData _iconFor(String type) => switch (type) {
        'WorkOrderCreated' => Icons.add_circle_outline,
        'WorkOrderCompleted' => Icons.check_circle_outline,
        'WorkOrderAssigned' => Icons.person_outline,
        'ChecklistStepCompleted' => Icons.check_box_outlined,
        'DocumentUploaded' => Icons.attach_file,
        _ => Icons.circle_outlined,
      };

  String _labelFor(String type, AppLocalizations l10n) => switch (type) {
        'WorkOrderCreated' => l10n.historyTypeCreated,
        'WorkOrderCompleted' => l10n.historyTypeCompleted,
        'WorkOrderAssigned' => l10n.historyTypeAssigned,
        'ChecklistStepCompleted' => l10n.historyTypeChecklist,
        'DocumentUploaded' => l10n.historyTypeDocument,
        _ => type,
      };

  @override
  Widget build(BuildContext context) {
    final l10n = AppLocalizations.of(context)!;

    return Scaffold(
      appBar: AppBar(title: Text(l10n.historyTitle)),
      body: Column(
        children: [
          Padding(
            padding: const EdgeInsets.fromLTRB(16, 12, 16, 0),
            child: TextField(
              controller: _searchController,
              decoration: InputDecoration(
                hintText: l10n.historySearch,
                prefixIcon: const Icon(Icons.search),
                border: const OutlineInputBorder(),
                contentPadding:
                    const EdgeInsets.symmetric(vertical: 8, horizontal: 12),
                suffixIcon: _searchController.text.isNotEmpty
                    ? IconButton(
                        icon: const Icon(Icons.clear),
                        onPressed: () {
                          _searchController.clear();
                          _load();
                        },
                      )
                    : null,
              ),
              onSubmitted: (_) => _load(),
            ),
          ),
          SizedBox(
            height: 48,
            child: ListView(
              scrollDirection: Axis.horizontal,
              padding:
                  const EdgeInsets.symmetric(horizontal: 16, vertical: 8),
              children: _eventTypes.map((type) {
                final label = type.isEmpty
                    ? l10n.historyAllTypes
                    : _labelFor(type, l10n);
                return Padding(
                  padding: const EdgeInsets.only(right: 8),
                  child: FilterChip(
                    label: Text(label,
                        style: const TextStyle(fontSize: 12)),
                    selected: _filterType == type,
                    onSelected: (_) {
                      setState(() => _filterType = type);
                      _load();
                    },
                  ),
                );
              }).toList(),
            ),
          ),
          Expanded(
            child: _loading
                ? const Center(child: CircularProgressIndicator())
                : _events.isEmpty
                    ? Center(
                        child: Text(l10n.historyEmpty,
                            style: const TextStyle(color: Colors.grey)))
                    : ListView.builder(
                        itemCount: _events.length,
                        itemBuilder: (ctx, i) {
                          final e = _events[i];
                          final color = _colorFor(e.eventType);
                          return IntrinsicHeight(
                            child: Row(
                              crossAxisAlignment:
                                  CrossAxisAlignment.stretch,
                              children: [
                                SizedBox(
                                  width: 48,
                                  child: Column(children: [
                                    if (i > 0)
                                      Container(
                                          width: 2,
                                          height: 12,
                                          color: Colors.grey.shade300),
                                    CircleAvatar(
                                      radius: 16,
                                      backgroundColor:
                                          color.withValues(alpha: 0.15),
                                      child: Icon(_iconFor(e.eventType),
                                          size: 16, color: color),
                                    ),
                                    Expanded(
                                        child: Container(
                                            width: 2,
                                            color: Colors.grey.shade300)),
                                  ]),
                                ),
                                Expanded(
                                  child: Padding(
                                    padding: const EdgeInsets.fromLTRB(
                                        8, 8, 16, 8),
                                    child: Column(
                                      crossAxisAlignment:
                                          CrossAxisAlignment.start,
                                      children: [
                                        Text(e.title,
                                            style: const TextStyle(
                                                fontWeight: FontWeight.w600,
                                                fontSize: 14)),
                                        if (e.details != null)
                                          Text(e.details!,
                                              style: const TextStyle(
                                                  fontSize: 12,
                                                  color: Colors.grey)),
                                        const SizedBox(height: 4),
                                        Row(children: [
                                          if (e.workOrderTitle != null)
                                            Expanded(
                                              child: Text(
                                                  e.workOrderTitle!,
                                                  style: const TextStyle(
                                                      fontSize: 11,
                                                      color: Color(
                                                          0xFF1e3a5f))),
                                            ),
                                          Text(
                                            e.timestamp.length >= 16
                                                ? e.timestamp
                                                    .substring(0, 16)
                                                    .replaceAll('T', ' ')
                                                : e.timestamp,
                                            style: const TextStyle(
                                                fontSize: 11,
                                                color: Colors.grey),
                                          ),
                                        ]),
                                        if (e.actor != null)
                                          Text(e.actor!,
                                              style: const TextStyle(
                                                  fontSize: 11,
                                                  color: Colors.grey)),
                                      ],
                                    ),
                                  ),
                                ),
                              ],
                            ),
                          );
                        },
                      ),
          ),
        ],
      ),
    );
  }
}
