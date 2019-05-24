using OrderService.Domain.Entities;
using OrderService.Domain.ReadModels;
using System;
using System.Collections.Generic;

namespace OrderService.Domain.Interface.Respository
{
    public interface IInvoiceRepository : IRepository<InvoiceModel, Entities.Invoice>
    {
        PagedModel Search(DateTime? deliveryTime, string customerCode, int customerId, int page, int pageSize);
        bool UpdateStatus(string invoiceCode, int status, bool served);
        string CancleStatus(int id);
    }
}
