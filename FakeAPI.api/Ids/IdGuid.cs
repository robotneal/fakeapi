using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FakeAPI.Api;

[JsonConverter(typeof(IdGuidJsonConverter))]
public class IdGuid : IdBase<Guid>
{
    public IdGuid(Guid id) : base(id) { }
}

public class IdGuidJsonConverter : JsonConverter<IdGuid>
{
    public override bool CanConvert(Type typeToConvert) => typeToConvert.IsSubclassOf(typeof(IdGuid));

    public override IdGuid Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return (IdGuid)Activator.CreateInstance(typeToConvert, reader.GetGuid())!;
    }

    public override void Write(Utf8JsonWriter writer, IdGuid value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}