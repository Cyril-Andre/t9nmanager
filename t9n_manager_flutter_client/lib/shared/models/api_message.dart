class ApiMessage {
  int httpStatus = 200;
  String message = "";
  String moreInfo = "";
  dynamic? value;

  ApiMessage(this.httpStatus, this.message, this.moreInfo, this.value);

  factory ApiMessage.fromJson(Map<String, dynamic> json) {
    return ApiMessage(json['httpStatus'] ?? 200, json['message'] ?? '', json['moreInfo'] ?? '', json['value']);
  }

  Map<String, dynamic> toJson() {
    return <String, dynamic>{'httpStatus': httpStatus.toString(), 'message': message, 'moreInfo': moreInfo, 'value':value};
  }
}
