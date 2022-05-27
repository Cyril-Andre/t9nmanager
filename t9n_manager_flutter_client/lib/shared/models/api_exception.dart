class ApiException implements Exception {
  String t9nMessage = '';
  int code = 0;
  ApiException(this.code,this.t9nMessage);
}
