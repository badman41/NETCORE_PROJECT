using OrderService.Domain.Entities;
using OrderService.Domain.ReadModels;

namespace OrderService.Domain.Interface.Respository
{
    public interface IAddressRepository : IRepository<AddressModel, Address>
    {
        AddressModel GetByLatLng(string lat, string lng);
    }
}
