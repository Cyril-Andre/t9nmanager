import 'package:t9n_manager_flutter_client/domains/tenant/models/tenant.dart';

class Invitation {
  final Tenant tenant;
  final String userEmail;
  Invitation(this.tenant, this.userEmail);

  factory Invitation.fromJson(Map<String, dynamic> json){
    return Invitation(json['tenant'], json['userEmail']);
  }

  Map<String,dynamic> toJson(){
    return <String,dynamic> {'tenant':tenant.toJson(), 'userEmail':userEmail};
  }
}
