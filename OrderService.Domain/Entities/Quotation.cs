using OrderService.Domain.Shared;
using OrderService.Domain.Shared.ValueObject;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.Entities
{
    public class Quotation : IEntity<Quotation>
    {
        public Identity Id { get; }
        public Customer Customer { get; }
        public StartDate StartDate { get; }
        public EndDate EndDate { get; }

        public Quotation(Identity id, Customer customer, StartDate startDate, EndDate endDate)
        {
            Id = id;
            Customer = customer;
            StartDate = startDate;
            EndDate = endDate;
        }
        public bool sameIdentityAs(Quotation other)
        {
            return Id.Equals(Id);
        }
    }
}
