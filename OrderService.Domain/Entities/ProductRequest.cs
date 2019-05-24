using OrderService.Domain.Shared;
using OrderService.Domain.Shared.ValueObject;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.Entities
{
    public class ProductRequest : IEntity<ProductRequest>
    {
        public Identity Id { get; }
        public Name ProductName { get; }
        public Description Description { get; }
        public Quantity Quantity { get; }
        public Status Status { get; }
        public Customer Customer { get; }
        public Response Response { get; }
        public ProductRequest(Identity id, Name productName, Description description, Quantity quantity
            , Status status, Response response, Customer customer)
        {
            Id = id;
            ProductName = productName;
            Description = description;
            Quantity = quantity;
            Status = status;
            Response = response;
            Customer = customer;
        }
        public bool sameIdentityAs(ProductRequest other)
        {
            return Id.Equals(Id);
        }
    }
}
