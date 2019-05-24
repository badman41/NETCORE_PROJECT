using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.Shared.ValueObject
{
    public class Name : IValueObject<Name>
    {
        public string Value { get; }

        public bool SameValueAs(Name other)
        {
            return Value == other.Value;
        }

        public Name(string value)
        {
            Value = value;
        }
    }
}
