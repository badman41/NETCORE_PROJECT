using MassTransit;
using MediatR;
using OrderService.Application.Interfaces;
using OrderService.Application.Request;
using OrderService.Application.Response;
using OrderService.Application.Shared;
using OrderService.Domain.Bus;
//using OrderService.Domain.Commands.Customer;
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
    public class CustomerAppService : ICustomerAppService
    {
        private readonly ICustomerService _service;
        private readonly IAddressService _addressService;
        private readonly IMediatorHandler Bus;
        private readonly IBus RabbitMQBus;
        private readonly DomainNotificationHandler _notifications;
        private readonly IQuotationAppService _QuotationAppService;

        public CustomerAppService(ICustomerService CustomerService, IAddressService AddressService, IMediatorHandler bus, IBus rabbitMQBus
                                    , INotificationHandler<DomainNotification> notifications
                                    , IQuotationAppService QuotationAppService)
        {
            _service = CustomerService;
            _addressService = AddressService;
            Bus = bus;
            _notifications = (DomainNotificationHandler)notifications;
            _QuotationAppService = QuotationAppService;
            RabbitMQBus = rabbitMQBus;

        }

        public Task<GetAllCustomerResponse> getAllCustomer(SearchCustomerRequest request)
        {
            GetAllCustomerResponse response = new GetAllCustomerResponse();
            try
            {
                CustomerModel customerModel = new CustomerModel();
                customerModel.Name = request.Name;
                customerModel.Code = request.Code;
                customerModel.PhoneNumber = request.PhoneNumber;
                PagedModel model = _service.Search(customerModel, request.Page, request.PageSize);
                response.Data = (IEnumerable<CustomerModel>) model.Data;
                if (response.Data!=null)
                {
                    foreach (CustomerModel customer in response.Data)
                    {
                        customer.Address = _addressService.Get(customer.AddressId);
                    }
                }
                response.Status = new StatusResponse("OK", true);
                response.Total = model.PageTotal;
                response.Page = request.Page;
                response.PageSize = request.PageSize;
            }
            catch(Exception e)
            {
                response.Status = new StatusResponse(e.ToString(), false);
            }
            return Task.FromResult(response);
        }

        public Task<GetCustomerResponse> getCustomerById(int id)
        {
            GetCustomerResponse response = new GetCustomerResponse();
            try
            {
                response.Data = _service.Get(id);
                response.Data.Address =  _addressService.Get(response.Data.AddressId);

                response.Status = new StatusResponse("OK", true);
            }
            catch (Exception e)
            {
                response.Status = new StatusResponse(e.ToString(), false);
            }
            return Task.FromResult(response);
        }

        public Task<AddNewCustomerResponse> addNewCustomer(AddNewCustomerRequest request)
        {
            AddNewCustomerCommand command = new AddNewCustomerCommand(
                    request.Name,
                    request.Note,
                    request.CartCode,
                    request.PhoneNumber,
                    request.Email,
                    request.Code,
                    request.Status,
                    request.Address.City,
                    request.Address.Country,
                    request.Address.District,
                    request.Address.Street,
                    request.Address.StreetNumber,
                     request.Address.Lat.ToString(),
                    request.Address.Lng.ToString()
                );
            Task<object> Customer = (Task<object>)Bus.SendCommand(command);
            RabbitMQBus.Publish(command);
            AddNewCustomerResponse response = new AddNewCustomerResponse();
            response = Common<AddNewCustomerResponse>.checkHasNotification(_notifications, response);
            if (response.Success)
            {
                CustomerModel CustomerModel = (CustomerModel)Customer.Result;
                //them bao gia
                AddNewQuotationRequest request2 = new AddNewQuotationRequest()
                {
                    customer_code = CustomerModel.Code,
                    date = DateTime.Now
                };
                _QuotationAppService.addNewQuotation(request2);
                response.Data = CustomerModel.ID;
            }
            return Task.FromResult(response);
        }

        public Task<UpdateCustomerResponse> updateCustomer(UpdateCustomerRequest request)
        {
            UpdateCustomerCommand command = new UpdateCustomerCommand(
                   request.Id,
                   request.Name,
                   request.Note,
                   request.CartCode,
                   request.PhoneNumber,
                   request.Email,
                   request.Code,
                   request.Status,
                   request.Address.City,
                   request.Address.Country,
                   request.Address.District,
                   request.Address.Street,
                   request.Address.StreetNumber,
                    request.Address.Lat.ToString(),
                   request.Address.Lng.ToString()
               );
            Task<object> Customer = (Task<object>)Bus.SendCommand(command);
            RabbitMQBus.Publish(command);
            UpdateCustomerResponse response = new UpdateCustomerResponse();
            response = Common<UpdateCustomerResponse>.checkHasNotification(_notifications, response);
            if (response.Success)
            {
                bool result = (bool)Customer.Result;
                response.Data = result;
            }
            return Task.FromResult(response);
        }

        public Task<IEnumerable<OrderedCustomerModel>> getListOrderedCustomer(DateTime date)
        {
            IEnumerable<OrderedCustomerModel> response = new List<OrderedCustomerModel>();
            try
            {
                response = _service.GetListOrdered(date);
            }
            catch (Exception e)
            {
                throw e;
            }
            return Task.FromResult(response);
        }

        //public Task<DeleteCustomerResponse> deleteCustomer(DeleteCustomerRequest request)
        //{
        //    DeleteCustomerCommand command = new DeleteCustomerCommand(request.Id);
        //    Task<object> Customer = (Task<object>)Bus.SendCommand(command);
        //    //RabbitMQBus.Publish(command);
        //    DeleteCustomerResponse response = new DeleteCustomerResponse();
        //    response = Common<DeleteCustomerResponse>.checkHasNotification(_notifications, response);
        //    return Task.FromResult(response);
        //}
    }
}
