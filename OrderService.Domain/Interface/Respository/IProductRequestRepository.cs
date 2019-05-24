using OrderService.Domain.Entities;
using OrderService.Domain.ReadModels;
using System.Collections.Generic;

namespace OrderService.Domain.Interface.Respository
{
    public interface IProductRequestRepository : IRepository<ProductRequestModel, Entities.ProductRequest>
    {
        PagedModel Search(int? status, int page, int pageSize);
        PagedModel GetAllByCustomer(int customerId, int page, int pageSize);
    }
}
