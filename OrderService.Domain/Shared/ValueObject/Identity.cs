using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.Shared.ValueObject
{
    public class Identity : IValueObject<Identity>
    {
        public uint Value { get; }

        public Identity(uint value)
        {
            Value = value;
        }

        public bool SameValueAs(Identity other)
        {
            return other.Value == Value;
        }

        public static implicit operator uint(Identity value)
        {
            return value.Value;
        }

        public static implicit operator Identity(uint value)
        {
            return new Identity(value);
        }
    }
}
