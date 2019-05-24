using OrderService.Domain.Entities;
using OrderService.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.Interface.Service
{
    public interface IUnitService : IService<UnitModel, OrderService.Domain.Entities.Unit>
    {
       IEnumerable<ProductUnitModel> GetALlOtherUnitOfProduct(int productId);//lay cac don vi tinh khac cua product
       IEnumerable<ProductUnitModel> GetALlProductUnitOfCustomer(int productId, int customerId);
    }
}
