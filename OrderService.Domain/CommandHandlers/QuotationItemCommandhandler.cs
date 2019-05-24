using MediatR;
using Newtonsoft.Json;
using OrderService.Domain.Bus;
using OrderService.Domain.CommandHandlers;
using OrderService.Domain.Commands.Product;
using OrderService.Domain.Commands.Quotation;
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

namespace OrderService.Domain.Quotation.CommandHandlers
{
    public class QuotationItemCommandhandler :   BaseCommandHandler
                                        , IRequestHandler<AddNewQuotationItemCommand, object>
                                        , IRequestHandler<UpdateQuotationItemCommand, object>
                                        //, IRequestHandler<DeleteQuotationCommand, object>
    {

        private readonly IQuotationRepository _QuotationRepository;
        private readonly IQuotationItemRepository _QuotationItemRepository;
        private readonly IMediatorHandler _bus;

        public QuotationItemCommandhandler(IQuotationRepository QuotationRepository,
                                      IQuotationItemRepository QuotationItemRepository,
                                      IMediatorHandler bus,
                                      INotificationHandler<DomainNotification> notifications) : base(bus, notifications)
        {
            _QuotationRepository = QuotationRepository;
            _QuotationItemRepository = QuotationItemRepository;
            _bus = bus;
        }

        public Task<object> Handle(AddNewQuotationItemCommand command, CancellationToken cancellationToken)
        {
            if (!command.IsValid())
            {
                NotifyValidationErrors(command);
            }
            else
            {
                QuotationItem quotationItem = new QuotationItem
                    (
                        null,
                        new Entities.Quotation(new Identity((uint)command.QuotationId),null,null,null),
                        new Entities.Product(null,null,new Code(command.ProductCode),null,null,null,null,null),
                        new Entities.Unit(new Identity((uint)command.UnitId),null),
                        new Price(command.Price)
                    );
                QuotationItemModel quotationItemModel = _QuotationItemRepository.GetByProperties(quotationItem);
                bool result = false;
                if (quotationItemModel == null)
                {
                    quotationItemModel = _QuotationItemRepository.Add(quotationItem);
                    result = (quotationItemModel != null);
                }
                else
                {
                    result = _QuotationItemRepository.Update(quotationItem);
                }
                if (!result)
                {
                    _bus.RaiseEvent(new DomainNotification("Quotation", "Server error", NotificationCode.Error));
                }
                return Task.FromResult(result as object);
            }
            return Task.FromResult(false as object);
        }

        public Task<object> Handle(UpdateQuotationItemCommand command, CancellationToken cancellationToken)
        {
            if (!command.IsValid())
            {
                NotifyValidationErrors(command);
            }
            else
            {
                QuotationItem quotationItem = new QuotationItem
                    (
                        null,
                        new Entities.Quotation(new Identity((uint)command.QuotationId), null, null, null),
                        new Entities.Product(null, null, new Code(command.ProductCode), null, null, null, null,null),
                        new Entities.Unit(new Identity((uint)command.UnitId), null),
                        new Price(command.Price)
                    );
                QuotationItemModel quotationItemModel = _QuotationItemRepository.GetByProperties(quotationItem);
                bool result = false;
                if (quotationItemModel == null)
                {
                    result = false;
                }
                else
                {
                    result = _QuotationItemRepository.Update(quotationItem);
                }
                if (!result)
                {
                    _bus.RaiseEvent(new DomainNotification("Quotation", "Server error", NotificationCode.Error));
                }
                return Task.FromResult(result as object);

            }
            return Task.FromResult(false as object);
        }

        //public Task<object> Handle(DeleteQuotationCommand command, CancellationToken cancellationToken)
        //{
        //    if (!command.IsValid(_QuotationRepository))
        //    {
        //        NotifyValidationErrors(command);
        //        return Task.FromResult(false as object);
        //    }
        //    else
        //    {
        //        bool result = _QuotationRepository.Delete(command.Id);
        //        return Task.FromResult(true as object);
        //    }

        //}
    }
}
