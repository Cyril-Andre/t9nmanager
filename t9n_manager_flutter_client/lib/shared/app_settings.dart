import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:flutter/services.dart';

import '../domains/tenant/models/tenant.dart';

class AppSettings with ChangeNotifier {
  final String apiUrl;
  final String? xCorrelationId;
  final Tenant publicTenant;
  AppSettings({required this.apiUrl, this.xCorrelationId,required this.publicTenant});

  static Future<AppSettings> forEnvironment(
      String? env, String? xCorrelationId) async {
    env = env ?? 'dev';
    WidgetsFlutterBinding.ensureInitialized();
    final contents = await rootBundle.loadString('assets/config/$env.json');

    final json = jsonDecode(contents);
    
    return AppSettings(
        apiUrl: json['apiUrl'], 
        xCorrelationId: json['xCorrelationId'], 
        publicTenant: Tenant.fromJson(json['publicTenant'])
        );
  }
}
