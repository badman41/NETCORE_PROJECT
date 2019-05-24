using OrderService.Domain.ReadModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OrderService.Application.Request
{
    public class AddNewInvoiceRequest
    {
        public int ID { get; set; }
        public AddressModel Address { get; set; }
        public string Note { get; set; }
        public string CustomerCode { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public double DeliveryTime { get; set; }
        public int Status { get; set; }
        public decimal WeightTotal { get; set; }
        public int TotalPrice { get; set; }

        public IEnumerable<InvoiceItemModel> Items { get; set; }
    }
}
