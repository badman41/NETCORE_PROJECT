using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.Shared.ValueObject
{
    public interface IValueObject<T>
    {
        bool SameValueAs(T other);
    }
}
