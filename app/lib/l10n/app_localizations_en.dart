// ignore: unused_import
import 'package:intl/intl.dart' as intl;
import 'app_localizations.dart';

// ignore_for_file: type=lint

/// The translations for English (`en`).
class AppLocalizationsEn extends AppLocalizations {
  AppLocalizationsEn([String locale = 'en']) : super(locale);

  @override
  String get appTitle => 'Maintenance System';

  @override
  String get navAssets => 'Assets';

  @override
  String get navScan => 'Scan';

  @override
  String get scanInstruction => 'Hold QR code inside the frame';

  @override
  String get scanInvalidCode => 'Invalid QR code. Please scan an asset code.';

  @override
  String get scanAssetNotFound => 'Asset not found. Please try again.';

  @override
  String get assetsTitle => 'Assets';

  @override
  String get assetsEmpty => 'No assets found.';

  @override
  String get assetsLoading => 'Loading…';

  @override
  String get assetsErrorLoading => 'Error loading assets';

  @override
  String get detailLocation => 'Location';

  @override
  String get detailDescription => 'Description';

  @override
  String get detailCreated => 'Created';

  @override
  String get detailQrCode => 'QR Code';

  @override
  String get detailWorkOrders => 'Recent Work Orders';

  @override
  String get detailNoWorkOrders => 'No work orders found.';

  @override
  String get detailError => 'Error';

  @override
  String get statusOpen => 'Open';

  @override
  String get statusAssigned => 'Assigned';

  @override
  String get statusInProgress => 'In Progress';

  @override
  String get statusDone => 'Done';

  @override
  String get navMyTasks => 'Tasks';

  @override
  String get myTasksTitle => 'My Tasks';

  @override
  String get myTasksEmpty => 'No tasks assigned.';

  @override
  String get retry => 'Retry';
}
