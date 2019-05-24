using OrderService.Domain.Entities;
using OrderService.Domain.Interface.Respository;
using OrderService.Domain.Interface.Service;
using OrderService.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Infra.Data.Respository.Services
{
    public class QuotationItemService : BaseService<QuotationItemModel, QuotationItem>, IQuotationItemService
    {
        private readonly IQuotationItemRepository _repository;
        public QuotationItemService(IQuotationItemRepository repository)
            : base(repository)
        {
            _repository = repository;
        }

        public IEnumerable<QuotationItemModel> GetListItemOfQuotation(int QuotationItemId, int productId)
        {
            return _repository.GetListItemOfQuotation(QuotationItemId, productId);
        }
    }
}
