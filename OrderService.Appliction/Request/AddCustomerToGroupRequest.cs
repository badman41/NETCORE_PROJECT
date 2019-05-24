using OrderService.Domain.ReadModels;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OrderService.Application.Request
{
    public class AddCustomerToGroupRequest
    {
        public int customer_id { get; set; }
        public int customer_group_id { get; set; }
    }
}
