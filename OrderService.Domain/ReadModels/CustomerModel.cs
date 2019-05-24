using OrderService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.ReadModels
{
    public class CustomerModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public string CartCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Code { get; set; }
        public int Status { get; set; }
        public int AddressId { get; set; }
        public AddressModel Address { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public CustomerModel() { }
        public CustomerModel(int id, string name, string note, string cartCode, string phoneNumber, string email
                            , string code, int status, int addressId, DateTime createdAt, DateTime updatedAt)
        {
            ID = id;
            Name = name;
            Note = note;
            CartCode = cartCode;
            PhoneNumber = phoneNumber;
            Email = email;
            Code = code;
            Status = status;
            AddressId = addressId;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }
    }
}
