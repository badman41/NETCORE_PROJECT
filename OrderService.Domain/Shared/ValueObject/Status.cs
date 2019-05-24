using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.Shared.ValueObject
{
    public class Status : IValueObject<Status>
    {
        public int Value { get; }

        public Status(int value)
        {
            Value = value;
        }

        public bool SameValueAs(Status other)
        {
            return other.Value == Value;
        }

        public static implicit operator int(Status value)
        {
            return value.Value;
        }

        public static implicit operator Status(int value)
        {
            return new Status(value);
        }
    }
}
