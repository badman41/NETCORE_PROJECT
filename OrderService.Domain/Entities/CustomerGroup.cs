using OrderService.Domain.Shared;
using OrderService.Domain.Shared.ValueObject;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.Entities
{
    public class CustomerGroup : IEntity<CustomerGroup>
    {
        public Identity Id { get; }
        public Name Name { get; }

        public CustomerGroup(Identity id, Name name)
        {
            Id = id;
            Name = name;
        }

        public bool sameIdentityAs(CustomerGroup other)
        {
            return Id.Equals(Id);
        }
    }
}
