using OrderService.Application.Request;
using OrderService.Application.Response;
using OrderService.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Application.Interfaces
{
    public interface ICustomerAppService
    {
        Task<GetAllCustomerResponse> getAllCustomer(SearchCustomerRequest request);
        Task<IEnumerable<OrderedCustomerModel>> getListOrderedCustomer(DateTime date);
        Task<GetCustomerResponse> getCustomerById(int id);
        Task<AddNewCustomerResponse> addNewCustomer(AddNewCustomerRequest request);
        Task<UpdateCustomerResponse> updateCustomer(UpdateCustomerRequest request);
        //Task<DeleteCustomerResponse> deleteCustomer(DeleteCustomerRequest request);
    }
}
