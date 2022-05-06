import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:flutter/services.dart';

class AppSettings with ChangeNotifier {
  final String? apiUrl;
  final String? xCorrelationId;
  AppSettings({this.apiUrl, this.xCorrelationId});

  static Future<AppSettings> forEnvironment(
      String? env, String? xCorrelationId) async {
    env = env ?? 'dev';
    //final appSettings = await AppSettings.forEnvironment(env, xCorrelationId);
    WidgetsFlutterBinding.ensureInitialized();
    final contents = await rootBundle.loadString('assets/config/$env.json');

    final json = jsonDecode(contents);
    return AppSettings(
        apiUrl: json['apiUrl'], xCorrelationId: json['xCorrelationId']);
  }
}
