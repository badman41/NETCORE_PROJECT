using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.ReadModels
{
    public class InvoiceItemModel
    {
        public int ID { get; set; }
        public bool Deliveried { get; set; }
        public int DeliveriedQuantity { get; set; }
        public string Note { get; set; }
        public int Price { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public int TotalPrice { get; set; }
        public int UnitID { get; set; }
        public string UnitName { get; set; }
        public decimal Weight { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
