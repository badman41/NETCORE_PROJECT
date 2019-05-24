using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.Shared.ValueObject
{
    public class Description : IValueObject<Description>
    {
        public string Value { get; }

        public Description(string value)
        {
            Value = value;
        }

        public bool SameValueAs(Description other)
        {
            return other.Value == Value;
        }

        public static implicit operator string(Description value)
        {
            return value.Value;
        }

        public static implicit operator Description(string value)
        {
            return new Description(value);
        }
    }
}
