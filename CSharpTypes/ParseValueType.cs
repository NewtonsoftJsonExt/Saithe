using System.ComponentModel;

namespace CSharpTypes;

[TypeConverter(typeof(Saithe.ParseTypeConverter<ParseValueType>)),
 Newtonsoft.Json.JsonConverter(typeof(Saithe.NewtonsoftJson.ParseTypeJsonConverter<ParseValueType>)),
 System.Text.Json.Serialization.JsonConverter(typeof(Saithe.SystemTextJson.ParseTypeJsonConverter<ParseValueType>))]
public record ParseValueType (string Value)
{
    public static ParseValueType Parse(string value)
    {
        var res = value.Split('_');
        if (res.Length == 2 && res[0] == "P") return new ParseValueType(res[1]);
        throw new ParseValueException($"Expected value to be in form: P_* but was '{value}'");
    }

    public override string ToString() => $"P_{Value}";
}