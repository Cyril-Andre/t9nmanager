import 'dart:convert';
import 'package:dio/dio.dart';
import 'package:flutter/material.dart';
import 'package:t9n_manager_flutter_client/domains/project/models/create_project_request_model.dart';
import 'package:t9n_manager_flutter_client/domains/project/models/project.dart';
import 'package:t9n_manager_flutter_client/shared/models/api_exception.dart';
import 'package:t9n_manager_flutter_client/generated/l10n.dart';
import 'package:t9n_manager_flutter_client/shared/app_settings.dart';
import 'package:t9n_manager_flutter_client/shared/models/api_message.dart';
import 'package:t9n_manager_flutter_client/domains/tenant/models/tenant.dart';

Future<List<Project>> getAllProjects(AppSettings appSettings, String jwt, BuildContext context, Tenant tenant) async {
  try {
    Dio dio = Dio();
    dio.options.sendTimeout = 2000;
    dio.options.receiveTimeout = 2000;
    dio.options.headers['content-type'] = 'application/json';
    dio.options.headers['X-Correlation-Id'] = appSettings.xCorrelationId ?? '';
    dio.options.headers['authorization'] = 'Bearer $jwt';
    var response = await dio.get("${appSettings.apiUrl}project?tenantKey=${tenant.id}");
    final ApiMessage apiMessage = ApiMessage.fromJson(response.data);
    final List<Project> projects = (apiMessage.value as List).map((e) => Project.fromJson(e as Map<String, dynamic>)).toList();
    return projects;
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

Future<ApiMessage> deleteProject(AppSettings appSettings, String jwt, BuildContext context, Project project) async {
  var xCorrelationId = appSettings.xCorrelationId;
  var apiUrl = appSettings.apiUrl;
  try {
    Dio dio = Dio();
    dio.options.sendTimeout = 2000;
    dio.options.receiveTimeout = 2000;
    dio.options.headers['content-type'] = 'application/json';
    dio.options.headers['X-Correlation-Id'] = xCorrelationId ?? '';
    dio.options.headers['authorization'] = 'Bearer $jwt';
    var response = await dio.delete("${apiUrl}project/${project.id}");
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

Future<Project> createProject(AppSettings appSettings, String jwt, BuildContext context, Tenant tenant, String projectName) async {
  var xCorrelationId = appSettings.xCorrelationId;
  var apiUrl = appSettings.apiUrl;
  try {
    Dio dio = Dio();
    dio.options.sendTimeout = 2000;
    dio.options.receiveTimeout = 2000;
    dio.options.headers['content-type'] = 'application/json';
    dio.options.headers['X-Correlation-Id'] = xCorrelationId ?? '';
    dio.options.headers['authorization'] = 'Bearer $jwt';
    CreateProjectRequestModel request = CreateProjectRequestModel(tenant, projectName);
    var body = jsonEncode(request.toJson());

    var response = await dio.post("${apiUrl}project/create", data: body);

    final ApiMessage apiMessage = ApiMessage.fromJson(response.data);

    final Project project = Project.fromJson(apiMessage.value);
    return project;
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
