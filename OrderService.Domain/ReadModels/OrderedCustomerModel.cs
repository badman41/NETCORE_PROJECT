using OrderService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.ReadModels
{
    public class OrderedCustomerModel
    {
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public bool Status { get; set; }
        public IEnumerable<int> InvoiceIDs { get; set; }
        
        public OrderedCustomerModel() { }
    }
}
