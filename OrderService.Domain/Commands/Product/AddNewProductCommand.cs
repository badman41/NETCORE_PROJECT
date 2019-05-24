using OrderService.Domain.ReadModels;
using OrderService.Domain.Validations.Product;
using System.Collections.Generic;

namespace OrderService.Domain.Commands.Product
{
    public class AddNewProductCommand : ProductCommand
    {
        public AddNewProductCommand(string name, string note, string code, IEnumerable<ProductUnitModel> productUnitModels, 
            int commonUnitId, decimal weightPerUnit, int preservationId)
        {
            Name = name;
            Note = note;
            Code = code;
            ProductUnitModels = productUnitModels;
            CommonUnitId = commonUnitId;
            WeightPerUnit = weightPerUnit;
            PreservationId = preservationId;
        }

        public override bool IsValid(object repository = null)
        {
            ValidationResult = new AddNewProductCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
