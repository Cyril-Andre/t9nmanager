import 'package:dio/dio.dart';

class DioHelper {
  static Dio? _instance;
  Dio getInstance() {
    _instance ??= createDioInstance();
    return _instance!;
  }

  Dio createDioInstance() {
    var dio = Dio();
    dio.interceptors.clear();
    dio.interceptors.add(InterceptorsWrapper(onRequest: (options, handler) {
      return handler.next(options);
    }, onResponse: (response, handler) {
      return handler.next(response);
    }, onError: (DioError e, handler) async {
      if (e.response != null) {
        if (e.response!.statusCode == 401) {
          throw e;
        } else {
          handler.next(e);
        }
      }
    }));
    return dio;
  }
}
