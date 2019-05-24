using OrderService.Domain.ReadModels;
using OrderService.Domain.Validations.Product;
using System;
using System.Collections.Generic;

namespace OrderService.Domain.Commands.ProductRequest
{
    public class AddNewProductRequestCommand : ProductRequestCommand
    {
        public AddNewProductRequestCommand(string description, string productName, int quantity,int status
            , DateTime createdAt, int userId)
        {
            Description = description;
            ProductName = productName;
            Quantity = quantity;
            Status = status;
            CreatedAt = createdAt;
            UserId = userId;
            Response = "";
        }
        public override bool IsValid(object repository = null)
        {
            //ValidationResult = new AddNewProductCommandValidation().Validate(this);
            //return ValidationResult.IsValid;
            return true;
        }
    }
}
