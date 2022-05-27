class UserResetPasswordModel {
  String userEmail = '';
  String? otp;
  String? userPassword;

  UserResetPasswordModel(this.userEmail, this.otp, this.userPassword);
  factory UserResetPasswordModel.fromJson(Map<String, dynamic> json) {
    return UserResetPasswordModel(json['userEmail'] ?? '', json['otp'],json['userPassword']);
  }

  Map<String, dynamic> toJson() {
    if (otp == null || userPassword==null) {
      return <String, dynamic>{'userEmail': userEmail};
    }
    return <String, dynamic>{'userEmail': userEmail, 'otp': otp, 'userPassword':userPassword};
  }
}
