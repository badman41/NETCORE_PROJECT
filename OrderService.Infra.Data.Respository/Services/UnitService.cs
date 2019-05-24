using OrderService.Domain.Entities;
using OrderService.Domain.Interface.Respository;
using OrderService.Domain.Interface.Service;
using OrderService.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Infra.Data.Respository.Services
{
    public class UnitService : BaseService<UnitModel, Unit>, IUnitService
    {
        private readonly IUnitRepository _repository;
        public UnitService(IUnitRepository repository)
            : base(repository)
        {
            _repository = repository;
        }

        public IEnumerable<ProductUnitModel> GetALlOtherUnitOfProduct(int productId)
        {
            return _repository.GetALlOtherUnitOfProduct(productId);
        }

        public IEnumerable<ProductUnitModel> GetALlProductUnitOfCustomer(int productId, int customerId)
        {
            return _repository.GetALlProductUnitOfCustomer(productId, customerId);
        }
    }
}
