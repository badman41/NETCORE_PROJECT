using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.Shared.ValueObject
{
    public class Price : IValueObject<Price>
    {
        public int Value { get; }

        public Price(int value)
        {
            Value = value;
        }

        public bool SameValueAs(Price other)
        {
            return other.Value == Value;
        }

        public static implicit operator int(Price value)
        {
            return value.Value;
        }

        public static implicit operator Price(int value)
        {
            return new Price(value);
        }
    }
}
