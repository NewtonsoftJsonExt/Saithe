using System;

namespace CSharpTypes;

/// <summary>
/// Customer identifier, simple wrapper around long value. Since it wraps long we need to use the JsonConverter
/// </summary>
[Newtonsoft.Json.JsonConverter(typeof(Saithe.NewtonsoftJson.ValueTypeJsonConverter<CustomerId>)),
 System.Text.Json.Serialization.JsonConverter(typeof(Saithe.SystemTextJson.ValueTypeLongConverter<CustomerId>))]
public struct CustomerId : IEquatable<CustomerId>
{
    public readonly long Value;

    public CustomerId(long value) => this.Value = value;

    public readonly static CustomerId Empty = new CustomerId();

    public bool Equals(CustomerId other) => Equals(Value, other.Value);

    public override bool Equals(object obj) => obj is CustomerId id && Equals(id);

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => Value.ToString();
}