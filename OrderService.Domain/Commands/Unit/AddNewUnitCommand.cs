using MediatR;
using OrderService.Domain.Validations.Unit;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.Commands.Unit
{
    public class AddNewUnitCommand : UnitCommand
    {
        public AddNewUnitCommand(string name)
        {
            Name = name;
        }

        public override bool IsValid(object repository = null)
        {
            ValidationResult = new AddNewUnitCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
