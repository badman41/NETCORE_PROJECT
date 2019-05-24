using MediatR;
using Newtonsoft.Json;
using OrderService.Domain.Bus;
using OrderService.Domain.CommandHandlers;
using OrderService.Domain.Commands.ProductRequest;
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

namespace OrderService.Domain.ProductRequest.CommandHandlers
{
    public class ProductRequestCommandhandler :   BaseCommandHandler
                                        , IRequestHandler<AddNewProductRequestCommand, object>
                                        , IRequestHandler<UpdateProductRequestCommand, object>
                                        , IRequestHandler<DeleteProductRequestCommand, object>
    {

        private readonly IProductRequestRepository _ProductRequestRepository;
        private readonly IMediatorHandler _bus;

        public ProductRequestCommandhandler(IProductRequestRepository ProductRequestRepository,
                                      IMediatorHandler bus,
                                      INotificationHandler<DomainNotification> notifications) : base(bus, notifications)
        {
            _ProductRequestRepository = ProductRequestRepository;
            _bus = bus;
        }

        public Task<object> Handle(AddNewProductRequestCommand command, CancellationToken cancellationToken)
        {
            if (!command.IsValid())
            {
                NotifyValidationErrors(command);
            }
            else
            {
                Entities.ProductRequest ProductRequest = new Entities.ProductRequest
                (
                    null,
                    new Name(command.ProductName),
                    new Description(command.Description),
                    new Quantity(command.Quantity),
                    new Status(command.Status),
                    new Response(command.Response),
                    new Entities.Customer(new Identity((uint)command.UserId),null,null,null,null,null,null,null,null)
                );
                ProductRequestModel model = _ProductRequestRepository.Add(ProductRequest);
                if (model != null)
                {
                    return Task.FromResult(model as object);
                }
                _bus.RaiseEvent(new DomainNotification("ProductRequest", "Server error", NotificationCode.Error));

            }
            return Task.FromResult(null as object);
        }

        public Task<object> Handle(UpdateProductRequestCommand command, CancellationToken cancellationToken)
        {
            if (!command.IsValid())
            {
                NotifyValidationErrors(command);
            }
            else
            {
                Entities.ProductRequest ProductRequest = new Entities.ProductRequest
                (
                    new Identity((uint)command.Id),
                    null,
                    null,
                    null,
                    new Status(command.Status),
                    new Response(command.Response),
                    null
                );
                bool result = _ProductRequestRepository.Update(ProductRequest);
                if (!result)
                {
                    _bus.RaiseEvent(new DomainNotification("ProductRequest", "Server error", NotificationCode.Error));
                }
                return Task.FromResult(result as object);

            }
            return Task.FromResult(null as object);
        }

        public Task<object> Handle(DeleteProductRequestCommand command, CancellationToken cancellationToken)
        {
            if (!command.IsValid())
            {
                NotifyValidationErrors(command);
            }
            else
            {
                bool result = _ProductRequestRepository.Delete(command.Id);
                return Task.FromResult(result as object);
            }
            return Task.FromResult(false as object);
        }
    }
}
