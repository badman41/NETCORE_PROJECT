using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.Shared.ValueObject
{
    public class StartDate : IValueObject<StartDate>
    {
        public DateTime Value { get; }

        public StartDate(DateTime value)
        {
            Value = value;
        }

        public bool SameValueAs(StartDate other)
        {
            return other.Value == Value;
        }

        public static implicit operator DateTime(StartDate value)
        {
            return value.Value;
        }

        public static implicit operator StartDate(DateTime value)
        {
            return new StartDate(value);
        }
    }
}
