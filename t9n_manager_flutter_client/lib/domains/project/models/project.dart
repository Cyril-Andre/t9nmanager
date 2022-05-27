class Project {
  String id = '';
  String name = '';

  Project(this.id, this.name);

  factory Project.fromJson(Map<String, dynamic> json) {
    return Project(json['id'], json['name']);
  }

  Map<String, dynamic> toJson() {
    return <String, dynamic>{'id': id, 'name': name};
  }
}
