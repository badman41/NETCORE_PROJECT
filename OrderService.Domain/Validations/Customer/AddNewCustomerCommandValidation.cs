using OrderService.Domain.Commands.Customer;
using OrderService.Domain.Commands.CustomerGroup;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.Validations.Customer
{
    public class AddNewProductCommandValidation : CustomerValidation<AddNewCustomerCommand>
    {
        public AddNewProductCommandValidation()
        {
            ValidateName();
            ValidateCode();
            ValidateEmail();
            ValidateCartCode();
            ValidatePhoneNumber();
        }
    }
}
