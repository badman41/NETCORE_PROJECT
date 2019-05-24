using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.ReadModels
{
    public class QuotationModel
    {
        public int ID { get; set; }
        public int CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public DateTime Date { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedAt { get; set; }
        public IEnumerable<ProductPriceModel> ProductPrices { get; set; }
    }
}
