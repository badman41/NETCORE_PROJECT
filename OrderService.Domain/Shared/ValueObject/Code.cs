using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.Shared.ValueObject
{
    public class Code : IValueObject<Code>
    {
        public string Value { get; }

        public Code(string value)
        {
            Value = value;
        }

        public bool SameValueAs(Code other)
        {
            return other.Value == Value;
        }

        public static implicit operator string(Code value)
        {
            return value.Value;
        }

        public static implicit operator Code(string value)
        {
            return new Code(value);
        }
    }
}
