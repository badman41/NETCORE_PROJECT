using System;

namespace OrderService.Domain.Commands.Quotation
{
    public class AddNewQuotationCommand : QuotationCommand
    {
        public AddNewQuotationCommand(int customerId, DateTime startDate)
        {
            CustomerId = customerId;
            StartDate = startDate;
        }

        public override bool IsValid(object repository = null)
        {
            //ValidationResult = new AddNewQuotationCommandValidation().Validate(this);
            //return ValidationResult.IsValid;
            return true;
        }
    }
}
