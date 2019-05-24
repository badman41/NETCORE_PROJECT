using OrderService.Domain.Shared;
using OrderService.Domain.Shared.ValueObject;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.Entities
{
    public class Unit : IEntity<Unit>
    {
        public Identity Id { get; }
        public Name Name { get; }

        public Unit(Identity id, Name name)
        {
            Id = id;
            Name = name;
        }

        public bool sameIdentityAs(Unit other)
        {
            return Id.Equals(Id);
        }
    }
}
