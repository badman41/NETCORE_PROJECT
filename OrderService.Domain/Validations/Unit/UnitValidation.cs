using System;
using FluentValidation;
using OrderService.Domain.Commands.Unit;
using OrderService.Domain.Interface.Respository;

namespace OrderService.Domain.Validations.Unit
{
    public abstract class UnitValidation<T> : AbstractValidator<T> where T : UnitCommand
    {
        protected void ValidateName()
        {
            RuleFor(c => c.Name)
                .NotNull().WithMessage("Name not null")
                .NotEmpty().WithMessage("Please ensure you have entered the Name")
                .Length(2, 150).WithMessage("The Name must have between 2 and 150 characters");
        }

        protected void ValidateExistId(IUnitRepository unitRepository)
        {
            RuleFor(c => c.Id).Must(x => unitRepository.Get(x) != null).WithMessage("Unit not found");
        }
    }
}
