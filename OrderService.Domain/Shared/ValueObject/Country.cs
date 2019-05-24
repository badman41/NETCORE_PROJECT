using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.Shared.ValueObject
{
    public class Country : IValueObject<Country>
    {
        public string Value { get; }

        public Country(string value)
        {
            Value = value;
        }

        public bool SameValueAs(Country other)
        {
            return other.Value == Value;
        }

        public static implicit operator string(Country value)
        {
            return value.Value;
        }

        public static implicit operator Country(string value)
        {
            return new Country(value);
        }
    }
}
