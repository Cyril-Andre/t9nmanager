class ApiMessage {
  int httpStatus = 200;
  String message = "";
  String moreInfo = "";
  
  ApiMessage(this.httpStatus, this.message, this.moreInfo);

  factory ApiMessage.fromJson(Map<String, dynamic> json) {
    return ApiMessage(json['httpStatus'] ?? 200,
        json['message'] ?? '', json['moreInfo'] ?? '');
  }

  Map<String, dynamic> toJson() {
    return <String, dynamic>{
      'httpStatus': httpStatus.toString(),
      'message': message,
      'moreInfo': moreInfo
    };
  }
}
