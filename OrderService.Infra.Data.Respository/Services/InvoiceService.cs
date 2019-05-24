using OrderService.Domain.Entities;
using OrderService.Domain.Interface.Respository;
using OrderService.Domain.Interface.Service;
using OrderService.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Infra.Data.Respository.Services
{
    public class InvoiceService : BaseService<InvoiceModel, Invoice>, IInvoiceService
    {
        private readonly IInvoiceRepository _repository;
        public InvoiceService(IInvoiceRepository repository)
            : base(repository)
        {
            _repository = repository;
        }

        public PagedModel Search(DateTime? deliveryTime, string customerCode, int customerId, int page, int pageSize)
        {
            return _repository.Search(deliveryTime, customerCode, customerId, page, pageSize);
        }
    }
}
