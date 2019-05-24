using System;
using FluentValidation;
using OrderService.Domain.Commands.Quotation;
using OrderService.Domain.Interface.Respository;

namespace OrderService.Domain.Validations.Product
{
    public abstract class QuotationItemValidation<T> : AbstractValidator<T> where T : QuotationItemCommand
    {
        protected void ValidateUnitId()
        {
            RuleFor(c => c.UnitId)
                .NotNull().WithMessage("UnitId not null");
        }
        protected void ValidateProductCode()
        {
            RuleFor(c => c.ProductCode)
                .NotNull().WithMessage("ProductCode not null")
                .NotEmpty().WithMessage("ProductCode not empty");
        }
        //protected void ValidateExistId(IProductRepository ProductRepository)
        //{
        //    RuleFor(c => c.Id).Must(x => ProductRepository.Get(x) != null).WithMessage("Product not found");
        //}

        protected void ValidatePrice()
        {
            RuleFor(c => c.Price)
                .NotNull().WithMessage("Price not null")
                .GreaterThan(0).WithMessage("Price must greater than 0");
        }
    }
}
