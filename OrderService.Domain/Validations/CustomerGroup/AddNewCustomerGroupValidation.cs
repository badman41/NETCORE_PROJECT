using OrderService.Domain.Commands.CustomerGroup;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.Validations.CustomerGroup
{
    public class AddNewCustomerGroupCommandValidation : CustomerGroupValidation<AddNewCustomerGroupCommand>
    {
        public AddNewCustomerGroupCommandValidation()
        {
            ValidateName();
        }
    }
}
