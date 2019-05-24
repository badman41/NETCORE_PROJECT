using OrderService.Domain.Commands.CustomerGroup;
using OrderService.Domain.Interface.Respository;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.Validations.CustomerGroup
{
    public class DeleteCustomerGroupValidation : CustomerGroupValidation<DeleteCustomerGroupCommand>
    {
        public DeleteCustomerGroupValidation(ICustomerGroupRepository CustomerGroupRepository)
        {
            ValidateExistId(CustomerGroupRepository);
        }
    }
}
