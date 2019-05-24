using OrderService.Domain.Shared;
using OrderService.Domain.Shared.ValueObject;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.Entities
{
    public class Customer : IEntity<Customer>
    {
        public Identity Id { get; }
        public Name Name { get; }
        public Note Note { get; }
        public CartCode CartCode { get; }
        public PhoneNumber PhoneNumber { get; }
        public Email Email { get; }
        public Code Code { get; }
        public Status Status { get; }
        public Address Address { get; }

        public Customer(Identity id, Name name, Note note, CartCode cartCode, PhoneNumber phoneNumber, Email email, Code code, Status status, Address address)
        {
            Id = id;
            Name = name;
            Note = note;
            CartCode = cartCode;
            PhoneNumber = phoneNumber;
            Code = code;
            Status = status;
            Address = address;
            Email = email;
        }

        public bool sameIdentityAs(Customer other)
        {
            return Id.Equals(Id);
        }
    }
}
