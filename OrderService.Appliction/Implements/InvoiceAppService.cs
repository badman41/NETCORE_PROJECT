using MassTransit;
using MediatR;
using OrderService.Application.Interfaces;
using OrderService.Application.Request;
using OrderService.Application.Response;
using OrderService.Application.Shared;
using OrderService.Domain.Bus;
//using OrderService.Domain.Commands.Invoice;
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
using OrderService.Domain.Commands.Product;
using OrderService.Domain.Commands.Invoices;
using OrderService.Domain.Interface.Respository;
using TransitionApp.Domain.ReadModel.Invoice;

namespace OrderService.Application.Implements
{
    public class InvoiceAppService : IInvoiceAppService
    {
        private readonly IInvoiceService _service;
        private readonly IInvoiceItemService _invoiceItemService;
        private readonly IAddressService _addressService;
        private readonly IProductRepository _ProductRepository;
        private readonly IInvoiceRepository _InvoiceRepository;
        private readonly IMediatorHandler Bus;
        private readonly DomainNotificationHandler _notifications;
        private readonly IBus RabbitMQBus;

        public InvoiceAppService(IInvoiceService InvoiceService, IAddressService addressService, IInvoiceItemService invoiceItemService, IMediatorHandler bus
                                    , INotificationHandler<DomainNotification> notifications, IBus rabbitMQBus, IProductRepository productRepository
                                    , IInvoiceRepository invoiceRepository)
        {
            _service = InvoiceService;
            _addressService = addressService;
            _invoiceItemService = invoiceItemService;
            Bus = bus;
            _notifications = (DomainNotificationHandler)notifications;
            RabbitMQBus = rabbitMQBus;
            _ProductRepository = productRepository;
            _InvoiceRepository = invoiceRepository;
        }

        public Task<AddNewInvoiceResponse> addNewInvoice(AddNewInvoiceRequest request)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(request.DeliveryTime).ToLocalTime();
            List<InvoiceItem> items = new List<InvoiceItem>();
            int totalPrice = 0;
            decimal totalWeight = 0;
            foreach (InvoiceItemModel invoiceItem in request.Items)
            {
                totalPrice += invoiceItem.TotalPrice;
                decimal weightPerUnit = _ProductRepository.GetWeightPerUnit(invoiceItem.ProductID, invoiceItem.UnitID);
                invoiceItem.Weight = weightPerUnit * invoiceItem.Quantity;
                totalWeight += invoiceItem.Weight;
            }
            AddNewInvoiceCommand command = new AddNewInvoiceCommand ()
            {
                Address = request.Address,
                CustomerCode= request.CustomerCode,
                CustomerID= request.CustomerID,
                CustomerName= request.CustomerName,
                DeliveryTime= dtDateTime,
                Code = Guid.NewGuid().ToString().Substring(0,8),
                TotalPrice = totalPrice,
                WeightTotal = totalWeight,
                Items= request.Items
            };
            Task<object> Invoice = (Task<object>)Bus.SendCommand(command);
            RabbitMQBus.Publish(command);
            AddNewInvoiceResponse response = new AddNewInvoiceResponse();
            response = Common<AddNewInvoiceResponse>.checkHasNotification(_notifications, response);
            if (response.Success)
            {
                response.Data = (int)Invoice.Result;
            }
            return Task.FromResult(response);
        }

        public Task<GetAllInvoiceResponse> getAllInvoice(SearchInvoiceRequest request)
        {
            GetAllInvoiceResponse response = new GetAllInvoiceResponse();
            try
            {
                List<InvoiceModel> datas = new List<InvoiceModel>();
                PagedModel pagedModel = _service.Search(request.from_time, request.customer_code, request.customer_id, request.page, request.page_size);
                
                if (pagedModel.Data != null)
                {
                    foreach (InvoiceModel q in pagedModel.Data)
                    {
                        q.Address = _addressService.Get(q.AddressId);
                        q.Items = _invoiceItemService.GetListItemOfInvoice(q.ID);
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
                Console.WriteLine(e.ToString());
            }
            return Task.FromResult(response);
        }

        public Task<GetAllInvoiceResponse> getInvoice(int invoiceId)
        {
            GetAllInvoiceResponse response = new GetAllInvoiceResponse();
            try
            {
                List<InvoiceModel> datas = new List<InvoiceModel>();
                InvoiceModel model = _service.Get(invoiceId);

                if (model != null)
                {
                    model.Address = _addressService.Get(model.AddressId);
                    model.Items = _invoiceItemService.GetListItemOfInvoice(model.ID);
                }
                datas.Add(model);
                response.Data = datas;
                response.Total = 1;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return Task.FromResult(response);
        }

        public Task<UpdateInvoiceResponse> updateInvoice(UpdateInvoiceRequest request)
        {
            List<InvoiceItem> items = new List<InvoiceItem>();
            int totalPrice = 0;
            decimal totalWeight = 0;
            foreach (InvoiceItemModel invoiceItem in request.Items)
            {
                totalPrice += invoiceItem.TotalPrice;
                decimal weightPerUnit = _ProductRepository.GetWeightPerUnit(invoiceItem.ProductID, invoiceItem.UnitID);
                invoiceItem.Weight = weightPerUnit * invoiceItem.Quantity;
                totalWeight += invoiceItem.Weight;
            }
            UpdateInvoiceCommand command = new UpdateInvoiceCommand()
            {
                ID = request.ID,
                Address = request.Address,
                CustomerCode = request.CustomerCode,
                CustomerID = request.CustomerID,
                CustomerName = request.CustomerName,
                TotalPrice = totalPrice,
                WeightTotal = totalWeight,
                Note = request.Note,
                Code = request.Code,
                Items = request.Items
            };
            Task<object> Invoice = (Task<object>)Bus.SendCommand(command);
            RabbitMQBus.Publish(command);
            UpdateInvoiceResponse response = new UpdateInvoiceResponse();
            response = Common<UpdateInvoiceResponse>.checkHasNotification(_notifications, response);
            response.OK = response.Success;
            response.Content = "";
            return Task.FromResult(response);
        }
        public Task<CancelInvoiceResponse> cancleInvoice(int id)
        {
            CancelInvoiceResponse response = new CancelInvoiceResponse() { Content = "" };
            string code = _InvoiceRepository.CancleStatus(id);
            if(code != null)
            {
                RabbitMQBus.Publish(new CancelInvoiceModel() { Code = code, Status = 3});
            }
            response.OK = code != null;
            return Task.FromResult(response);
        }
    }
}
