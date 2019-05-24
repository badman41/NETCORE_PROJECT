using System;
using FluentValidation;
using OrderService.Domain.Commands.Product;
using OrderService.Domain.Interface.Respository;

namespace OrderService.Domain.Validations.Product
{
    public abstract class ProductValidation<T> : AbstractValidator<T> where T : ProductCommand
    {
        protected void ValidateName()
        {
            RuleFor(c => c.Name)
                .NotNull().WithMessage("Name not null")
                .NotEmpty().WithMessage("Please ensure you have entered the Name")
                .Length(2, 150).WithMessage("The Name must have between 2 and 150 characters");
        }

        protected void ValidateExistId(IProductRepository ProductRepository)
        {
            RuleFor(c => c.Id).Must(x => ProductRepository.Get(x) != null).WithMessage("Product not found");
        }

        protected void ValidateCode()
        {
            RuleFor(c => c.Code)
                .NotNull().WithMessage("Code not null")
                .NotEmpty().WithMessage("Please ensure you have entered the Name")
                .MaximumLength(50).WithMessage("The Code must contain maximum 50 characters");
        }
    }
}
