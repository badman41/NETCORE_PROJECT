using MediatR;
using Newtonsoft.Json;
using OrderService.Domain.Bus;
using OrderService.Domain.CommandHandlers;
using OrderService.Domain.Commands.Preservation;
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

namespace OrderService.Domain.Preservation.CommandHandlers
{
    public class PreservationCommandhandler :   BaseCommandHandler
                                        , IRequestHandler<AddNewPreservationCommand, object>
                                        , IRequestHandler<DeletePreservationCommand, object>
    {

        private readonly IPreservationRepository _PreservationRepository;
        private readonly IMediatorHandler _bus;

        public PreservationCommandhandler(IPreservationRepository PreservationRepository,
                                      IMediatorHandler bus,
                                      INotificationHandler<DomainNotification> notifications) : base(bus, notifications)
        {
            _PreservationRepository = PreservationRepository;
            _bus = bus;
        }

        public Task<object> Handle(AddNewPreservationCommand command, CancellationToken cancellationToken)
        {
            Entities.Preservation u = new Entities.Preservation(null, new Description(command.Description));
            if (!command.IsValid())
            {
                NotifyValidationErrors(command);
            }
            else
            {
                PreservationModel model = _PreservationRepository.Add(u);
                if (model != null)
                {
                    return Task.FromResult(model as object);
                }
                _bus.RaiseEvent(new DomainNotification("Preservation", "Server error", NotificationCode.Error));

            }
            return Task.FromResult(null as object);
        }

        public Task<object> Handle(DeletePreservationCommand command, CancellationToken cancellationToken)
        {
            if (!command.IsValid(_PreservationRepository))
            {
                NotifyValidationErrors(command);
                return Task.FromResult(false as object);
            }
            else
            {
                bool result = _PreservationRepository.Delete(command.Id);
                return Task.FromResult(true as object);
            }
            
        }
    }
}
