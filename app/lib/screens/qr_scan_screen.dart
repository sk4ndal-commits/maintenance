import 'package:flutter/material.dart';
import 'package:mobile_scanner/mobile_scanner.dart';
import '../l10n/app_localizations.dart';
import '../services/asset_service.dart';
import 'asset_detail_screen.dart';

const _primaryColor = Color(0xFF1e3a5f);

class QrScanScreen extends StatefulWidget {
  const QrScanScreen({super.key});

  @override
  State<QrScanScreen> createState() => _QrScanScreenState();
}

class _QrScanScreenState extends State<QrScanScreen> {
  final MobileScannerController _controller = MobileScannerController();
  bool _processing = false;

  @override
  void dispose() {
    _controller.dispose();
    super.dispose();
  }

  Future<void> _onDetect(BarcodeCapture capture) async {
    if (_processing) return;
    final barcode = capture.barcodes.firstOrNull;
    if (barcode == null || barcode.rawValue == null) return;

    final raw = barcode.rawValue!;
    if (!raw.startsWith('asset:')) {
      _showError(AppLocalizations.of(context)!.scanInvalidCode);
      return;
    }

    final assetId = raw.replaceFirst('asset:', '');
    setState(() => _processing = true);
    await _controller.stop();

    try {
      final asset = await AssetService().getById(assetId);
      if (!mounted) return;
      await Navigator.push(
        context,
        MaterialPageRoute(
          builder: (_) => AssetDetailScreen(assetId: asset.assetId),
        ),
      );
    } catch (_) {
      if (!mounted) return;
      _showError(AppLocalizations.of(context)!.scanAssetNotFound);
    } finally {
      if (mounted) {
        setState(() => _processing = false);
        await _controller.start();
      }
    }
  }

  void _showError(String message) {
    ScaffoldMessenger.of(context).showSnackBar(
      SnackBar(
        content: Text(message),
        backgroundColor: const Color(0xFFb91c1c),
        behavior: SnackBarBehavior.floating,
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    final l10n = AppLocalizations.of(context)!;
    return Stack(
      children: [
        MobileScanner(
          controller: _controller,
          onDetect: _onDetect,
        ),
        Center(
          child: Container(
            width: 260,
            height: 260,
            decoration: BoxDecoration(
              border: Border.all(color: Colors.white, width: 3),
            ),
          ),
        ),
        Positioned(
          bottom: 48,
          left: 0,
          right: 0,
          child: Center(
            child: Container(
              padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 8),
              color: _primaryColor.withValues(alpha: 0.85),
              child: Text(
                l10n.scanInstruction,
                style: const TextStyle(color: Colors.white, fontSize: 14),
              ),
            ),
          ),
        ),
        if (_processing)
          const Center(child: CircularProgressIndicator(color: Colors.white)),
      ],
    );
  }
}
