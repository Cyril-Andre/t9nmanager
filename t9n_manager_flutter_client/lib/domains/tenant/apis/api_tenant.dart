import 'dart:convert';
import 'package:dio/dio.dart';
import 'package:flutter/material.dart';
import 'package:t9n_manager_flutter_client/shared/models/api_exception.dart';
import 'package:t9n_manager_flutter_client/generated/l10n.dart';
import 'package:t9n_manager_flutter_client/shared/app_settings.dart';
import 'package:t9n_manager_flutter_client/shared/models/api_message.dart';
import 'package:t9n_manager_flutter_client/domains/tenant/models/invitation.dart';
import 'package:t9n_manager_flutter_client/domains/tenant/models/tenant.dart';

Future<List<Tenant>> getAllTenants(AppSettings appSettings, String jwt, BuildContext context) async {
  try {
    Dio dio = Dio();
    dio.options.sendTimeout = 2000;
    dio.options.receiveTimeout = 2000;
    dio.options.headers['content-type'] = 'application/json';
    dio.options.headers['X-Correlation-Id'] = appSettings.xCorrelationId ?? '';
    dio.options.headers['authorization'] = 'Bearer $jwt';
    var response = await dio.get("${appSettings.apiUrl}tenant");
    final ApiMessage apiMessage = ApiMessage.fromJson(response.data);
    final List<Tenant> tenants = (apiMessage.value as List).map((e) => Tenant.fromJson(e as Map<String, dynamic>)).toList();
    tenants.add(appSettings.publicTenant);
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

Future<ApiMessage> deleteTenant(AppSettings appSettings, String jwt, BuildContext context, Tenant tenant) async {
  var xCorrelationId = appSettings.xCorrelationId;
  var apiUrl = appSettings.apiUrl;
  try {
    Dio dio = Dio();
    dio.options.sendTimeout = 2000;
    dio.options.receiveTimeout = 2000;
    dio.options.headers['content-type'] = 'application/json';
    dio.options.headers['X-Correlation-Id'] = xCorrelationId ?? '';
    dio.options.headers['authorization'] = 'Bearer $jwt';
    var response = await dio.delete("${apiUrl}tenant/${tenant.id}");
    var apiMessage = ApiMessage.fromJson(response.data);
    return apiMessage;
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

Future<Tenant> createTenant(AppSettings appSettings, String jwt, BuildContext context, String tenantName) async {
  var xCorrelationId = appSettings.xCorrelationId;
  var apiUrl = appSettings.apiUrl;
  try {
    Dio dio = Dio();
    dio.options.sendTimeout = 2000;
    dio.options.receiveTimeout = 2000;
    dio.options.headers['content-type'] = 'application/json';
    dio.options.headers['X-Correlation-Id'] = xCorrelationId ?? '';
    dio.options.headers['authorization'] = 'Bearer $jwt';
    var response = await dio.post("${apiUrl}tenant/create/$tenantName");
    final ApiMessage apiMessage = ApiMessage.fromJson(response.data);

    final Tenant tenant = Tenant.fromJson(apiMessage.value);
    return tenant;
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

Future<ApiMessage> inviteUser(AppSettings appSettings, String jwt, BuildContext context, Invitation invitation) async {
  var xCorrelationId = appSettings.xCorrelationId;
  var apiUrl = appSettings.apiUrl;
  try {
    Dio dio = Dio();
    dio.options.sendTimeout = 2000;
    dio.options.receiveTimeout = 2000;
    dio.options.headers['content-type'] = 'application/json';
    dio.options.headers['X-Correlation-Id'] = xCorrelationId ?? '';
    dio.options.headers['authorization'] = 'Bearer $jwt';
    var body = jsonEncode(invitation.toJson());

    var response = await dio.post("${apiUrl}tenant/invite", data: body);
    ApiMessage apiMessage = ApiMessage.fromJson(response.data);
    return apiMessage;
  } on DioError catch (e) {
    if (e.response?.statusCode == 401) {
      return ApiMessage(401, S.of(context).exception_message_loginfailed, "", null);
    } else if (e.response?.statusCode == 400) {
      return ApiMessage(400, S.of(context).exception_message_wrongpayload, "", null);
    } else if (e.type == DioErrorType.connectTimeout || e.type == DioErrorType.receiveTimeout || e.type == DioErrorType.sendTimeout) {
      return ApiMessage(408, S.of(context).exception_message_timeout, "", null);
    } else {
      return ApiMessage(500, S.of(context).exception_message_unexpected, "", null);
    }
  }
}
