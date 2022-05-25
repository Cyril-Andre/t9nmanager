import 'dart:async';
import 'dart:convert';
import 'package:dio/dio.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:t9n_manager_flutter_client/domains/authentication/models/user_reinit_password_model.dart';
import 'package:t9n_manager_flutter_client/shared/app_settings.dart';

import '../../../generated/l10n.dart';
import '../../../shared/models/api_message.dart';
import '../models/user_login_model.dart';
import '../models/user_registration_model.dart';
import 'package:http/http.dart' as http;

Future<ApiMessage> postSignup(UserRegistrationModel userRegistrationModel, AppSettings appSettings) async {
  Map<String, String> headers = {'Content-Type': 'application/json', 'X-Correlation-Id': appSettings.xCorrelationId ?? ''};
  var body = jsonEncode(userRegistrationModel.toJson());
  var result = http.post(Uri.parse("${appSettings.apiUrl}user/register"), headers: headers, body: body).then(onValue).catchError(onError);
  return result;
}

Future<ApiMessage> postLogin(UserLoginModel userLoginModel, BuildContext context) async {
  try {
    String? xCorrelationId = context.read<AppSettings>().xCorrelationId;
    String? apiUrl = context.read<AppSettings>().apiUrl;
    Dio dio = Dio();
    dio.options.sendTimeout = 2000;
    dio.options.receiveTimeout = 2000;
    dio.options.headers['content-type'] = 'application/json';
    dio.options.headers['X-Correlation-Id'] = xCorrelationId ?? '';

    var body = jsonEncode(userLoginModel.toJson());
    var result = await dio.post("${apiUrl}user/login", data: body);
    ApiMessage apiMessage = ApiMessage.fromJson(result.data);
    return apiMessage;
  } on DioError catch (e) {
    if (e.response?.statusCode == 401) {
      return ApiMessage(401, S.of(context).exception_message_loginfailed, "",null);
    } else if (e.response?.statusCode == 400) {
      return ApiMessage(400, S.of(context).exception_message_wrongpayload, "",null);
    } else if (e.type == DioErrorType.connectTimeout || e.type == DioErrorType.receiveTimeout || e.type == DioErrorType.sendTimeout) {
      return ApiMessage(408, S.of(context).exception_message_timeout, "", null);
    } else {
      return ApiMessage(500, S.of(context).exception_message_unexpected, "", null);
    }
  }
}

Future<ApiMessage> postResetPassword1(UserResetPasswordModel userResetPasswordModel, AppSettings appSettings) async {
  Map<String, String> headers = {'Content-Type': 'application/json', 'X-Correlation-Id': appSettings.xCorrelationId ?? ''};
  var body = jsonEncode(userResetPasswordModel.toJson());
  var result = http.post(Uri.parse("${appSettings.apiUrl}user/startresetpassword"), headers: headers, body: body).then(onValue).catchError(onError);
  return result;
}

Future<ApiMessage> postResetPassword2(UserResetPasswordModel userResetPasswordModel, AppSettings appSettings) async {
  Map<String, String> headers = {'Content-Type': 'application/json', 'X-Correlation-Id': appSettings.xCorrelationId ?? ''};
  var body = jsonEncode(userResetPasswordModel.toJson());
  var result = http.post(Uri.parse("${appSettings.apiUrl}user/finalizeresetpassword"), headers: headers, body: body).then(onValue).catchError(onError);
  return result;
}

Future<ApiMessage> onValue(http.Response response) async {
  final responseData = response.body;
  var apiMessage = ApiMessage.fromJson(jsonDecode(responseData));
  return apiMessage;
}

Future<ApiMessage> onError(error) async {
  final responseData = error.message;
  var apiMessage = ApiMessage(0, 'Client error', responseData, null);
  return apiMessage;
}
