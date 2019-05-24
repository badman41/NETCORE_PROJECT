using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.Shared.ValueObject
{
    public class Deliveried : IValueObject<Deliveried>
    {
        public bool Value { get; }

        public Deliveried(bool value)
        {
            Value = value;
        }

        public bool SameValueAs(Deliveried other)
        {
            return other.Value == Value;
        }

        public static implicit operator bool(Deliveried value)
        {
            return value.Value;
        }

        public static implicit operator Deliveried(bool value)
        {
            return new Deliveried(value);
        }
    }
}
