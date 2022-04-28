
import 'package:intl/intl.dart';

class User {
  String userName = '';
  String userEmail = '';
  String userPassword = '';
  DateTime userBirthdate = DateTime(DateTime.now().year - 13, DateTime.now().month, DateTime.now().day);

  User(this.userName, this.userEmail, this.userBirthdate, this.userPassword);

  factory User.fromJson(Map<String,dynamic> json){
    return User(
      json['userName']??'', 
      json['userEmail']??'', 
      json['userBirthdate']??'', 
      json['userPassword']??'');
  }

  Map<String,dynamic> toJson(){
    return <String,dynamic>{
      'userName':userName,
      'userEmail':userEmail,
      'userBirthdate':DateFormat('yyyy-MM-dd').format(userBirthdate),
      'userPassword':userPassword
    };
  }
}

