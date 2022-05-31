import 'package:dio/dio.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:t9n_manager_flutter_client/shared/app_settings.dart';
import 'package:t9n_manager_flutter_client/shared/models/api_message.dart';

Future<ApiMessage> postUploadArbFile(AppSettings appSettings, String jwt, BuildContext context, List<int> bytes) async {
  try {
    String? xCorrelationId = context.read<AppSettings>().xCorrelationId;
    String? apiUrl = context.read<AppSettings>().apiUrl;
    Dio dio = Dio();
    dio.options.sendTimeout = 2000;
    dio.options.receiveTimeout = 2000;
    dio.options.headers['content-type'] = 'application/json';
    dio.options.headers['X-Correlation-Id'] = xCorrelationId ?? '';

    FormData formData = FormData.fromMap({
      "file": await MultipartFile.fromBytes(bytes), // fromFile(filePath, filename: filename),
    });
    var response = await dio.post("${apiUrl}resource/arb", data: formData);
    return (response as ApiMessage);
  } on DioError catch (e) {
    return ApiMessage(500, "error", "", null);
  }
}
