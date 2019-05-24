using OrderService.Domain.ReadModels;
using OrderService.Domain.Validations.Product;
using System.Collections.Generic;

namespace OrderService.Domain.Commands.Quotation
{
    public class AddNewQuotationItemCommand : QuotationItemCommand
    {
        public AddNewQuotationItemCommand(int quotationId, int unitId, string productCode, int price)
        {
            QuotationId = quotationId;
            UnitId = unitId;
            ProductCode = productCode;
            Price = price;
        }

        public override bool IsValid(object repository = null)
        {
            ValidationResult = new AddNewQuotationItemCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
