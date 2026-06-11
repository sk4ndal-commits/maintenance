import 'dart:async';

import 'package:flutter/foundation.dart';
import 'package:flutter/widgets.dart';
import 'package:flutter_localizations/flutter_localizations.dart';
import 'package:intl/intl.dart' as intl;

import 'app_localizations_de.dart';
import 'app_localizations_en.dart';

// ignore_for_file: type=lint

/// Callers can lookup localized strings with an instance of AppLocalizations
/// returned by `AppLocalizations.of(context)`.
///
/// Applications need to include `AppLocalizations.delegate()` in their app's
/// `localizationDelegates` list, and the locales they support in the app's
/// `supportedLocales` list. For example:
///
/// ```dart
/// import 'l10n/app_localizations.dart';
///
/// return MaterialApp(
///   localizationsDelegates: AppLocalizations.localizationsDelegates,
///   supportedLocales: AppLocalizations.supportedLocales,
///   home: MyApplicationHome(),
/// );
/// ```
///
/// ## Update pubspec.yaml
///
/// Please make sure to update your pubspec.yaml to include the following
/// packages:
///
/// ```yaml
/// dependencies:
///   # Internationalization support.
///   flutter_localizations:
///     sdk: flutter
///   intl: any # Use the pinned version from flutter_localizations
///
///   # Rest of dependencies
/// ```
///
/// ## iOS Applications
///
/// iOS applications define key application metadata, including supported
/// locales, in an Info.plist file that is built into the application bundle.
/// To configure the locales supported by your app, you’ll need to edit this
/// file.
///
/// First, open your project’s ios/Runner.xcworkspace Xcode workspace file.
/// Then, in the Project Navigator, open the Info.plist file under the Runner
/// project’s Runner folder.
///
/// Next, select the Information Property List item, select Add Item from the
/// Editor menu, then select Localizations from the pop-up menu.
///
/// Select and expand the newly-created Localizations item then, for each
/// locale your application supports, add a new item and select the locale
/// you wish to add from the pop-up menu in the Value field. This list should
/// be consistent with the languages listed in the AppLocalizations.supportedLocales
/// property.
abstract class AppLocalizations {
  AppLocalizations(String locale)
    : localeName = intl.Intl.canonicalizedLocale(locale.toString());

  final String localeName;

  static AppLocalizations? of(BuildContext context) {
    return Localizations.of<AppLocalizations>(context, AppLocalizations);
  }

  static const LocalizationsDelegate<AppLocalizations> delegate =
      _AppLocalizationsDelegate();

  /// A list of this localizations delegate along with the default localizations
  /// delegates.
  ///
  /// Returns a list of localizations delegates containing this delegate along with
  /// GlobalMaterialLocalizations.delegate, GlobalCupertinoLocalizations.delegate,
  /// and GlobalWidgetsLocalizations.delegate.
  ///
  /// Additional delegates can be added by appending to this list in
  /// MaterialApp. This list does not have to be used at all if a custom list
  /// of delegates is preferred or required.
  static const List<LocalizationsDelegate<dynamic>> localizationsDelegates =
      <LocalizationsDelegate<dynamic>>[
        delegate,
        GlobalMaterialLocalizations.delegate,
        GlobalCupertinoLocalizations.delegate,
        GlobalWidgetsLocalizations.delegate,
      ];

  /// A list of this localizations delegate's supported locales.
  static const List<Locale> supportedLocales = <Locale>[
    Locale('de'),
    Locale('en'),
  ];

  /// No description provided for @appTitle.
  ///
  /// In de, this message translates to:
  /// **'Maintenance System'**
  String get appTitle;

  /// No description provided for @navAssets.
  ///
  /// In de, this message translates to:
  /// **'Assets'**
  String get navAssets;

  /// No description provided for @navScan.
  ///
  /// In de, this message translates to:
  /// **'Scannen'**
  String get navScan;

  /// No description provided for @scanInstruction.
  ///
  /// In de, this message translates to:
  /// **'QR-Code in den Rahmen halten'**
  String get scanInstruction;

  /// No description provided for @scanInvalidCode.
  ///
  /// In de, this message translates to:
  /// **'Ungültiger QR-Code. Bitte einen Asset-Code scannen.'**
  String get scanInvalidCode;

  /// No description provided for @scanAssetNotFound.
  ///
  /// In de, this message translates to:
  /// **'Asset nicht gefunden. Bitte erneut versuchen.'**
  String get scanAssetNotFound;

  /// No description provided for @assetsTitle.
  ///
  /// In de, this message translates to:
  /// **'Assets'**
  String get assetsTitle;

