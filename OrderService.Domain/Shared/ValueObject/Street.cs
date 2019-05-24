using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.Shared.ValueObject
{
    public class Street : IValueObject<Street>
    {
        public string Value { get; }

        public Street(string value)
        {
            Value = value;
        }

        public bool SameValueAs(Street other)
        {
            return other.Value == Value;
        }

        public static implicit operator string(Street value)
        {
            return value.Value;
        }

        public static implicit operator Street(string value)
        {
            return new Street(value);
        }
    }
}
