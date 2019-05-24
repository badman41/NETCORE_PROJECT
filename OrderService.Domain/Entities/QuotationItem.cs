using OrderService.Domain.Shared;
using OrderService.Domain.Shared.ValueObject;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.Entities
{
    public class QuotationItem : IEntity<QuotationItem>
    {
        public Identity Id { get; }
        public Quotation Quotation { get; }
        public Product Product { get; }
        public Unit Unit { get; }
        public Price Price { get; }

        public QuotationItem(Identity id, Quotation quotation, Product product, Unit unit, Price price)
        {
            Id = id;
            Quotation = quotation;
            Product = product;
            Unit = unit;
            Price = price;
        }
        public bool sameIdentityAs(QuotationItem other)
        {
            return Id.Equals(Id);
        }
    }
}
