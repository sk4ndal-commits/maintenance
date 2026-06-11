// ignore: unused_import
import 'package:intl/intl.dart' as intl;
import 'app_localizations.dart';

// ignore_for_file: type=lint

/// The translations for German (`de`).
class AppLocalizationsDe extends AppLocalizations {
  AppLocalizationsDe([String locale = 'de']) : super(locale);

  @override
  String get appTitle => 'Maintenance System';

  @override
  String get navAssets => 'Assets';

  @override
  String get navScan => 'Scannen';

  @override
  String get assetsTitle => 'Assets';

  @override
  String get assetsEmpty => 'Keine Assets vorhanden.';

  @override
  String get assetsLoading => 'Lädt…';

  @override
  String get assetsErrorLoading => 'Fehler beim Laden';

  @override
  String get detailLocation => 'Standort';

  @override
  String get detailDescription => 'Beschreibung';

  @override
  String get detailCreated => 'Erstellt';

  @override
  String get detailQrCode => 'QR-Code';

  @override
  String get detailWorkOrders => 'Letzte Work Orders';

  @override
  String get detailNoWorkOrders => 'Keine Work Orders vorhanden.';

  @override
  String get detailError => 'Fehler';

  @override
  String get statusOpen => 'Offen';

  @override
  String get statusAssigned => 'Zugewiesen';

  @override
  String get statusInProgress => 'In Bearbeitung';

  @override
  String get statusDone => 'Abgeschlossen';
}
