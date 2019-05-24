using OrderService.Domain.Entities;
using OrderService.Domain.Interface.Respository;
using OrderService.Domain.Interface.Service;
using OrderService.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Infra.Data.Respository.Services
{
    public class ProductService : BaseService<ProductModel, Product>, IProductService
    {
        private readonly IProductRepository _repository;
        public ProductService(IProductRepository repository)
            : base(repository)
        {
            _repository = repository;
        }

        public PagedModel GetAllProductByCustomer(int customerId, int page, int pageSize)
        {
            return _repository.GetAllProductByCustomer(customerId, page, pageSize);
        }

        public PagedModel Search(ProductModel customer, int page, int pageSize)
        {
            return _repository.Search(customer, page, pageSize);
        }
    }
}
