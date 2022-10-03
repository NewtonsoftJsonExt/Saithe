using System;

namespace CSharpTypes;

/// <summary>
/// Customer identifier, simple wrapper around long value. Since it wraps long we need to use the JsonConverter
/// </summary>
[Newtonsoft.Json.JsonConverter(typeof(Saithe.NewtonsoftJson.ValueTypeJsonConverter<CustomerId>)),
 System.Text.Json.Serialization.JsonConverter(typeof(Saithe.SystemTextJson.ValueTypeLongJsonConverter<CustomerId>))]
public record struct CustomerId(long Value)
{
    public override string ToString() => Value.ToString();
}