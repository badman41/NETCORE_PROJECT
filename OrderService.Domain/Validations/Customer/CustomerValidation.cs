using System;
using FluentValidation;
using OrderService.Domain.Commands.Customer;
using OrderService.Domain.Interface.Respository;

namespace OrderService.Domain.Validations.Customer
{
    public abstract class CustomerValidation<T> : AbstractValidator<T> where T : CustomerCommand
    {
        protected void ValidateName()
        {
            RuleFor(c => c.Name)
                .NotNull().WithMessage("Name not null")
                .NotEmpty().WithMessage("Please ensure you have entered the Name")
                .Length(2, 150).WithMessage("The Name must have between 2 and 150 characters");
        }

        protected void ValidateExistId(ICustomerRepository CustomerRepository)
        {
            RuleFor(c => c.Id).Must(x => CustomerRepository.Get(x) != null).WithMessage("Customer not found");
        }

        protected void ValidateCode()
        {
            RuleFor(c => c.Code)
                .NotNull().WithMessage("Code not null")
                .NotEmpty().WithMessage("Please ensure you have entered the Name")
                .MaximumLength(50).WithMessage("The Code must contain maximum 50 characters");
        }
        protected void ValidateCartCode()
        {
            RuleFor(c => c.CartCode)
                .NotNull().WithMessage("Code not null")
                .NotEmpty().WithMessage("Please ensure you have entered the Name")
                .MaximumLength(50).WithMessage("The Code must contain maximum 50 characters");
        }
        protected void ValidateEmail()
        {
            RuleFor(c => c.Email).EmailAddress().WithMessage("Email not valid")
                .MaximumLength(250).WithMessage("The Email must contain maximum 250 characters");
        }
        protected void ValidatePhoneNumber()
        {
            RuleFor(c => c.PhoneNumber).Matches(@"^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\s\./0-9]*$")
                .WithMessage("Phone number not valid");
        }
    }
}
