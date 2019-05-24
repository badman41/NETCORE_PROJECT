using OrderService.Domain.Entities;
using OrderService.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.Interface.Service
{
    public interface IInvoiceService : IService<InvoiceModel, Entities.Invoice>
    {
        PagedModel Search(DateTime? deliveryTime, string customerCode, int customerId, int page, int pageSize);
    }
}
