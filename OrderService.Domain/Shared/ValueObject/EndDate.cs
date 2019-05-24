using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.Shared.ValueObject
{
    public class EndDate : IValueObject<EndDate>
    {
        public DateTime Value { get; }

        public EndDate(DateTime value)
        {
            Value = value;
        }

        public bool SameValueAs(EndDate other)
        {
            return other.Value == Value;
        }

        public static implicit operator DateTime(EndDate value)
        {
            return value.Value;
        }

        public static implicit operator EndDate(DateTime value)
        {
            return new EndDate(value);
        }
    }
}
