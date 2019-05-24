using MediatR;
using OrderService.Domain.Commands.Preservations;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.Commands.Preservation
{
    public class AddNewPreservationCommand : PreservationCommand
    {
        public AddNewPreservationCommand(string description)
        {
            Description = description;
        }

        public override bool IsValid(object repository = null)
        {
            //ValidationResult = new AddNewPreservationCommandValidation().Validate(this);
            //return ValidationResult.IsValid;
            return true;
        }
    }
}
