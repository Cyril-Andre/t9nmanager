
class ArbResourceCollection {
/*
{
  "@@locale": "en",
  "@@context": "a",
  
  "title": "Genome Master",
  "@title": {
    "type": "text",
    "context": "login screen",
    "description": "Name of the application",
    "screen": null,
    "source_text": "Genome Master"
  }
  
  ,
  "test_plural": "{age,plural,=1{Tu as {age} an.}other{Tu as {age} ans.}}",
  "@test_plural": {
    "type": null,
    "context": null,
    "description": null,
    "screen": null,
    "source_text": null
  }
}
*/

  String locale = 'en';
  String context = 'global';
  List<ArbResourceEntry> arbEntries = [];

  ArbResourceCollection(this.locale, this.context, this.arbEntries);

  factory ArbResourceCollection.fromJson(Map<String, dynamic> json) {
    var _locale = json['@@locale'];
    var _context = json['@@context'];
    Iterable<String> _ids = json.keys.where((element) => element.substring(0, 1) != "@");
    List<ArbResourceEntry> _entries = [];
    for (var element in _ids) {
      String entryId = element;
      String entryValue = json[element];
      var meta = json['@' + element];
      String? type;
      String? context;
      String? description;
      String? sourceText;
      String? screen;

      if (meta != null) {
        type = meta['type'];
        context = meta['context'];
        description = meta['description'];
        sourceText = meta['source_text'];
        screen = meta['screen'];
      }
      var entry = ArbResourceEntry(entryId, entryValue, type, context, description, sourceText, screen);
      _entries.add(entry);
    }
    return ArbResourceCollection(_locale, _context, _entries);
  }

  Map<String, dynamic> toJson() {
    Map<String, dynamic> result = {
      "@@locale": locale,
      "@@context": context,
    };
    for (var element in arbEntries) {
      result[element.id] = element.value;
      if (element.type != null || element.context != null || element.description != null || element.screen != null || element.source_text != null) {
        Map<String, dynamic> meta = {
          "type": element.type,
          "context": element.context,
          "description": element.description,
          "source_text": element.source_text,
          "screen": element.screen
        };
        result['@${element.id}'] = meta;
      }
    }
    return result;
  }
}

class ArbResourceEntry {
  String id = '';
  String value = '';
  // Meta data comes after @id
  String? type;
  String? context;
  String? description;
  // ignore: non_constant_identifier_names
  String? source_text;
  String? screen;

  ArbResourceEntry(this.id, this.value, this.type, this.context, this.description, this.source_text, this.screen);
/*
  "title": "Genome Master",
  "@title": {
    "type": "text",
    "context": "login screen",
    "description": "Name of the application",
    "screen": null,
    "source_text": "Genome Master"
  }
*/
  factory ArbResourceEntry.fromJson(Map<String, dynamic> json) {
    var _id = json.keys.firstWhere((element) => element.substring(0, 1) == '@');
    var _value = json[_id.substring(1, _id.length)];
    var meta = json[_id];
    return ArbResourceEntry(_id.substring(1, _id.length), _value, meta['type'], meta['context'], meta['description'], meta['source_text'], meta['screen']);
  }

  Map<String, dynamic> toJson() {
    Map<String, dynamic> meta = {"type": type, "context": context, "description": description, "source_text": source_text, "screen": screen};
    return <String, dynamic>{id: value, '@$id': meta};
  }
}
