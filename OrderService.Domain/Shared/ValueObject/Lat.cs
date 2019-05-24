using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.Shared.ValueObject
{
    public class Lat : IValueObject<Lat>
    {
        public string Value { get; }

        public Lat(string value)
        {
            Value = value;
        }

        public bool SameValueAs(Lat other)
        {
            return other.Value == Value;
        }

        public static implicit operator string(Lat value)
        {
            return value.Value;
        }

        public static implicit operator Lat(string value)
        {
            return new Lat(value);
        }
    }
}
