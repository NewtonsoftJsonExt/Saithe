using Newtonsoft.Json;
using Saithe;
using System;
using System.ComponentModel;

namespace CSharpTypes
{
    [TypeConverter(typeof(ValueTypeConverter<CustomerId>))]
    [JsonConverter(typeof(ValueTypeJsonConverter<CustomerId>))]
    public struct CustomerId : IEquatable<CustomerId>
    {
        public readonly int Value;

        public CustomerId(int value)
        {
            this.Value = value;
        }

        public readonly static CustomerId Empty = new CustomerId();

        public bool Equals(CustomerId other)
        {
            if (ReferenceEquals(null, other)) return false;
            return Equals(Value, other.Value);
        }
        public override bool Equals(object obj)
        {
            if (obj is CustomerId)
                return Equals((CustomerId)obj);
            return false;
        }
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
