using OrderService.Domain.Entities;
using OrderService.Domain.ReadModels;
using System.Collections.Generic;

namespace OrderService.Domain.Interface.Respository
{
    public interface IUnitRepository : IRepository<UnitModel, OrderService.Domain.Entities.Unit>
    {
        IEnumerable<ProductUnitModel> GetALlOtherUnitOfProduct(int productId);//lay cac don vi tinh khac cua product
        IEnumerable<ProductUnitModel> GetALlProductUnitOfCustomer(int productId, int customerId);
        UnitModel GetByName(string name);
    }
}
