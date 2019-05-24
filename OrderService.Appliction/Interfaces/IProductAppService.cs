using OrderService.Application.Request;
using OrderService.Application.Response;
using OrderService.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Application.Interfaces
{
    public interface IProductAppService
    {
        Task<GetAllProductResponse> getAllProduct(SearchProductRequest request);
        Task<GetAllProductResponse> getAllProductByCustomer(GetAllProductByCustomerRequest request);
        Task<GetProductResponse> getProductById(int id);
        Task<AddNewProductResponse> addNewProduct(AddNewProductRequest request);
        Task<UpdateProductResponse> updateProduct(UpdateProductRequest request);
        //Task<DeleteProductResponse> deleteProduct(DeleteProductRequest request);
    }
}
