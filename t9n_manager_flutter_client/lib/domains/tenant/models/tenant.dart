class Tenant {
  String tenantKey = '';
  String tenantName = '';
  String adminUserName = '';
  Tenant(this.tenantKey, this.tenantName, this.adminUserName);

  factory Tenant.fromJson(Map<String, dynamic> json) {
    return Tenant(json['tenantKey'], json['tenantName'], json['adminUserName']);
  }

  Map<String, dynamic> toJson() {
    return <String, dynamic>{'tenantKey': tenantKey, 'tenantName': tenantName, 'adminUserName':adminUserName};
  }
}
