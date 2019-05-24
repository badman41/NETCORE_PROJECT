using OrderService.Application.Request;
using OrderService.Application.Response;
using OrderService.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Application.Interfaces
{
    public interface ICustomerGroupAppService
    {
        Task<GetCustomerByGroupResponse> getCustomerGroupById(GetCustomerByGroupRequest request);
        Task<GetAllCustomerGroupResponse> getAllCustomerGroup();
        Task<AddNewCustomerGroupResponse> addNewCustomerGroup(AddNewCustomerGroupRequest request);
        Task<DeleteCustomerGroupResponse> deleteCustomerGroup(DeleteCustomerGroupRequest request);
        Task<AddCustomerToGroupResponse> addCustomerToGroup(AddCustomerToGroupRequest request);
    }
}
