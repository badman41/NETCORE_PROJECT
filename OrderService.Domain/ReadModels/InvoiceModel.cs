using OrderService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.ReadModels
{
    public class InvoiceModel
    {
        public int ID { get; set; }
        public AddressModel Address { get; set; }
        public string Note { get; set; }
        public string CustomerCode { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public DateTime DeliveryTime { get; set; }
        public int Status { get; set; }
        public int AddressId { get; set; }
        public decimal WeightTotal { get; set; }
        public int TotalPrice { get; set; }
        public string Code { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public IEnumerable<InvoiceItemModel> Items { get; set; }
        public InvoiceModel() { }
    }
}
