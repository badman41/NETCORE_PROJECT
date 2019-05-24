using OrderService.Domain.ReadModels;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OrderService.Application.Request
{
    public class AddNewProductRequestRequest
    {
        public string description { get; set; }
        public string product_name { get; set; }
        public int quantity { get; set; }
        public int userId { get; set; }
    }
}
