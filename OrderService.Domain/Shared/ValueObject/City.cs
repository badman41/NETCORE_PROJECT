using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.Shared.ValueObject
{
    public class City : IValueObject<City>
    {
        public string Value { get; }

        public City(string value)
        {
            Value = value;
        }

        public bool SameValueAs(City other)
        {
            return other.Value == Value;
        }

        public static implicit operator string(City value)
        {
            return value.Value;
        }

        public static implicit operator City(string value)
        {
            return new City(value);
        }
    }
}
