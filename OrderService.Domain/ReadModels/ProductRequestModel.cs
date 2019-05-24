using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.ReadModels
{
    public class ProductRequestModel
    {
        public int ID { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public int Status { get; set; }
        public string Response { get; set; }
        public int CustomerId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CustomerName { get; set; }
    }
}
