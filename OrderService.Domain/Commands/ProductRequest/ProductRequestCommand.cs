using OrderService.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.Commands.ProductRequest
{
    public abstract class ProductRequestCommand: BaseCommand
    {
        public int Id { get; protected set; }
        public string Description { get; protected set; }
        public string ProductName { get; protected set; }
        public string Response { get; protected set; }
        public int Status { get; protected set; }
        public int Quantity { get; protected set; }
        public int UserId { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
    }
}
