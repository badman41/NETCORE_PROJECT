using OrderService.Domain.Entities;
using OrderService.Domain.ReadModels;

namespace OrderService.Domain.Interface.Respository
{
    public interface ICustomerGroupRepository : IRepository<CustomerGroupModel, Entities.CustomerGroup>
    {
    }
}
