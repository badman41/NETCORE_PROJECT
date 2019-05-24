using OrderService.Domain.ReadModels;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OrderService.Application.Request
{
    public class AddNewCustomerRequest
    {
        public string Name { get; set; }
        public string Note { get; set; }
        public string CartCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Code { get; set; }
        public int Status { get; set; }
        public string Email { get; set; }

        public AddressModel Address { get; set; }
        //public string City { get; set; }
        //public string Country { get; set; }
        //public string District { get; set; }
        //public float Lat { get; set; }
        //public float Lng { get; set; }
        //public string Street { get; set; }
        //public string StreetNumber { get; set; }
    }
}
