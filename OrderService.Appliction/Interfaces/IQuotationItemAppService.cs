using OrderService.Application.Request;
using OrderService.Application.Response;
using OrderService.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Application.Interfaces
{
    public interface IQuotationItemAppService
    {
        Task<bool> addNewQuotation(AddNewQuotationItemRequest request);
        Task<bool> updateQuotation(UpdateQuotationItemRequest request);
        //Task<DeleteQuotationResponse> deleteQuotation(DeleteQuotationRequest request);
    }
}
