import 'dart:io';

import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:t9n_manager_flutter_client/shared/app_settings.dart';
import 'package:uuid/uuid.dart';

import 'package:t9n_manager_flutter_client/shared/app_state_notifier.dart';
import 'package:t9n_manager_flutter_client/t9manager_app.dart';

class MyHttpOverrides extends HttpOverrides {
  @override
  HttpClient createHttpClient(SecurityContext? context) {
    return super.createHttpClient(context)..badCertificateCallback = (X509Certificate cert, String host, int port) => true;
  }
}

void main({String? env}) async {
  if (env == null || env == "dev") {
    HttpOverrides.global = MyHttpOverrides(); // To be removed in Prod
  }
  var uuid = const Uuid();
  var xCorrelationId = uuid.v4();
  final appSettings = await AppSettings.forEnvironment(env, xCorrelationId);
  final appState = AppState();
  appState.setTenant(appSettings.publicTenant);
  runApp(
    MultiProvider(providers: [
      ChangeNotifierProvider(create: (_) => appState),
      ChangeNotifierProvider(create: (_) => appSettings),
    ], child: const t9nManagerApp()),
  );
}
