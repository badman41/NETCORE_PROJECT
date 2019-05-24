using OrderService.Domain.Commands.Quotation;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.Validations.Product
{
    public class AddNewQuotationItemCommandValidation : QuotationItemValidation<AddNewQuotationItemCommand>
    {
        public AddNewQuotationItemCommandValidation()
        {
            ValidateUnitId();
            ValidateProductCode();
            ValidatePrice();
        }
    }
}
