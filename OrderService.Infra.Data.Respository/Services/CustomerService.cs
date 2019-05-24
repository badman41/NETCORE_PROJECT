using OrderService.Domain.Entities;
using OrderService.Domain.Interface.Respository;
using OrderService.Domain.Interface.Service;
using OrderService.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Infra.Data.Respository.Services
{
    public class CustomerService : BaseService<CustomerModel, Customer>, ICustomerService
    {
        private readonly ICustomerRepository _repository;
        public CustomerService(ICustomerRepository repository)
            : base(repository)
        {
            _repository = repository;
        }

        public PagedModel Search(CustomerModel customer,int page, int pageSize)
        {
            return _repository.Search(customer, page, pageSize);
        }
        public CustomerModel GetByCode(string code)
        {
            return _repository.GetByCode(code);
        }
        public IEnumerable<CustomerModel> GetListByCustomerGroup(int customerGroup)
        {
            return _repository.GetListByCustomerGroup(customerGroup);
        }

        public IEnumerable<OrderedCustomerModel> GetListOrdered(DateTime date)
        {
            return _repository.GetListOrdered(date);
        }
    }
}
