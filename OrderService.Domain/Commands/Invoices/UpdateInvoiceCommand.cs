using OrderService.Domain.Commands.Invoices;
using OrderService.Domain.Commands.Product;
using OrderService.Domain.ReadModels;
using System.Collections.Generic;

namespace OrderService.Domain.Commands.Invoices
{
    public class UpdateInvoiceCommand : InvoiceCommand
    {
        public UpdateInvoiceCommand()
        {
        }

        public override bool IsValid(object repository = null)
        {
            //ValidationResult = new UpdateInvoiceCommandValidation().Validate(this);
            //return ValidationResult.IsValid;
            return true;
        }
    }
}
