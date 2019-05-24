using OrderService.Domain.Entities;
using OrderService.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.Interface.Service
{
    public interface IProductRequestService : IService<ProductRequestModel, Entities.ProductRequest>
    {
        PagedModel Search(int? status, int page, int pageSize);
        PagedModel GetAllByCustomer(int customerId, int page, int pageSize);
    }
}
