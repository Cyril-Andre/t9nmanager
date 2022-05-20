import 'package:dio/dio.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:t9n_manager_flutter_client/shared/app_state_notifier.dart';
import 'package:t9n_manager_flutter_client/shared/models/api_exception.dart';
import '../../../generated/l10n.dart';
import '../../../shared/app_settings.dart';
import '../models/tenant.dart';

Future<List<Tenant>> getAllTenants(AppSettings appSettings, String jwt, BuildContext context) async {
  try {
    //Map<String, String> headers = {'Content-Type': 'application/json', 'X-Correlation-Id': appSettings.xCorrelationId ?? '', 'authorization': 'Bearer $jwt'};
    Dio dio = Dio();
    dio.options.sendTimeout = 2000;
    dio.options.receiveTimeout = 2000;
    dio.options.headers['content-type'] = 'application/json';
    dio.options.headers['X-Correlation-Id'] = appSettings.xCorrelationId ?? '';
    dio.options.headers['authorization'] = 'Bearer $jwt';
    var response = await dio.get("${appSettings.apiUrl}tenant"); //http.get(Uri.parse("${appSettings.apiUrl}tenant"),headers: headers);
    final List<Tenant> tenants = (response.data as List).map((e) => Tenant.fromJson(e as Map<String, dynamic>)).toList();
    return tenants;
  } on DioError catch (e) {
    if (e.response?.statusCode == 404) {
      throw ApiException(404, S.of(context).exception_message_userunknown);
    } else if (e.response?.statusCode == 400) {
      throw ApiException(400, S.of(context).exception_message_wrongtoken);
    } else if (e.type == DioErrorType.connectTimeout || e.type == DioErrorType.receiveTimeout || e.type == DioErrorType.sendTimeout) {
      throw ApiException(408, S.of(context).exception_message_timeout);
    } else if (e.response?.statusCode == 401) {
      throw ApiException(401, S.of(context).exception_message_notauthoraized);
    } else {
      throw ApiException(500, S.of(context).exception_message_unexpected);
    }
  }
}

Future<List<Tenant>> deleteTenant(BuildContext context, Tenant tenant) async {
  var xCorrelationId = context.watch<AppSettings>().xCorrelationId;
  var jwt = context.watch<AppState>().jwt;
  var apiUrl = context.watch<AppSettings>().apiUrl;
  try {
    Dio dio = Dio();
    dio.options.sendTimeout = 2000;
    dio.options.receiveTimeout = 2000;
    dio.options.headers['content-type'] = 'application/json';
    dio.options.headers['X-Correlation-Id'] = xCorrelationId ?? '';
    dio.options.headers['authorization'] = 'Bearer $jwt';
    var response = await dio.delete("${apiUrl}tenant/${tenant.tenantKey}");
    //ApiMessage(sage, moreInfo)
    final List<Tenant> tenants = (response.data as List).map((e) => Tenant.fromJson(e as Map<String, dynamic>)).toList();
    return tenants;
  } on DioError catch (e) {
    if (e.response?.statusCode == 404) {
      throw ApiException(404, S.of(context).exception_message_tenantunknown);
    } else if (e.response?.statusCode == 400) {
      throw ApiException(400, S.of(context).exception_message_wrongtoken);
    } else if (e.type == DioErrorType.connectTimeout || e.type == DioErrorType.receiveTimeout || e.type == DioErrorType.sendTimeout) {
      throw ApiException(408, S.of(context).exception_message_timeout);
    } else if (e.response?.statusCode == 401) {
      throw ApiException(401, S.of(context).exception_message_notauthoraized);
    } else {
      throw ApiException(500, S.of(context).exception_message_unexpected);
    }
  }
}
