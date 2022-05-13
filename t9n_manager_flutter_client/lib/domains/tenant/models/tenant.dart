class Tenant {
  String tenantKey = '';
  String tenantName = '';

  Tenant(this.tenantKey, this.tenantName);

  factory Tenant.fromJson(Map<String, dynamic> json) {
    return Tenant(json['tenantKey'], json['tenantName']);
  }

  Map<String, dynamic> toJson() {
    return <String, dynamic>{'tenantKey': tenantKey, 'tenantName': tenantName};
  }
}
