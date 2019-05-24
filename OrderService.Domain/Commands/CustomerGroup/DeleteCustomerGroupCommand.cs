using MediatR;
using OrderService.Domain.Interface.Respository;
using OrderService.Domain.Validations.CustomerGroup;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.Commands.CustomerGroup
{
    public class DeleteCustomerGroupCommand : CustomerGroupCommand
    {
        public DeleteCustomerGroupCommand(int id)
        {
            Id = id;
        }

        public override bool IsValid( object CustomerGroupRepository)
        {
            ValidationResult = new DeleteCustomerGroupValidation(CustomerGroupRepository as ICustomerGroupRepository).Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
