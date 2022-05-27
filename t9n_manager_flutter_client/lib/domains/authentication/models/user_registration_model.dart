import 'package:intl/intl.dart';

class UserRegistrationModel {
  String firstName = '';
  String lastName = '';
  String userName = '';
  String userEmail = '';
  String userPassword = '';
  DateTime userBirthdate = DateTime(DateTime.now().year - 13, DateTime.now().month, DateTime.now().day);

  UserRegistrationModel(this.userName, this.userEmail, this.userBirthdate, this.userPassword, this.firstName, this.lastName);

  factory UserRegistrationModel.fromJson(Map<String, dynamic> json) {
    return UserRegistrationModel(json['userName'] ?? '', json['userEmail'] ?? '', json['userBirthdate'] ?? '', json['userPassword'] ?? '', json['firstName']??'',json['lastName']??'');
  }

  Map<String, dynamic> toJson() {
    return <String, dynamic>{'userName': userName, 'userEmail': userEmail, 'userBirthdate': DateFormat('yyyy-MM-dd').format(userBirthdate), 'userPassword': userPassword, 'firstName':firstName, 'lastName':lastName};
  }
}
