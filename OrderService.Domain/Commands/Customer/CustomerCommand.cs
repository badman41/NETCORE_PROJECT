using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.Commands.Customer
{
    public abstract class CustomerCommand: BaseCommand
    {
        public int Id { get; protected set; }
        public string Name { get; protected set; }
        public string AddressId { get; protected set; }
        public string Note { get; protected set; }
        public string CartCode { get; protected set; }
        public string PhoneNumber { get; protected set; }
        public string Code { get; protected set; }
        public int Status { get; protected set; }
        public string Email { get; protected set; }
        public int CustomerGroupId { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
        public DateTime UpdatedAt { get; protected set; }

        //Address
        public string City { get; set; }
        public string Country { get; set; }
        public string District { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
        public string Street { get; set; }
        public string StreetNumber { get; set; } 

    }
}
