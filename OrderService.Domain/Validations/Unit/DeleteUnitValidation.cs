using OrderService.Domain.Commands.Unit;
using OrderService.Domain.Interface.Respository;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.Validations.Unit
{
    public class DeleteUnitValidation : UnitValidation<DeleteUnitCommand>
    {
        public DeleteUnitValidation(IUnitRepository unitRepository)
        {
            ValidateExistId(unitRepository);
        }
    }
}
