using MassTransit;
using MediatR;
using OrderService.Application.Interfaces;
using OrderService.Application.Request;
using OrderService.Application.Response;
using OrderService.Application.Shared;
using OrderService.Domain.Bus;
//using OrderService.Domain.Commands.Quotation;
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
using OrderService.Domain.Commands.Quotation;

namespace OrderService.Application.Implements
{
    public class QuotationAppService : IQuotationAppService
    {
        private readonly IQuotationService _service;
        private readonly IQuotationItemService _quotationItemService;
        private readonly ICustomerService _customerService;
        private readonly IMediatorHandler Bus;
        private readonly DomainNotificationHandler _notifications;

        public QuotationAppService(IQuotationService QuotationService, IQuotationItemService QuotationItemService
                                    , ICustomerService CustomerService,IMediatorHandler bus
                                    , INotificationHandler<DomainNotification> notifications)
        {
            _service = QuotationService;
            _quotationItemService = QuotationItemService;
            _customerService = CustomerService;
            Bus = bus;
            _notifications = (DomainNotificationHandler)notifications;

        }

        public Task<AddNewQuotationResponse> addNewQuotation(AddNewQuotationRequest request)
        {
            AddNewQuotationResponse response = new AddNewQuotationResponse();
            CustomerModel customer = _customerService.GetByCode(request.customer_code);

            if (customer == null) return Task.FromResult(response);
            AddNewQuotationCommand command = new AddNewQuotationCommand
            (
                customer.ID,
                request.date
            );
            Task<object> Quotation = (Task<object>)Bus.SendCommand(command);
            //RabbitMQBus.Publish(command);
            
            response = Common<AddNewQuotationResponse>.checkHasNotification(_notifications, response);
            if (response.Success)
            {
                QuotationModel QuotationModel = (QuotationModel)Quotation.Result;
                response.Data = QuotationModel.ID;
            }
            return Task.FromResult(response);
        }

        public Task<AddNewQuotationByGroupResponse> addNewQuotationByCustomerGroup(AddNewQuotationByGroupRequest request)
        {
            AddNewQuotationByGroupResponse response = new AddNewQuotationByGroupResponse() { Data = new List<int>() } ;
            IEnumerable<CustomerModel> customers = _customerService.GetListByCustomerGroup(request.group_id);

            if (customers == null || customers.Count() < 1) return Task.FromResult(response);
            foreach (CustomerModel customer in customers)
            {
                AddNewQuotationCommand command = new AddNewQuotationCommand
                (
                    customer.ID,
                    request.date
                );
                Task<object> Quotation = (Task<object>)Bus.SendCommand(command);

                response = Common<AddNewQuotationByGroupResponse>.checkHasNotification(_notifications, response);
                if (response.Success)
                {
                    QuotationModel QuotationModel = (QuotationModel)Quotation.Result;
                    response.Data.Add(QuotationModel.ID);
                }
            }
            
            
            return Task.FromResult(response);
        }

        public Task<GetAllQuotationResponse> getAllQuotation(SearchQuotationRequest request)
        {
            GetAllQuotationResponse response = new GetAllQuotationResponse();
            try
            {
                List<QuotationModel> datas = new List<QuotationModel>();
                PagedModel pagedModel = _service.Search(request.Code, request.Page, request.PageSize);
                
                if (pagedModel.Data != null)
                {
                    foreach (QuotationModel q in pagedModel.Data)
                    {
                        //lay danh sach san pham co trong bao gia
                        q.ProductPrices = _service.GetListProductOfQuotation(q.ID);
                        if(q.ProductPrices !=null)
                        {
                            //lay danh sach bao gia cho tung don vi tinh theo san pham
                            foreach (ProductPriceModel p in q.ProductPrices)
                            {
                                p.Prices = _quotationItemService.GetListItemOfQuotation(q.ID, p.ProductID);
                            }
                        }
                        datas.Add(q);
                    }
                }
                response.Data = datas;
                response.Page = pagedModel.Page;
                response.PageSize = pagedModel.PageSize;
                response.Total = pagedModel.PageTotal;
            }
            catch(Exception e)
            {
                //response.Data = e as object;
            }
            return Task.FromResult(response);
        }

        //public Task<AddNewQuotationResponse> addNewQuotation(AddNewQuotationRequest request)
        //{
        //    AddNewQuotationCommand command = new AddNewQuotationCommand 
        //    (
        //        request.Quotation.Name,
        //        request.Quotation.Note,
        //        request.Quotation.Code,
        //        request.Quotation.OtherUnitOfQuotation,
        //        request.Quotation.UnitId,
        //        request.Quotation.WeightPerUnit
        //    );
        //    Task<object> Quotation = (Task<object>)Bus.SendCommand(command);
        //    //RabbitMQBus.Publish(command);
        //    AddNewQuotationResponse response = new AddNewQuotationResponse();
        //    response = Common<AddNewQuotationResponse>.checkHasNotification(_notifications, response);
        //    if (response.Success)
        //    {
        //        QuotationModel QuotationModel = (QuotationModel)Quotation.Result;
        //        response.Data = QuotationModel.Id;
        //    }
        //    return Task.FromResult(response);
        //}

        //public Task<GetQuotationResponse> getQuotationById(int id)
        //{
        //    GetQuotationResponse response = new GetQuotationResponse();
        //    try
        //    {
        //        QuotationModel p = _service.Get(id);
        //        p.Unit = _unitService.Get(p.UnitId);//lay don vi tinh thong thuong
        //        p.OtherUnitOfQuotation = _unitService.GetALlUnitOfQuotation(p.Id);//lay don vi tinh trong bao gia

        //        DataQuotationResponse d = new DataQuotationResponse();
        //        d.ID = p.Id;
        //        d.UpdatedAt = p.UpdatedAt;
        //        d.CreatedAt = p.CreatedAt;
        //        d.QuotationInfo = p;

        //        response.Data = d;

        //        response.Message = "Success";
        //        response.Success = true;
        //    }
        //    catch (Exception e)
        //    {
        //        response.Message = e.ToString();
        //        response.Success = true;
        //    }
        //    return Task.FromResult(response);
        //}

        //public Task<UpdateQuotationResponse> updateQuotation(UpdateQuotationRequest request)
        //{
        //    UpdateQuotationCommand command = new UpdateQuotationCommand
        //    (
        //        request.Quotation.Id,
        //        request.Quotation.Name,
        //        request.Quotation.Note,
        //        request.Quotation.Code,
        //        request.Quotation.OtherUnitOfQuotation,
        //        request.Quotation.UnitId,
        //        request.Quotation.WeightPerUnit
        //    );
        //    Task<object> Quotation = (Task<object>)Bus.SendCommand(command);
        //    UpdateQuotationResponse response = new UpdateQuotationResponse();
        //    response = Common<UpdateQuotationResponse>.checkHasNotification(_notifications, response);
        //    if (response.Success)
        //    {
        //        response.Data = (bool) Quotation.Result;
        //    }
        //    return Task.FromResult(response);
        //}
    }
}
