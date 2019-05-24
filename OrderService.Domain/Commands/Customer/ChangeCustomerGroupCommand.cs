using OrderService.Domain.Validations.Customer;

namespace OrderService.Domain.Commands.Customer
{
    public class ChangeCustomerGroupCommand : CustomerCommand
    {
        public ChangeCustomerGroupCommand(int id, int customerGroupId)
        {
            Id = id;
            CustomerGroupId = customerGroupId;
        }

        public override bool IsValid(object repository = null)
        {
            //ValidationResult = new AddNewProductCommandValidation().Validate(this);
            //return ValidationResult.IsValid;
            return true;
        }
    }
}
