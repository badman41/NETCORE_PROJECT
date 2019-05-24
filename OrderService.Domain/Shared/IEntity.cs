using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.Shared
{
    interface IEntity<T>
    {
        bool sameIdentityAs(T other);
    }
}
