class Tenant {
  String id = '';
  String name = '';
  String adminUserName = '';
  Tenant(this.id, this.name, this.adminUserName);

  factory Tenant.fromJson(Map<String, dynamic> json) {
    return Tenant(json['id'], json['name'], json['adminUserName']);
  }

  Map<String, dynamic> toJson() {
    return <String, dynamic>{'id': id, 'name': name, 'adminUserName': adminUserName};
  }
}
