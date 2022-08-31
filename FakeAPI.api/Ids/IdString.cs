using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FakeAPI.Api;

[JsonConverter(typeof(IdStringJsonConverter))]
public class IdString : IdBase<string>
{
    public IdString(string id) : base(id) { }

    public static implicit operator string(IdString id) => id.Id;
    public static explicit operator IdString(string id) => new(id);
}

public class IdStringJsonConverter : JsonConverter<IdString>
{
    public override bool CanConvert(Type typeToConvert) => typeToConvert.IsSubclassOf(typeof(IdString));

    public override IdString Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return (IdString)Activator.CreateInstance(typeToConvert, reader.GetString())!;
    }

    public override void Write(Utf8JsonWriter writer, IdString value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}