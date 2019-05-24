using OrderService.Domain.ReadModels;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OrderService.Application.Request
{
    public class DeleteCustomerGroupRequest
    {
        public int Id { get; set; }
        public DeleteCustomerGroupRequest(int id)
        {
            Id = id;
        }
    }
}
