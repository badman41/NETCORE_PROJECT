using OrderService.Application.Request;
using OrderService.Application.Response;
using OrderService.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Application.Interfaces
{
    public interface IInvoiceAppService
    {
        Task<GetAllInvoiceResponse> getAllInvoice(SearchInvoiceRequest request);
        Task<GetAllInvoiceResponse> getInvoice(int invoiceId);
        Task<AddNewInvoiceResponse> addNewInvoice(AddNewInvoiceRequest request);
        Task<UpdateInvoiceResponse> updateInvoice(UpdateInvoiceRequest request);
        Task<CancelInvoiceResponse> cancleInvoice(int id);
    }
}
