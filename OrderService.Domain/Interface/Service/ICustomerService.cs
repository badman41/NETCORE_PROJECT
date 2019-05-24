using OrderService.Domain.Entities;
using OrderService.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.Interface.Service
{
    public interface ICustomerService : IService<CustomerModel, Entities.Customer>
    {
        PagedModel Search(CustomerModel customer, int page, int pageSize);
        CustomerModel GetByCode(string code);
        IEnumerable<CustomerModel> GetListByCustomerGroup(int customerGroup);
        IEnumerable<OrderedCustomerModel> GetListOrdered(DateTime date);
    }
}