  /// No description provided for @assetsEmpty.
  ///
  /// In de, this message translates to:
  /// **'Keine Assets vorhanden.'**
  String get assetsEmpty;

  /// No description provided for @assetsLoading.
  ///
  /// In de, this message translates to:
  /// **'Lädt…'**
  String get assetsLoading;

  /// No description provided for @assetsErrorLoading.
  ///
  /// In de, this message translates to:
  /// **'Fehler beim Laden'**
  String get assetsErrorLoading;

  /// No description provided for @detailLocation.
  ///
  /// In de, this message translates to:
  /// **'Standort'**
  String get detailLocation;

  /// No description provided for @detailDescription.
  ///
  /// In de, this message translates to:
  /// **'Beschreibung'**
  String get detailDescription;

  /// No description provided for @detailCreated.
  ///
  /// In de, this message translates to:
  /// **'Erstellt'**
  String get detailCreated;

  /// No description provided for @detailQrCode.
  ///
  /// In de, this message translates to:
  /// **'QR-Code'**
  String get detailQrCode;

  /// No description provided for @detailWorkOrders.
  ///
  /// In de, this message translates to:
  /// **'Letzte Wartungsaufträge'**
  String get detailWorkOrders;

  /// No description provided for @detailNoWorkOrders.
  ///
  /// In de, this message translates to:
  /// **'Keine Wartungsaufträge vorhanden.'**
  String get detailNoWorkOrders;

  /// No description provided for @detailError.
  ///
  /// In de, this message translates to:
  /// **'Fehler'**
  String get detailError;

  /// No description provided for @statusOpen.
  ///
  /// In de, this message translates to:
  /// **'Offen'**
  String get statusOpen;

  /// No description provided for @statusAssigned.
  ///
  /// In de, this message translates to:
  /// **'Zugewiesen'**
  String get statusAssigned;

  /// No description provided for @statusInProgress.
  ///
  /// In de, this message translates to:
  /// **'In Bearbeitung'**
  String get statusInProgress;

  /// No description provided for @statusDone.
  ///
  /// In de, this message translates to:
  /// **'Abgeschlossen'**
  String get statusDone;

  /// No description provided for @navMyTasks.
  ///
  /// In de, this message translates to:
  /// **'Meine Aufgaben'**
  String get navMyTasks;

  /// No description provided for @myTasksTitle.
  ///
  /// In de, this message translates to:
  /// **'Meine Aufgaben'**
  String get myTasksTitle;

  /// No description provided for @myTasksEmpty.
  ///
  /// In de, this message translates to:
  /// **'Keine Aufgaben vorhanden.'**
  String get myTasksEmpty;

  /// No description provided for @retry.
  ///
  /// In de, this message translates to:
  /// **'Erneut versuchen'**
  String get retry;

  /// No description provided for @statusChangeToInProgress.
  ///
  /// In de, this message translates to:
  /// **'Starten'**
  String get statusChangeToInProgress;

  /// No description provided for @statusChangeToDone.
  ///
  /// In de, this message translates to:
  /// **'Abschließen'**
  String get statusChangeToDone;

  /// No description provided for @statusChangeError.
  ///
  /// In de, this message translates to:
  /// **'Statusänderung fehlgeschlagen: {message}'**
  String statusChangeError(String message);

  /// No description provided for @priorityLow.
  ///
  /// In de, this message translates to:
  /// **'Niedrig'**
  String get priorityLow;

  /// No description provided for @priorityMedium.
  ///
  /// In de, this message translates to:
  /// **'Mittel'**
  String get priorityMedium;

  /// No description provided for @priorityHigh.
  ///
  /// In de, this message translates to:
  /// **'Hoch'**
  String get priorityHigh;
}

class _AppLocalizationsDelegate
    extends LocalizationsDelegate<AppLocalizations> {
  const _AppLocalizationsDelegate();

  @override
  Future<AppLocalizations> load(Locale locale) {
    return SynchronousFuture<AppLocalizations>(lookupAppLocalizations(locale));
  }

  @override
  bool isSupported(Locale locale) =>
      <String>['de', 'en'].contains(locale.languageCode);

  @override
  bool shouldReload(_AppLocalizationsDelegate old) => false;
}

AppLocalizations lookupAppLocalizations(Locale locale) {
  // Lookup logic when only language code is specified.
  switch (locale.languageCode) {
    case 'de':
      return AppLocalizationsDe();
    case 'en':
      return AppLocalizationsEn();
  }

  throw FlutterError(
    'AppLocalizations.delegate failed to load unsupported locale "$locale". This is likely '
    'an issue with the localizations generation tool. Please file an issue '
    'on GitHub with a reproducible sample app and the gen-l10n configuration '
    'that was used.',
  );
}
