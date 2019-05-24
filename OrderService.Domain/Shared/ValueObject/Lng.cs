using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.Shared.ValueObject
{
    public class Lng : IValueObject<Lng>
    {
        public string Value { get; }

        public Lng(string value)
        {
            Value = value;
        }

        public bool SameValueAs(Lng other)
        {
            return other.Value == Value;
        }

        public static implicit operator string(Lng value)
        {
            return value.Value;
        }

        public static implicit operator Lng(string value)
        {
            return new Lng(value);
        }
    }
}
