using OrderService.Domain.Entities;
using OrderService.Domain.Interface.Respository;
using OrderService.Domain.Interface.Service;
using OrderService.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Infra.Data.Respository.Services
{
    public class InvoiceItemService : BaseService<InvoiceItemModel, InvoiceItem>, IInvoiceItemService
    {
        private readonly IInvoiceItemRepository _repository;
        public InvoiceItemService(IInvoiceItemRepository repository)
            : base(repository)
        {
            _repository = repository;
        }

        public IEnumerable<InvoiceItemModel> GetListItemOfInvoice(int InvoiceItemId)
        {
            return _repository.GetListItemOfInvoice(InvoiceItemId);
        }
    }
}
