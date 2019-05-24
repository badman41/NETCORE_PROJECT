using OrderService.Domain.Entities;
using OrderService.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.Interface.Service
{
    public interface IInvoiceItemService : IService<InvoiceItemModel, InvoiceItem>
    {
        IEnumerable<InvoiceItemModel> GetListItemOfInvoice(int InvoiceId);
    }
}
