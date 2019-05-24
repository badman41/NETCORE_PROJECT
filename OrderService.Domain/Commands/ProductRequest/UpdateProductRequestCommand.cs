using OrderService.Domain.ReadModels;
using OrderService.Domain.Validations.Product;
using System;
using System.Collections.Generic;

namespace OrderService.Domain.Commands.ProductRequest
{
    public class UpdateProductRequestCommand : ProductRequestCommand
    {
        public UpdateProductRequestCommand(int id, string response, int status)
        {
            Id = id;
            Response = response;
            Status = status;
        }
        public override bool IsValid(object repository = null)
        {
            //ValidationResult = new AddNewProductCommandValidation().Validate(this);
            //return ValidationResult.IsValid;
            return true;
        }
    }
}
