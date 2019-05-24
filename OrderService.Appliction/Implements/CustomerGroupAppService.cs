using MassTransit;
using MediatR;
using OrderService.Application.Interfaces;
using OrderService.Application.Request;
using OrderService.Application.Response;
using OrderService.Application.Shared;
using OrderService.Domain.Bus;
using OrderService.Domain.Commands.CustomerGroup;
using OrderService.Domain.Interface.Service;
using OrderService.Domain.Notifications;
using OrderService.Domain.ReadModels;
using OrderService.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using TransitionApp.Models.VehicleXXX;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System;
using OrderService.Domain.Commands.Customer;

namespace OrderService.Application.Implements
{
    public class CustomerGroupAppService : ICustomerGroupAppService
    {
        private readonly ICustomerGroupService _service;
        private readonly ICustomerService _customerService;
        private readonly IAddressService _addressService;
        private readonly IMediatorHandler Bus;
        private readonly DomainNotificationHandler _notifications;

        public CustomerGroupAppService(ICustomerGroupService CustomerGroupService, IMediatorHandler bus
                                        ,ICustomerService CustomerService, IAddressService AddressService
                                        , INotificationHandler<DomainNotification> notifications)
        {
            _service = CustomerGroupService;
            _customerService = CustomerService;
            _addressService = AddressService;
            Bus = bus;
            _notifications = (DomainNotificationHandler)notifications;
        }

        public Task<GetAllCustomerGroupResponse> getAllCustomerGroup()
        {
            GetAllCustomerGroupResponse response = new GetAllCustomerGroupResponse();
            try
            {
                response.Data = _service.All();
                response.Message = "";
                response.Success = true;
                if(response.Data != null)
                {
                    response.Total = response.Data.Count();
                }
            }
            catch(Exception e)
            {
                response.Message = e.ToString();
                response.Success = false ;
            }
            return Task.FromResult(response);
        }

        public Task<AddNewCustomerGroupResponse> addNewCustomerGroup(AddNewCustomerGroupRequest request)
        {
            AddNewCustomerGroupCommand command = new AddNewCustomerGroupCommand(request.Name);
            Task<object> CustomerGroup = (Task<object>) Bus.SendCommand(command);
            //RabbitMQBus.Publish(command);
            AddNewCustomerGroupResponse response = new AddNewCustomerGroupResponse();
            response = Common<AddNewCustomerGroupResponse>.checkHasNotification(_notifications, response);
            if (response.Success)
            {
                CustomerGroupModel CustomerGroupModel = (CustomerGroupModel)CustomerGroup.Result;
                response.Data = CustomerGroupModel.ID;
            }
            return Task.FromResult(response);
        }

        public Task<DeleteCustomerGroupResponse> deleteCustomerGroup(DeleteCustomerGroupRequest request)
        {
            DeleteCustomerGroupCommand command = new DeleteCustomerGroupCommand(request.Id);
            Task<object> CustomerGroup = (Task<object>)Bus.SendCommand(command);
            //RabbitMQBus.Publish(command);
            DeleteCustomerGroupResponse response = new DeleteCustomerGroupResponse();
            response = Common<DeleteCustomerGroupResponse>.checkHasNotification(_notifications, response);
            return Task.FromResult(response);
        }

        public Task<GetCustomerByGroupResponse> getCustomerGroupById(GetCustomerByGroupRequest request)
        {
            GetCustomerByGroupResponse response = new GetCustomerByGroupResponse();
            try
            {
                response.Data = _customerService.GetListByCustomerGroup(request.CustomerGroupId);
                response.Page = request.Page;
                response.PageSize = request.PageSize;
                response.Total = response.Data.Count();
                if (response.Data != null)
                {
                    foreach (CustomerModel customer in response.Data)
                    {
                        customer.Address = _addressService.Get(customer.AddressId);
                    }
                }

                response.Status = new StatusResponse("OK", true);
                if (response.Data != null)
                {
                    response.Total = response.Data.Count();
                }
            }
            catch (Exception e)
            {
                response.Status = new StatusResponse(e.ToString(), false);
            }
            return Task.FromResult(response);
        }

        public Task<AddCustomerToGroupResponse> addCustomerToGroup(AddCustomerToGroupRequest request)
        {
            ChangeCustomerGroupCommand command = new ChangeCustomerGroupCommand(request.customer_id,request.customer_group_id);
            Task<object> CustomerGroup = (Task<object>)Bus.SendCommand(command);
            //RabbitMQBus.Publish(command);
            AddCustomerToGroupResponse response = new AddCustomerToGroupResponse();
            response = Common<AddCustomerToGroupResponse>.checkHasNotification(_notifications, response);
            if (response.Success)
            {
                CustomerGroupModel CustomerGroupModel = (CustomerGroupModel)CustomerGroup.Result;
                response.Data = (bool)CustomerGroup.Result;
            }
            return Task.FromResult(response);
        }
    }
}
