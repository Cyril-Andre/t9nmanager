import '../../tenant/models/tenant.dart';

class CreateProjectRequestModel {
  Tenant? tenant;
  String projectName = '';

  CreateProjectRequestModel(this.tenant, this.projectName);

  factory CreateProjectRequestModel.fromJson(Map<String, dynamic> json) {
    return CreateProjectRequestModel(json['tenant'], json['projectName']);
  }

  Map<String, dynamic> toJson() {
    return <String, dynamic>{'tenant': tenant?.toJson(), 'projectName': projectName};
  }
}
