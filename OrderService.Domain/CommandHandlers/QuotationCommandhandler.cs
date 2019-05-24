using MediatR;
using Newtonsoft.Json;
using OrderService.Domain.Bus;
using OrderService.Domain.CommandHandlers;
using OrderService.Domain.Commands.Quotation;
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
    public class QuotationCommandhandler :   BaseCommandHandler
                                        , IRequestHandler<AddNewQuotationCommand, object>
    {

        private readonly IQuotationRepository _QuotationRepository;
        private readonly IMediatorHandler _bus;

        public QuotationCommandhandler(IQuotationRepository QuotationRepository,
                                      IMediatorHandler bus,
                                      INotificationHandler<DomainNotification> notifications) : base(bus, notifications)
        {
            _QuotationRepository = QuotationRepository;
            _bus = bus;
        }

        public Task<object> Handle(AddNewQuotationCommand command, CancellationToken cancellationToken)
        {
            Entities.Quotation u = new Entities.Quotation( 
                null,
                new Entities.Customer((uint)command.CustomerId,null,null,null,null,null,null,null,null),
                new StartDate(command.StartDate),
                null
                );
            if (!command.IsValid())
            {
                NotifyValidationErrors(command);
            }
            else
            {
                QuotationModel model = _QuotationRepository.Add(u);
                if (model != null)
                {
                    return Task.FromResult(model as object);
                }
                _bus.RaiseEvent(new DomainNotification("Quotation", "Server error", NotificationCode.Error));

            }
            return Task.FromResult(null as object);
        }
        
    }
}
