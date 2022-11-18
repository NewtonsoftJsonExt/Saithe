﻿using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Saithe;

namespace CSharpTypes
{
    [TypeConverter(typeof(ParseTypeConverter<ParseValueType>))]
    public class ParseValueType : IEquatable<ParseValueType>, IParsable<ParseValueType>
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

        public static ParseValueType Parse(string s, IFormatProvider provider) => Parse(s);

        public static bool TryParse([NotNullWhen(true)] string s, IFormatProvider provider, [MaybeNullWhen(false)] out ParseValueType result)
        {
            try{
                result = Parse(s);
                return true;
            }catch(Exception){
                result = default;
                return false;
            }
        }
    }
}
