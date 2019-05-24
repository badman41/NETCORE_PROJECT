using MediatR;
using Newtonsoft.Json;
using OrderService.Domain.Bus;
using OrderService.Domain.CommandHandlers;
using OrderService.Domain.Commands.Invoices;
using OrderService.Domain.Commands.Product;
using OrderService.Domain.Entities;
using OrderService.Domain.Interface.Respository;
using OrderService.Domain.Notifications;
using OrderService.Domain.ReadModels;
using OrderService.Domain.Shared.ValueObject;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OrderService.Domain.Invoice.CommandHandlers
{
    public class InvoiceCommandhandler :   BaseCommandHandler
                                        , IRequestHandler<AddNewInvoiceCommand, object>
                                        , IRequestHandler<UpdateInvoiceCommand, object>
                                        //, IRequestHandler<DeleteInvoiceCommand, object>
    {

        private readonly IInvoiceRepository _InvoiceRepository;
        private readonly IProductRepository _ProductRepository;
        private readonly ICustomerRepository _CustomerRepository;
        private readonly IMediatorHandler _bus;

        public InvoiceCommandhandler(IInvoiceRepository InvoiceRepository,
                                      IProductRepository ProductRepository,
                                      ICustomerRepository CustomerRepository,
                                      IMediatorHandler bus,
                                      INotificationHandler<DomainNotification> notifications) : base(bus, notifications)
        {
            _InvoiceRepository = InvoiceRepository;
            _ProductRepository = ProductRepository;
            _CustomerRepository = CustomerRepository;
            _bus = bus;
        }

        public Task<object> Handle(AddNewInvoiceCommand command, CancellationToken cancellationToken)
        {
            if (!command.IsValid())
            {
                NotifyValidationErrors(command);
            }
            else
            {
                List<InvoiceItem> items = new List<InvoiceItem>();
                foreach (InvoiceItemModel invoiceItem in command.Items)
                {
                    InvoiceItem item = new InvoiceItem
                        (
                            null,
                            new Entities.Product(new Identity((uint)invoiceItem.ProductID), null, null, null, null, null, null,null),
                            new Entities.Unit(new Identity((uint)invoiceItem.UnitID), null),
                            new Price(invoiceItem.Price),
                            new Note(invoiceItem.Note),
                            new Quantity(invoiceItem.Quantity),
                            new Weight(invoiceItem.Weight),
                            new Price(invoiceItem.TotalPrice),
                            new Deliveried(false),
                            new Quantity(0)
                        );
                    items.Add(item);
                }
                Entities.Invoice Invoice = new Entities.Invoice
                (
                    null,
                    new Entities.Customer((uint)command.CustomerID,null, null, null, null, null, null, null, null),
                    new DeliveryTime(command.DeliveryTime),
                    new Price(command.TotalPrice),
                    new Note(command.Note),
                    new Weight(command.WeightTotal),
                    new Code(command.Code),
                    items
                );
                InvoiceModel model = _InvoiceRepository.Add(Invoice);
                if (model != null)
                {
                    //send sms
                    CustomerModel customer = _CustomerRepository.Get(command.CustomerID);
                    if(customer!= null)
                    {
                        string message = "Bạn vừa đặt hàng thành công trên avita. Mã đơn hàng của bạn là: " +
                            Invoice.Code + ". Vui lòng truy cập fanpage để xem tình trạng đơn hàng";
                        SendSMS.SendSMSCommon.SendSMS(message, "+84" + customer.PhoneNumber.Substring(1));
                    }
                    return Task.FromResult(model.ID as object);
                }
                _bus.RaiseEvent(new DomainNotification("Invoice", "Server error", NotificationCode.Error));

            }
            return Task.FromResult(null as object);
        }

        public Task<object> Handle(UpdateInvoiceCommand command, CancellationToken cancellationToken)
        {
            if (!command.IsValid())
            {
                NotifyValidationErrors(command);
            }
            else
            {
                List<InvoiceItem> items = new List<InvoiceItem>();
                foreach (InvoiceItemModel invoiceItem in command.Items)
                {
                    InvoiceItem item = new InvoiceItem
                        (
                            new Identity((uint)invoiceItem.ID),
                            new Entities.Product(new Identity((uint)invoiceItem.ProductID), null, null, null, null, null, null, null),
                            new Entities.Unit(new Identity((uint)invoiceItem.UnitID), null),
                            new Price(invoiceItem.Price),
                            new Note(invoiceItem.Note),
                            new Quantity(invoiceItem.Quantity),
                            new Weight(invoiceItem.Weight),
                            new Price(invoiceItem.TotalPrice),
                            new Deliveried(false),
                            new Quantity(0)
                        );
                    items.Add(item);
                }
                Entities.Invoice Invoice = new Entities.Invoice
                (
                    new Identity((uint)command.ID),
                    new Entities.Customer((uint)command.CustomerID, null, null, null, null, null, null, null, null),
                    new DeliveryTime(command.DeliveryTime),
                    new Price(command.TotalPrice),
                    new Note(command.Note),
                    new Weight(command.WeightTotal),
                    new Code(command.Code),
                    items
                );
                bool result = _InvoiceRepository.Update(Invoice);
                return Task.FromResult(result as object);
            }
            return Task.FromResult(null as object);
        }
    }
}
