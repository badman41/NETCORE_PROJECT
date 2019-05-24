using OrderService.Domain.Shared;
using OrderService.Domain.Shared.ValueObject;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.Entities
{
    public class Invoice : IEntity<Invoice>
    {
        public Identity Id { get; }
        public Customer Customer { get; }
        public Code Code { get; }
        public DeliveryTime DeliveryTime { get; }
        public Note Note { get; }
        public Weight WeightTotal { get; }
        public Price TotalPrice { get; }
        public IEnumerable<InvoiceItem> Items { get; }

        public Invoice(Identity id, Customer customer, DeliveryTime deliveryTime, Price totalPrice, Note note,
            Weight weightTotal, Code code, IEnumerable<InvoiceItem> items)
        {
            Id = id;
            Customer = customer;
            DeliveryTime = deliveryTime;
            Note = note;
            WeightTotal = weightTotal;
            TotalPrice = totalPrice;
            Items = items;
            Code = code;
        }
        public bool sameIdentityAs(Invoice other)
        {
            return Id.Equals(Id);
        }
    }
}
