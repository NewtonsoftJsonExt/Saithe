using System;

namespace CSharpTypes;

/// <summary>
/// Order identifier, simple wrapper around long value. Since it wraps long we need to use the JsonConverter
/// </summary>
[Newtonsoft.Json.JsonConverter(typeof(Saithe.NewtonsoftJson.ValueTypeJsonConverter<OrderId>)),
 System.Text.Json.Serialization.JsonConverter(typeof(Saithe.SystemTextJson.ValueTypeLongJsonConverter<OrderId>))]
public record struct OrderId(long Value)
{
    public override string ToString() => Value.ToString();
}