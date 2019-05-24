using MassTransit;
using MediatR;
using Newtonsoft.Json;
using OrderService.Domain.Bus;
using OrderService.Domain.Commands;
using OrderService.Domain.Event;
using OrderService.Domain.Notifications;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Infra.Data.Respository.Bus
{
    public class InMemoryBus : IMediatorHandler
    {
        private readonly IMediator _mediator;
        private readonly IBus _bus;

        public InMemoryBus( IMediator mediator, IBus bus)
        {
            _mediator = mediator;
            _bus = bus;
        }

        public Task RaiseEvent<T>(T @event) where T : Event
        {
            //if (!@event.MessageType.Equals("DomainNotification"))
            //    _eventStore?.Save(@event);

            return _mediator.Publish(@event);
        }

        public Task SendCommand<T>(T command) where T : BaseCommand
        {
            //rabbitmq
            //var factory = new ConnectionFactory() { HostName = "localhost" };
            //using (var connection = factory.CreateConnection())
            //{
            //    using (var channel = connection.CreateModel())
            //    {
            //        channel.QueueDeclare(queue: "unit",
            //                             durable: false,
            //                             exclusive: false,
            //                             autoDelete: false,
            //                             arguments: null);
            //        string jsonified = JsonConvert.SerializeObject(command);

            //        var body = Encoding.UTF8.GetBytes(jsonified);

            //        channel.BasicPublish(exchange: "",
            //                             routingKey: "unit",
            //                             basicProperties: null,
            //                             body: body);
            //        Console.WriteLine(" [x] Sent {0}", jsonified);
            //    }
            //}
            //return Task.FromResult(true);
            return _mediator.Send(command);
            //await _bus.Publish<T>(command);
        }
    }
}
