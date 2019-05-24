using OrderService.Domain.ReadModels;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OrderService.Application.Request
{
    public class UpdateCustomerRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public string CartCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Code { get; set; }
        public int Status { get; set; }
        public string Email { get; set; }

        public AddressModel Address { get; set; }
    }
}
