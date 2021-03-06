﻿using System;
using System.ComponentModel;
using Saithe;

namespace CSharpTypes
{
    [TypeConverter(typeof(ParseTypeConverter<ParseValueType>))]
    public class ParseValueType : IEquatable<ParseValueType>
    {
        public readonly string Value;

        public ParseValueType(string value)
        {
            Value = value;
        }

        public static ParseValueType Parse(string value)
        {
            var res = value.Split('_');
            if (res.Length == 2 && res[0] == "P") return new ParseValueType(res[1]);
            throw new ParseValueException($"Expected value to be in form: P_* but was '{value}'");
        }

        public override string ToString() => $"P_{Value}";

        public override bool Equals(object obj)
        {
            return Value.Equals(obj as ParseValueType);
        }
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public bool Equals(ParseValueType other)
        {
            if (ReferenceEquals(null, other)) return false;
            return Value.Equals(other.Value);
        }
    }
}
