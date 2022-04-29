import 'dart:convert';
import 'package:flutter_test/flutter_test.dart';
import 'package:t9n_manager_flutter_client/domains/arb_management/models/arb_resource_collection.dart';

void main() {
  test('Deserialize ARB Entry json string', () async {
    var json =
        '{"title":"Genome Master","@title":{"type":"text","context":"login screen","description":"Name of the application","screen":null,"source_text":"Genome Master"}}';
    Map<String, dynamic> jsonObject = await jsonDecode(json);
    expect(jsonObject['title'], 'Genome Master');

    ArbResourceEntry deserialized = ArbResourceEntry.fromJson(jsonObject);
    expect(deserialized.id, "title");
    expect(deserialized.value, "Genome Master");
    expect(deserialized.description, "Name of the application");
  });

  test('Serialize ArbEntry', () async {
    var jsonReference =
        '{"title":"Genome Master","@title":{"type":"text","context":"login screen","description":"Name of the application","source_text":"","screen":null}}';
    ArbResourceEntry arbEntry = ArbResourceEntry("title", "Genome Master", "text", "login screen", "Name of the application", "", null);
    var json = arbEntry.toJson();
    var comparison = jsonEncode(json);
    expect(comparison, jsonReference);
  });

  test('Deserialize ARB Collection', () async {
    var json =
        '{"@@locale":"en","@@context":"a","title":"Genome Master","@title":{"type":"text","context":"login screen","description":"Name of the application","screen":null,"source_text":"Genome Master"},"test_plural":"{age,plural,=1{Tu as {age} an.}other{Tu as {age} ans.}}","@test_plural":{"type":null,"context":null,"description":null,"screen":null,"source_text":null}}';
    Map<String, dynamic> jsonObject = await jsonDecode(json);
    expect(jsonObject['@@locale'], "en");

    ArbResourceCollection deserialized = ArbResourceCollection.fromJson(jsonObject);
    expect(deserialized.locale, "en");
    expect(deserialized.context, "a");
    int nbAfterDeserialize = deserialized.arbEntries.length;
    print("Total entries = $nbAfterDeserialize");
    var title = deserialized.arbEntries.firstWhere((element) => element.id == "title");
    expect(title.id, "title");
    expect(title.value, "Genome Master");
    expect(title.description, "Name of the application");

    var test_plural = deserialized.arbEntries.firstWhere((element) => element.id == "test_plural");
    expect(test_plural.id, "test_plural");
  });
}
