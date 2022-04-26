import 'dart:async';
import 'dart:convert';
import 'package:t9n_manager_flutter_client/shared/app_settings.dart';
import 'package:t9n_manager_flutter_client/shared/app_state_notifier.dart';

import '../../../shared/models/api_message.dart';
import '../models/user_login_model.dart';
import '../models/user_registration_model.dart';
import 'package:http/http.dart' as http;

Future<ApiMessage> postSignup(UserRegistrationModel userRegistrationModel,
    AppSettings appSettings) async {
  Map<String, String> headers = {
    'Content-Type': 'application/json',
    'X-Correlation-Id': appSettings.xCorrelationId ?? ''
  };
  var body = jsonEncode(userRegistrationModel.toJson());
  var result = http
      .post(Uri.parse("${appSettings.apiUrl}user/register"),
          headers: headers, body: body)
      .then(onValue)
      .catchError(onError);
  return result;
}

Future<ApiMessage> postLogin(
    UserLoginModel userLoginModel, AppSettings appSettings) async {
  try {
    Map<String, String> headers = {
      'Content-Type': 'application/json',
      'X-Correlation-Id': appSettings.xCorrelationId ?? ''
    };
    var body = jsonEncode(userLoginModel.toJson());
    var result = http
        .post(Uri.parse("${appSettings.apiUrl}user/login"),
            headers: headers, body: body)
        .then(onValue)
        .catchError(onError);
    return result;
  } catch (ex) {
    rethrow;
  }
}

Future<ApiMessage> onValue(http.Response response) async {
  final responseData = response.body;
  var apiMessage = ApiMessage.fromJson(jsonDecode(responseData));
  return apiMessage;
}

Future<ApiMessage> onError(error) async {
  final responseData = error.message;
  var apiMessage = ApiMessage(0, 'Client error',  responseData);
  return apiMessage;
}
