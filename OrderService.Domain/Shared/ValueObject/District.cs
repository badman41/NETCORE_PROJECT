using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.Shared.ValueObject
{
    public class District : IValueObject<District>
    {
        public string Value { get; }

        public District(string value)
        {
            Value = value;
        }

        public bool SameValueAs(District other)
        {
            return other.Value == Value;
        }

        public static implicit operator string(District value)
        {
            return value.Value;
        }

        public static implicit operator District(string value)
        {
            return new District(value);
        }
    }
}
