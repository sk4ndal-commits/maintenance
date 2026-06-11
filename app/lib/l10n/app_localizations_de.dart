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
  String get scanInstruction => 'QR-Code in den Rahmen halten';

  @override
  String get scanInvalidCode =>
      'Ungültiger QR-Code. Bitte einen Asset-Code scannen.';

  @override
  String get scanAssetNotFound =>
      'Asset nicht gefunden. Bitte erneut versuchen.';

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
  String get detailWorkOrders => 'Letzte Wartungsaufträge';

  @override
  String get detailNoWorkOrders => 'Keine Wartungsaufträge vorhanden.';

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

  @override
  String get navMyTasks => 'Meine Aufgaben';

  @override
  String get myTasksTitle => 'Meine Aufgaben';

  @override
  String get myTasksEmpty => 'Keine Aufgaben vorhanden.';

  @override
  String get retry => 'Erneut versuchen';

  @override
  String get statusChangeToInProgress => 'Starten';

  @override
  String get statusChangeToDone => 'Abschließen';

  @override
  String get checklistTitle => 'Checkliste';

  @override
  String get checklistMandatory => 'Pflicht';

  @override
  String get checklistEmpty => 'Keine Checklistenpunkte.';

  @override
  String get checklistBlockedHint =>
      'Alle Pflichtschritte müssen abgehakt sein.';

  @override
  String statusChangeError(String message) {
    return 'Statusänderung fehlgeschlagen: $message';
  }

  @override
  String get priorityLow => 'Niedrig';

  @override
  String get priorityMedium => 'Mittel';

  @override
  String get priorityHigh => 'Hoch';

  @override
  String get documentsTitle => 'Dokumente & Fotos';

  @override
  String get documentsNotes => 'Notizen';

  @override
  String get documentsCamera => 'Foto aufnehmen';

  @override
  String get documentsGallery => 'Aus Galerie';

  @override
  String get documentsEmpty => 'Keine Dokumente hochgeladen.';
}
