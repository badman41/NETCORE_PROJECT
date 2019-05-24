using MediatR;
using OrderService.Domain.Validations.CustomerGroup;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.Commands.CustomerGroup
{
    public class AddNewCustomerGroupCommand : CustomerGroupCommand
    {
        public AddNewCustomerGroupCommand(string name)
        {
            Name = name;
        }

        public override bool IsValid(object repository = null)
        {
            ValidationResult = new AddNewCustomerGroupCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
