using OrderService.Domain.Validations.Customer;

namespace OrderService.Domain.Commands.Customer
{
    public class AddNewCustomerCommand : CustomerCommand
    {
        public AddNewCustomerCommand(string name, string note, string cartCode, string phoneNumber, string email
                            , string code, int status, string city, string country, string district, string street, string streetNumber, string lat, string lng)
        {
            Name = name;
            Note = note;
            CartCode = cartCode;
            PhoneNumber = phoneNumber;
            Email = email;
            Code = code;
            Status = status;
            City = city;
            Country = country;
            District = district;
            Street = street;
            StreetNumber = streetNumber;
            Lat = lat;
            Lng = lng;
        }

        public override bool IsValid(object repository = null)
        {
            ValidationResult = new AddNewProductCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
