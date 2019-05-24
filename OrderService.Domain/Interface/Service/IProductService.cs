using OrderService.Domain.Entities;
using OrderService.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.Interface.Service
{
    public interface IProductService : IService<ProductModel, Entities.Product>
    {
        PagedModel Search(ProductModel customer, int page, int pageSize);
        PagedModel GetAllProductByCustomer(int customerId, int page, int pageSize);
    }
}
