using System;
using FluentValidation;
using OrderService.Domain.Commands.CustomerGroup;
using OrderService.Domain.Interface.Respository;

namespace OrderService.Domain.Validations.CustomerGroup
{
    public abstract class CustomerGroupValidation<T> : AbstractValidator<T> where T : CustomerGroupCommand
    {
        protected void ValidateName()
        {
            RuleFor(c => c.Name)
                .NotNull().WithMessage("Name not null")
                .NotEmpty().WithMessage("Please ensure you have entered the Name")
                .Length(2, 150).WithMessage("The Name must have between 2 and 150 characters");
        }

        protected void ValidateExistId(ICustomerGroupRepository CustomerGroupRepository)
        {
            RuleFor(c => c.Id).Must(x => CustomerGroupRepository.Get(x) != null).WithMessage("CustomerGroup not found");
        }
    }
}
