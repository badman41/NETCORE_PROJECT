using OrderService.Application.Request;
using OrderService.Application.Response;
using OrderService.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Application.Interfaces
{
    public interface IProductRequestAppService
    {
        Task<GetAllProductRequestResponse> getAllProductRequest(SearchProductRequestRequest request);
        Task<GetAllProductRequestResponse> getAllByCustomer(GetAllProductRequestByCustomerRequest request);
        Task<AddNewProductRequestResponse> addNewProductRequest(AddNewProductRequestRequest request);
        Task<UpdateProductRequestResponse> updateProductRequest(UpdateProductRequestRequest request);
        Task<DeleteProductRequestResponse> deleteProductRequest(int id);
    }
}
