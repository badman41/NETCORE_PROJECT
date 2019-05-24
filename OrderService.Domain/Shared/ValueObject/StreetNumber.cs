using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.Shared.ValueObject
{
    public class StreetNumber : IValueObject<StreetNumber>
    {
        public string Value { get; }

        public StreetNumber(string value)
        {
            Value = value;
        }

        public bool SameValueAs(StreetNumber other)
        {
            return other.Value == Value;
        }

        public static implicit operator string(StreetNumber value)
        {
            return value.Value;
        }

        public static implicit operator StreetNumber(string value)
        {
            return new StreetNumber(value);
        }
    }
}
