using OrderService.Domain.Entities;
using OrderService.Domain.Interface.Respository;
using OrderService.Domain.Interface.Service;
using OrderService.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Infra.Data.Respository.Services
{
    public class QuotationService : BaseService<QuotationModel, Quotation>, IQuotationService
    {
        private readonly IQuotationRepository _repository;
        public QuotationService(IQuotationRepository repository)
            : base(repository)
        {
            _repository = repository;
        }
        public PagedModel Search(string customerCode, int page, int pageSize)
        {
            return _repository.Search(customerCode, page, pageSize);
        }
        public IEnumerable<ProductPriceModel> GetListProductOfQuotation(int quotationId)
        {
            return _repository.GetListProductOfQuotation(quotationId);
        }
    }
}
