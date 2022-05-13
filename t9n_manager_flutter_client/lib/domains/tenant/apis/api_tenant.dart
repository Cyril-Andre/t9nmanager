import 'package:dio/dio.dart';
import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;
import 'package:t9n_manager_flutter_client/shared/models/api_exception.dart';
import 'dart:convert';
import '../../../generated/l10n.dart';
import '../../../shared/app_settings.dart';
import '../../../shared/models/api_message.dart';
import '../models/tenant.dart';

Future<List<Tenant>> getAllTenants(AppSettings appSettings, String jwt, BuildContext context) async {
  try {
    //Map<String, String> headers = {'Content-Type': 'application/json', 'X-Correlation-Id': appSettings.xCorrelationId ?? '', 'authorization': 'Bearer $jwt'};
    var dio = Dio();
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
      throw ApiException(S.of(context).exception_message_userunknown);
    } else if (e.response?.statusCode == 400) {
      throw ApiException(S.of(context).exception_message_wrongtoken);
    } 
    else if (e.type==DioErrorType.connectTimeout || e.type==DioErrorType.receiveTimeout || e.type==DioErrorType.sendTimeout){
      throw ApiException(S.of(context).exception_message_timeout);
    }
    else {
      throw ApiException(S.of(context).exception_message_unexpected);
    }
  }
}

Future<ApiMessage> onValue(http.Response response) async {
  final responseData = response.body;
  var apiMessage = ApiMessage.fromJson(jsonDecode(responseData));
  return apiMessage;
}

Future<ApiMessage> onError(error) async {
  final responseData = error.message;
  var apiMessage = ApiMessage(0, 'Client error', responseData);
  return apiMessage;
}
