using OrderService.Domain.Commands.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.Validations.Product
{
    public class UpdateProductCommandValidation : ProductValidation<UpdateProductCommand>
    {
        public UpdateProductCommandValidation()
        {
            ValidateName();
            ValidateCode();
        }
    }
}
