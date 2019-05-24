using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.Shared.ValueObject
{
    public class CartCode : IValueObject<CartCode>
    {
        public string Value { get; }

        public CartCode(string value)
        {
            Value = value;
        }

        public bool SameValueAs(CartCode other)
        {
            return other.Value == Value;
        }

        public static implicit operator string(CartCode value)
        {
            return value.Value;
        }

        public static implicit operator CartCode(string value)
        {
            return new CartCode(value);
        }
    }
}
