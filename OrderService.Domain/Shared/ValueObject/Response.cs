using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.Shared.ValueObject
{
    public class Response : IValueObject<Response>
    {
        public string Value { get; }

        public Response(string value)
        {
            Value = value;
        }

        public bool SameValueAs(Response other)
        {
            return other.Value == Value;
        }

        public static implicit operator string(Response value)
        {
            return value.Value;
        }

        public static implicit operator Response(string value)
        {
            return new Response(value);
        }
    }
}
