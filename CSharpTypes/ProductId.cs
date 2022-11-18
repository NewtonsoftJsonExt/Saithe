using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Saithe;

namespace CSharpTypes
{
    [TypeConverter(typeof(ParseTypeConverter<ProductId>))]
    public struct ProductId: IEquatable<ProductId>, IParsable<ProductId>
    {
        public readonly long Value;

        public ProductId(long value)
        {
            this.Value = value;
        }

        public readonly static ProductId Empty = new ProductId();

        public bool Equals(ProductId other)
        {
            if (ReferenceEquals(null, other)) return false;
            return Equals(Value, other.Value);
        }
        public override bool Equals(object obj)
        {
            if (obj is ProductId)
                return Equals((ProductId)obj);
            return false;
        }
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
        public override string ToString()
        {
            return $"ProductId/{Value}";
        }
        private static bool TryParse(string str, out ProductId result) 
        {
            result = Empty;
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }
            var split = str.Split('/');
            long res;
            if (split.Length == 2 
                && split[0] == "ProductId"
                && long.TryParse(split[1], out res))
            {
                result = new ProductId(res);
                return true;
            }
            return false;
        }
        private static ProductId Parse(string str) 
        {
            ProductId res;
            if (TryParse(str, out res))
                return res;
            throw new Exception("Could not parse product id");
        }

        public static ProductId Parse(string s, IFormatProvider provider) => Parse(s);

        public static bool TryParse([NotNullWhen(true)] string s, IFormatProvider provider, [MaybeNullWhen(false)] out ProductId result) => TryParse(s, out result);
    }
}
