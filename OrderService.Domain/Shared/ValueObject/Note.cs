using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.Shared.ValueObject
{
    public class Note : IValueObject<Note>
    {
        public string Value { get; }

        public Note(string value)
        {
            Value = value;
        }

        public bool SameValueAs(Note other)
        {
            return other.Value == Value;
        }

        public static implicit operator string(Note value)
        {
            return value.Value;
        }

        public static implicit operator Note(string value)
        {
            return new Note(value);
        }
    }
}
