using OrderService.Domain.Entities;
using OrderService.Domain.ReadModels;
using System.Collections.Generic;

namespace OrderService.Domain.Interface.Respository
{
    public interface IProductRepository : IRepository<ProductModel, Entities.Product>
    {
        PagedModel Search(ProductModel customer, int page, int pageSize);
        PagedModel GetAllProductByCustomer(int customerId, int page, int pageSize);
        decimal GetWeightPerUnit(int productId, int unitId);
        ProductModel GetByCode(string code);
    }
}
