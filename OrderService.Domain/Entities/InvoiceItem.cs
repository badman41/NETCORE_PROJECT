using OrderService.Domain.Shared;
using OrderService.Domain.Shared.ValueObject;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.Entities
{
    public class InvoiceItem : IEntity<InvoiceItem>
    {
        public Identity Id { get; }
        public Product Product { get; }
        public Unit Unit { get; }
        public Price Price { get; }
        public Note Note { get; }
        public Quantity Quantity { get; }
        public Weight Weight { get; }
        public Price TotalPrice { get; }
        public Deliveried Deliveried { get; }
        public Quantity DeliveriedQuantity { get; }

        public InvoiceItem(Identity id, Product product, Unit unit, Price price, Note note, 
            Quantity quantity, Weight weight, Price totalPrice, Deliveried deliveried, Quantity deliveriedQuantity)
        {
            Id = id;
            Product = product;
            Unit = unit;
            Price = price;
            Note = note;
            Quantity = quantity;
            Weight = weight;
            TotalPrice = totalPrice;
            Deliveried = deliveried;
            DeliveriedQuantity = deliveriedQuantity;
        }
        public bool sameIdentityAs(InvoiceItem other)
        {
            return Id.Equals(Id);
        }
    }
}
