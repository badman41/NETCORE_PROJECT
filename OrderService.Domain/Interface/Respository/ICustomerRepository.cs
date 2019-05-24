using OrderService.Domain.Entities;
using OrderService.Domain.ReadModels;
using System;
using System.Collections.Generic;

namespace OrderService.Domain.Interface.Respository
{
    public interface ICustomerRepository : IRepository<CustomerModel, Entities.Customer>
    {
        PagedModel Search(CustomerModel customer, int page, int pageSize);
        CustomerModel GetByCode(string code);
        IEnumerable<CustomerModel> GetListByCustomerGroup(int customerGroup);
        bool ChangeCustomerGroup(int customerGroupId, int customerId);
        IEnumerable<OrderedCustomerModel> GetListOrdered(DateTime date);
    }
}
