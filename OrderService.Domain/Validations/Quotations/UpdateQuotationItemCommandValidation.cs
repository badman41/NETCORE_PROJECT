using OrderService.Domain.Commands.Quotation;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.Validations.Product
{
    public class UpdateQuotationItemCommandValidation : QuotationItemValidation<UpdateQuotationItemCommand>
    {
        public UpdateQuotationItemCommandValidation()
        {
            ValidateUnitId();
            ValidateProductCode();
            ValidatePrice();
        }
    }
}
