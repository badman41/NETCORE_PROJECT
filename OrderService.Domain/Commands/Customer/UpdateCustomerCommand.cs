using OrderService.Domain.Validations.Customer;

namespace OrderService.Domain.Commands.Customer
{
    public class UpdateCustomerCommand : CustomerCommand
    {
        public UpdateCustomerCommand(int id,string name, string note, string cartCode, string phoneNumber, string email
                            , string code, int status, string city, string country, string district, string street, string streetNumber, string lat, string lng)
        {
            Id = id;
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
            ValidationResult = new UpdateCustomerCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
