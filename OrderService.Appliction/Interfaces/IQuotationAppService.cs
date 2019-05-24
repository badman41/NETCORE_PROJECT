using OrderService.Application.Request;
using OrderService.Application.Response;
using OrderService.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Application.Interfaces
{
    public interface IQuotationAppService
    {
        Task<GetAllQuotationResponse> getAllQuotation(SearchQuotationRequest request);
        //Task<GetQuotationResponse> getQuotationById(int id);
        Task<AddNewQuotationResponse> addNewQuotation(AddNewQuotationRequest request);
        Task<AddNewQuotationByGroupResponse> addNewQuotationByCustomerGroup(AddNewQuotationByGroupRequest request);
        //Task<UpdateQuotationResponse> updateQuotation(UpdateQuotationRequest request);
        //Task<DeleteQuotationResponse> deleteQuotation(DeleteQuotationRequest request);
    }
}
