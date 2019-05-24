using OrderService.Domain.Shared;
using OrderService.Domain.Shared.ValueObject;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.Entities
{
    public class Preservation : IEntity<Preservation>
    {
        public Identity Id { get; }
        public Description Description { get; }

        public Preservation(Identity id, Description description)
        {
            Id = id;
            Description = description;
        }

        public bool sameIdentityAs(Preservation other)
        {
            return Id.Equals(Id);
        }
    }
}
