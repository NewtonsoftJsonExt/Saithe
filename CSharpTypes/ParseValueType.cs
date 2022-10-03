using System;
using System.ComponentModel;

namespace CSharpTypes;

[TypeConverter(typeof(Saithe.ParseTypeConverter<ParseValueType>)),
 Newtonsoft.Json.JsonConverter(typeof(Saithe.NewtonsoftJson.ParseTypeJsonConverter<ParseValueType>)),
 System.Text.Json.Serialization.JsonConverter(typeof(Saithe.SystemTextJson.ParseTypeJsonConverter<ParseValueType>))]
public class ParseValueType : IEquatable<ParseValueType>
{
    public readonly string Value;

    public ParseValueType(string value) => Value = value;

    public static ParseValueType Parse(string value)
    {
        var res = value.Split('_');
        if (res.Length == 2 && res[0] == "P") return new ParseValueType(res[1]);
        throw new ParseValueException($"Expected value to be in form: P_* but was '{value}'");
    }

    public override string ToString() => $"P_{Value}";

    public override bool Equals(object obj) => this.Equals(obj as ParseValueType);

    public override int GetHashCode() => Value.GetHashCode();

    public bool Equals(ParseValueType other) => !ReferenceEquals(null, other) && Value.Equals(other.Value);
}