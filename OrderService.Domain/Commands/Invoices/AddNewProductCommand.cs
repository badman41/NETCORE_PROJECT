using OrderService.Domain.Commands.Invoices;

namespace OrderService.Domain.Commands.Invoices
{
    public class AddNewInvoiceCommand : InvoiceCommand
    {
        public AddNewInvoiceCommand()
        {
        }

        public override bool IsValid(object repository = null)
        {
            //ValidationResult = new AddNewProductCommandValidation().Validate(this);
            //return ValidationResult.IsValid;
            return true;
        }
    }
}
