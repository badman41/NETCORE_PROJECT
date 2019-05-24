using MediatR;
using OrderService.Domain.Interface.Respository;
using OrderService.Domain.Validations.Unit;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.Commands.Unit
{
    public class DeleteUnitCommand : UnitCommand
    {
        public DeleteUnitCommand(int id)
        {
            Id = id;
        }

        public override bool IsValid( object unitRepository)
        {
            ValidationResult = new DeleteUnitValidation(unitRepository as IUnitRepository).Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
