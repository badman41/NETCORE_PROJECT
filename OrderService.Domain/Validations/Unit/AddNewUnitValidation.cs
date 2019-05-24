using OrderService.Domain.Commands.Unit;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.Validations.Unit
{
    public class AddNewUnitCommandValidation : UnitValidation<AddNewUnitCommand>
    {
        public AddNewUnitCommandValidation()
        {
            ValidateName();
        }
    }
}
