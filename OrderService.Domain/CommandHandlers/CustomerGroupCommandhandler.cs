using MediatR;
using Newtonsoft.Json;
using OrderService.Domain.Bus;
using OrderService.Domain.CommandHandlers;
using OrderService.Domain.Commands.CustomerGroup;
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

namespace OrderService.Domain.CustomerGroup.CommandHandlers
{
    public class CustomerGroupCommandhandler :   BaseCommandHandler
                                        , IRequestHandler<AddNewCustomerGroupCommand, object>
                                        , IRequestHandler<DeleteCustomerGroupCommand, object>
    {

        private readonly ICustomerGroupRepository _CustomerGroupRepository;
        private readonly IMediatorHandler _bus;

        public CustomerGroupCommandhandler(ICustomerGroupRepository CustomerGroupRepository,
                                      IMediatorHandler bus,
                                      INotificationHandler<DomainNotification> notifications) : base(bus, notifications)
        {
            _CustomerGroupRepository = CustomerGroupRepository;
            _bus = bus;
        }

        public Task<object> Handle(AddNewCustomerGroupCommand command, CancellationToken cancellationToken)
        {
            Entities.CustomerGroup u = new Entities.CustomerGroup(null, new Name(command.Name));
            if (!command.IsValid())
            {
                NotifyValidationErrors(command);
            }
            else
            {
                CustomerGroupModel model = _CustomerGroupRepository.Add(u);
                if (model != null)
                {
                    return Task.FromResult(model as object);
                }
                _bus.RaiseEvent(new DomainNotification("CustomerGroup", "Server error", NotificationCode.Error));

            }
            return Task.FromResult(null as object);
        }

        public Task<object> Handle(DeleteCustomerGroupCommand command, CancellationToken cancellationToken)
        {
            if (!command.IsValid(_CustomerGroupRepository))
            {
                NotifyValidationErrors(command);
                return Task.FromResult(false as object);
            }
            else
            {
                bool result = _CustomerGroupRepository.Delete(command.Id);
                return Task.FromResult(true as object);
            }
            
        }
    }
}
