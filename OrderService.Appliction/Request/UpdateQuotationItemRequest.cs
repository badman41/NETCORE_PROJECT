using OrderService.Domain.ReadModels;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OrderService.Application.Request
{
    public class UpdateQuotationItemRequest
    {
        public int QuotationID { get; set; }
        public int Price { get; set; }
        public string ProductCode { get; set; }
        public int UnitID { get; set; }
    }
}
