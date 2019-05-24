using OrderService.Domain.Entities;
using OrderService.Domain.Interface.Respository;
using OrderService.Domain.Interface.Service;
using OrderService.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Infra.Data.Respository.Services
{
    public class ProductRequestService : BaseService<ProductRequestModel, ProductRequest>, IProductRequestService
    {
        private readonly IProductRequestRepository _repository;
        public ProductRequestService(IProductRequestRepository repository)
            : base(repository)
        {
            _repository = repository;
        }
        
        public PagedModel Search(int? status, int page, int pageSize)
        {
            return _repository.Search(status, page, pageSize);
        }

        public PagedModel GetAllByCustomer(int customerId, int page, int pageSize)
        {
            return _repository.GetAllByCustomer(customerId, page, pageSize);
        }
    }
}
