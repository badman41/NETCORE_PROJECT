using MassTransit;
using MediatR;
using OrderService.Application.Interfaces;
using OrderService.Application.Request;
using OrderService.Application.Response;
using OrderService.Application.Shared;
using OrderService.Domain.Bus;
//using OrderService.Domain.Commands.ProductRequest;
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
using OrderService.Domain.Commands.ProductRequest;

namespace OrderService.Application.Implements
{
    public class ProductRequestAppService : IProductRequestAppService
    {
        private readonly IProductRequestService _service;
        private readonly IMediatorHandler Bus;
        private readonly DomainNotificationHandler _notifications;

        public ProductRequestAppService(IProductRequestService ProductRequestService, IMediatorHandler bus
                                    , INotificationHandler<DomainNotification> notifications)
        {
            _service = ProductRequestService;
            Bus = bus;
            _notifications = (DomainNotificationHandler)notifications;

        }

        public Task<GetAllProductRequestResponse> getAllProductRequest(SearchProductRequestRequest request)
        {
            GetAllProductRequestResponse response = new GetAllProductRequestResponse();
            try
            {
                //dieu kien search
                PagedModel pagedModel = _service.Search(request.status, request.Page, request.PageSize);
                response.Data = (IEnumerable<ProductRequestModel>)pagedModel.Data;
                response.Total = response.Data.Count();
            }
            catch (Exception e)
            {
                throw e;
            }
            return Task.FromResult(response);
        }

        public Task<AddNewProductRequestResponse> addNewProductRequest(AddNewProductRequestRequest request)
        {
            AddNewProductRequestCommand command = new AddNewProductRequestCommand
            (
                request.description,
                request.product_name,
                request.quantity,
                (int)StatusProductRequest.DangXuLy,
                DateTime.Now,
                request.userId
            );
            Task<object> ProductRequest = (Task<object>)Bus.SendCommand(command);
            //RabbitMQBus.Publish(command);
            AddNewProductRequestResponse response = new AddNewProductRequestResponse();
            response = Common<AddNewProductRequestResponse>.checkHasNotification(_notifications, response);
            if (response.Success)
            {
                ProductRequestModel ProductRequestModel = (ProductRequestModel)ProductRequest.Result;
                response.Data = ProductRequestModel.ID;
            }
            return Task.FromResult(response);
        }
        public Task<UpdateProductRequestResponse> updateProductRequest(UpdateProductRequestRequest request)
        {
            UpdateProductRequestCommand command = new UpdateProductRequestCommand
            (
                request.Id,
                request.response,
                (int)StatusProductRequest.DaXuLy
            );
            Task<object> ProductRequest = (Task<object>)Bus.SendCommand(command);
            UpdateProductRequestResponse response = new UpdateProductRequestResponse();
            response = Common<UpdateProductRequestResponse>.checkHasNotification(_notifications, response);
            if (response.Success)
            {
                response.Data = (bool)ProductRequest.Result;
            }
            return Task.FromResult(response);
        }

        public Task<GetAllProductRequestResponse> getAllByCustomer(GetAllProductRequestByCustomerRequest request)
        {
            GetAllProductRequestResponse response = new GetAllProductRequestResponse();
            try
            {
                //dieu kien search
                PagedModel pagedModel = _service.GetAllByCustomer(request.user_id, request.Page, request.PageSize);
                response.Data = (IEnumerable<ProductRequestModel>)pagedModel.Data;
                response.Total = response.Data.Count();
            }
            catch (Exception e)
            {
                throw e;
            }
            return Task.FromResult(response);
        }

        public Task<DeleteProductRequestResponse> deleteProductRequest(int id)
        {
            DeleteProductRequestCommand command = new DeleteProductRequestCommand
            (
                id
            );
            Task<object> ProductRequest = (Task<object>)Bus.SendCommand(command);
            DeleteProductRequestResponse response = new DeleteProductRequestResponse();
            response = Common<DeleteProductRequestResponse>.checkHasNotification(_notifications, response);
            return Task.FromResult(response);
        }
    }
}
