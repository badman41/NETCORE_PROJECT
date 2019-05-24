using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.Shared.ValueObject
{
    public class Weight : IValueObject<Weight>
    {
        public decimal Value { get; }

        public Weight(decimal value)
        {
            Value = value;
        }

        public bool SameValueAs(Weight other)
        {
            return other.Value == Value;
        }

        public static implicit operator decimal(Weight value)
        {
            return value.Value;
        }

        public static implicit operator Weight(decimal value)
        {
            return new Weight(value);
        }
    }
}
