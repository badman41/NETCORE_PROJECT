using OrderService.Domain.ReadModels;
using OrderService.Domain.Validations.Product;
using System;
using System.Collections.Generic;

namespace OrderService.Domain.Commands.ProductRequest
{
    public class DeleteProductRequestCommand : ProductRequestCommand
    {
        public DeleteProductRequestCommand(int id)
        {
            Id = id;
        }
        public override bool IsValid(object repository = null)
        {
            //ValidationResult = new AddNewProductCommandValidation().Validate(this);
            //return ValidationResult.IsValid;
            return true;
        }
    }
}
