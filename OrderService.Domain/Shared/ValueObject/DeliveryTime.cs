using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.Shared.ValueObject
{
    public class DeliveryTime : IValueObject<DeliveryTime>
    {
        public DateTime Value { get; }

        public DeliveryTime(DateTime value)
        {
            Value = value;
        }

        public bool SameValueAs(DeliveryTime other)
        {
            return other.Value == Value;
        }

        public static implicit operator DateTime(DeliveryTime value)
        {
            return value.Value;
        }

        public static implicit operator DeliveryTime(DateTime value)
        {
            return new DeliveryTime(value);
        }
    }
}
