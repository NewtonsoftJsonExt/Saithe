using System;

namespace CSharpTypes;

/// <summary>
/// Order identifier, simple wrapper around long value. Since it wraps long we need to use the JsonConverter
/// </summary>
[Newtonsoft.Json.JsonConverter(typeof(Saithe.NewtonsoftJson.ValueTypeJsonConverter<OrderId>)),
 System.Text.Json.Serialization.JsonConverter(typeof(Saithe.SystemTextJson.ValueTypeLongJsonConverter<OrderId>))]
public struct OrderId : IEquatable<OrderId>
{
    public readonly long Value;

    public OrderId(long value) => this.Value = value;

    public readonly static OrderId Empty = new OrderId();

    public bool Equals(OrderId other) => Equals(Value, other.Value);

    public override bool Equals(object obj)
    {
        if (obj is OrderId id)
            return Equals(id);
        return false;
    }
    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => Value.ToString();
}