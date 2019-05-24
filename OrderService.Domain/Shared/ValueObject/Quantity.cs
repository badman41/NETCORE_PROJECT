using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.Shared.ValueObject
{
    public class Quantity : IValueObject<Quantity>
    {
        public int Value { get; }

        public Quantity(int value)
        {
            Value = value;
        }

        public bool SameValueAs(Quantity other)
        {
            return other.Value == Value;
        }

        public static implicit operator int(Quantity value)
        {
            return value.Value;
        }

        public static implicit operator Quantity(int value)
        {
            return new Quantity(value);
        }
    }
}
