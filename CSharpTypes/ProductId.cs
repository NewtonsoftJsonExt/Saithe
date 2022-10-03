﻿using System;
using System.ComponentModel;

namespace CSharpTypes;

[TypeConverter(typeof(Saithe.ParseTypeConverter<ProductId>)),
 Newtonsoft.Json.JsonConverter(typeof(Saithe.NewtonsoftJson.ParseTypeJsonConverter<ProductId>)),
 System.Text.Json.Serialization.JsonConverter(typeof(Saithe.SystemTextJson.ParseTypeJsonConverter<ProductId>))]
public struct ProductId: IEquatable<ProductId>
{
    public readonly long Value;

    public ProductId(long value) => this.Value = value;

    public readonly static ProductId Empty = new ProductId();

    public bool Equals(ProductId other) => Equals(Value, other.Value);

    public override bool Equals(object obj) => obj is ProductId id && Equals(id);

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => $"ProductId/{Value}";

    public static bool TryParse(string str, out ProductId result) 
    {
        result = Empty;
        if (string.IsNullOrEmpty(str))
        {
            return false;
        }
        var split = str.Split('/');
        if (split.Length == 2 
            && split[0] == "ProductId"
            && long.TryParse(split[1], out var res))
        {
            result = new ProductId(res);
            return true;
        }
        return false;
    }
    public static ProductId Parse(string str) => TryParse(str, out var res) ? res : throw new Exception("Could not parse product id");
}