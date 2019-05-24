using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.Shared.ValueObject
{
    public class PhoneNumber : IValueObject<PhoneNumber>
    {
        public string Value { get; }

        public PhoneNumber(string value)
        {
            Value = value;
        }

        public bool SameValueAs(PhoneNumber other)
        {
            return other.Value == Value;
        }

        public static implicit operator string(PhoneNumber value)
        {
            return value.Value;
        }

        public static implicit operator PhoneNumber(string value)
        {
            return new PhoneNumber(value);
        }
    }
}
