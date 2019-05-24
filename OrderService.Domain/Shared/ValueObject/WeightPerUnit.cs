using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.Shared.ValueObject
{
    public class WeightPerUnit : IValueObject<WeightPerUnit>
    {
        public decimal Value { get; }

        public WeightPerUnit(decimal value)
        {
            Value = value;
        }

        public bool SameValueAs(WeightPerUnit other)
        {
            return other.Value == Value;
        }

        public static implicit operator decimal(WeightPerUnit value)
        {
            return value.Value;
        }

        public static implicit operator WeightPerUnit(decimal value)
        {
            return new WeightPerUnit(value);
        }
    }
}
