using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.Shared.ValueObject
{
    public class Email : IValueObject<Email>
    {
        public string Value { get; }

        public Email(string value)
        {
            Value = value;
        }

        public bool SameValueAs(Email other)
        {
            return other.Value == Value;
        }

        public static implicit operator string(Email value)
        {
            return value.Value;
        }

        public static implicit operator Email(string value)
        {
            return new Email(value);
        }
    }
}
