class UserLoginModel {
  String userName = '';
  String password = '';

  UserLoginModel(this.userName, this.password);

  factory UserLoginModel.fromJson(Map<String, dynamic> json) {
    return UserLoginModel(json['userName'] ?? '', json['userPassword'] ?? '');
  }

  Map<String, dynamic> toJson() {
    return <String, dynamic>{
      'userName': userName,
      'password': password
    };
  }
}
