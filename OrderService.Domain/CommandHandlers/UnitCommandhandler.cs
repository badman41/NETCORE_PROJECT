using MediatR;
using Newtonsoft.Json;
using OrderService.Domain.Bus;
using OrderService.Domain.CommandHandlers;
using OrderService.Domain.Commands.Unit;
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

namespace OrderService.Domain.Unit.CommandHandlers
{
    public class UnitCommandhandler :   BaseCommandHandler
                                        , IRequestHandler<AddNewUnitCommand, object>
                                        , IRequestHandler<DeleteUnitCommand, object>
    {

        private readonly IUnitRepository _unitRepository;
        private readonly IMediatorHandler _bus;

        public UnitCommandhandler(IUnitRepository unitRepository,
                                      IMediatorHandler bus,
                                      INotificationHandler<DomainNotification> notifications) : base(bus, notifications)
        {
            _unitRepository = unitRepository;
            _bus = bus;
        }

        public Task<object> Handle(AddNewUnitCommand command, CancellationToken cancellationToken)
        {
            Entities.Unit u = new Entities.Unit(null, new Name(command.Name));
            if (!command.IsValid())
            {
                NotifyValidationErrors(command);
            }
            else
            {
                UnitModel model = _unitRepository.Add(u);
                if (model != null)
                {
                    return Task.FromResult(model as object);
                }
                _bus.RaiseEvent(new DomainNotification("Unit", "Server error", NotificationCode.Error));

            }
            return Task.FromResult(null as object);
        }

        public Task<object> Handle(DeleteUnitCommand command, CancellationToken cancellationToken)
        {
            if (!command.IsValid(_unitRepository))
            {
                NotifyValidationErrors(command);
                return Task.FromResult(false as object);
            }
            else
            {
                bool result = _unitRepository.Delete(command.Id);
                return Task.FromResult(true as object);
            }
            
        }
    }
}
