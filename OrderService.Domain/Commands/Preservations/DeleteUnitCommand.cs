using MediatR;
using OrderService.Domain.Commands.Preservations;
using OrderService.Domain.Interface.Respository;
using OrderService.Domain.Validations;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.Commands.Preservation
{
    public class DeletePreservationCommand : PreservationCommand
    {
        public DeletePreservationCommand(int id)
        {
            Id = id;
        }

        public override bool IsValid( object PreservationRepository)
        {
            //ValidationResult = new DeletePreservationValidation(PreservationRepository as IPreservationRepository).Validate(this);
            //return ValidationResult.IsValid;
            return true;
        }
    }
}
