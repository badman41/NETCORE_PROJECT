using OrderService.Domain.ReadModels;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OrderService.Application.Request
{
    public class AddNewCustomerGroupRequest
    {
        public string Name { get; set; }
    }
}
