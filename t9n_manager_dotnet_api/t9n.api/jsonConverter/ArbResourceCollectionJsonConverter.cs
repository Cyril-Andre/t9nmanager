using arb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace t9n.api.jsonConverter
{
    public class ArbResourceCollectionJsonConverter : JsonConverter<ArbResourceCollection>
    {
        public override ArbResourceCollection Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            ArbResourceEntry arbResourceEntry = null;
            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException("Expected StartObject token");
            var arbResourceCollection = new ArbResourceCollection();
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                    return arbResourceCollection;
                if (reader.TokenType!=JsonTokenType.PropertyName)
                    throw new JsonException("Expected PropertyName token");
                var propName = reader.GetString();
                reader.Read();
                if (string.Equals("@@", propName.Substring(0, 2), StringComparison.OrdinalIgnoreCase))
                {
                    if (string.Equals("@@locale", propName, StringComparison.OrdinalIgnoreCase)) // ARB collection locale
                    {
                        arbResourceCollection.Locale = reader.GetString();
                    }
                    if (string.Equals("@@context", propName, StringComparison.OrdinalIgnoreCase)) // ARB collection locale
                    {
                        arbResourceCollection.Context= reader.GetString();
                    }
                }
                else
                {
                    if (string.Equals("@", propName.Substring(0, 1))) // ARB Entry description
                    {
                        if (arbResourceEntry == null || string.Equals(arbResourceEntry.Id.Substring(1, arbResourceEntry.Id.Length - 1), propName))// ARB entry attribute while entry itself is missing
                            throw new JsonException("Expected entry token but got attribute");
                        else
                        {
                            if (reader.TokenType != JsonTokenType.StartObject) throw new JsonException("Expected start object token");
                            while (reader.Read()) // parse all attributes
                            {
                                if (reader.TokenType != JsonTokenType.EndObject)
                                {

                                    if (reader.TokenType != JsonTokenType.PropertyName) throw new JsonException("Expected PropertyName token");
                                    var attributePropName = reader.GetString();
                                    reader.Read();
                                    var arbResourceEntryAttribute = new ArbReourceEntryAttribute(attributePropName) { AttributeValue = reader.GetString() };
                                    switch (arbResourceEntryAttribute.AttributeName.ToLower())
                                    {
                                        case "type":
                                            arbResourceEntry.Type = arbResourceEntryAttribute;
                                            break;
                                        case "context":
                                            arbResourceEntry.Context = arbResourceEntryAttribute;
                                            break;
                                        case "description":
                                            arbResourceEntry.Description = arbResourceEntryAttribute;
                                            break;
                                        case "source_text":
                                            arbResourceEntry.Source_text = arbResourceEntryAttribute;
                                            break;
                                        case "screen":
                                            arbResourceEntry.Screen = arbResourceEntryAttribute;
                                            break;
                                    }
                                }
                                else
                                    break;
                            }
                            if (arbResourceEntry != null)
                            {
                                arbResourceEntry = null;
                            }
                        }
                    }
                    else // ARB Entry
                    {
                        arbResourceEntry = new ArbResourceEntry(null) { Id = propName, Value= reader.GetString() };
                        arbResourceCollection.ArbEntries.Add(arbResourceEntry);
                    }
                }               
            }
            /*
            var arbResourceEntry = new ArbResourceEntry(null);
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                    return arbResourceEntry;
                if (reader.TokenType != JsonTokenType.PropertyName)
                    throw new JsonException("Expected PRopertyName token");
                var propName = reader.GetString();
                reader.Read();
                switch (propName)
                {
                    case nameof(arbResourceEntry.Id):
                        arbResourceEntry.Id = reader.GetString();
                        break;
                    case nameof(arbResourceEntry.Value):
                        arbResourceEntry.Value = reader.GetString();
                        break;
                    case "@"+nameof(arbResourceEntry.Id):
                        while (reader.Read())
                        {
                            if (reader.TokenType == JsonTokenType.EndObject) break;
                            if (reader.TokenType != JsonTokenType.StartArray) throw new JsonException("Expected StartObject token for attribute");
                            reader.Read();
                            if (reader.TokenType != JsonTokenType.PropertyName)
                                throw new JsonException("Expected PropertyName token for attribute");
                            var attributePropName = reader.GetString();
                            reader.Read();
                            var arbResourceEntryAttribute = new ArbReourceEntryAttribute(attributePropName) { AttributeValue = reader.GetString() };
                            switch (arbResourceEntryAttribute.AttributeName)
                            {
                                case nameof(arbResourceEntry.Type):
                                    arbResourceEntry.Type = arbResourceEntryAttribute;
                                    break;
                                case nameof(arbResourceEntry.Context):
                                    arbResourceEntry.Context = arbResourceEntryAttribute;
                                    break;
                                case nameof(arbResourceEntry.Description):
                                    arbResourceEntry.Description = arbResourceEntryAttribute;
                                    break;
                                case nameof(arbResourceEntry.Source_text):
                                    arbResourceEntry.Source_text = arbResourceEntryAttribute;
                                    break;
                                case nameof(arbResourceEntry.Screen):
                                    arbResourceEntry.Screen = arbResourceEntryAttribute;
                                    break;
                            }
                        }
                        break;
                }
            }
            */
            throw new JsonException("Expected EndObjerct token");
        }

        public override void Write(Utf8JsonWriter writer, ArbResourceCollection value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            if (!string.IsNullOrEmpty(value.Locale)) writer.WriteString("@@locale", value.Locale);
            if (!string.IsNullOrEmpty(value.Context)) writer.WriteString("@@context", value.Context);
            foreach (var ae in value.ArbEntries)
            {
                writer.WriteString(ae.Id, ae.Value);
                if (ae.Type != null || ae.Context != null || ae.Description != null || ae.Screen != null || ae.Source_text != null)
                {
                    writer.WritePropertyName(JsonEncodedText.Encode("@" + ae.Id));
                    writer.WriteStartObject();
                    if (ae.Type != null) writer.WriteString(ae.Type.AttributeName, ae.Type.AttributeValue);
                    if (ae.Context != null) writer.WriteString(ae.Context.AttributeName, ae.Context.AttributeValue);
                    if (ae.Description != null) writer.WriteString(ae.Description.AttributeName, ae.Description.AttributeValue);
                    if (ae.Screen != null) writer.WriteString(ae.Screen.AttributeName, ae.Screen.AttributeValue);
                    if (ae.Source_text != null) writer.WriteString(ae.Source_text.AttributeName, ae.Source_text.AttributeValue);
                    writer.WriteEndObject();
                }
            }
            writer.WriteEndObject();
        }
    }
}
