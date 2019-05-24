using MassTransit;
using MediatR;
using Newtonsoft.Json;
using OrderService.Domain.Bus;
using OrderService.Domain.Commands.Unit;
using OrderService.Domain.Interface.Respository;
using OrderService.Domain.Shared.ValueObject;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OrderService.Domain.CommandHandlers
{
    public class UnitCommandhandlerMQ : IConsumer<AddNewUnitCommand>
    {
        public Task Consume(ConsumeContext<AddNewUnitCommand> context)
        {
            Console.WriteLine($"Receive message request: {JsonConvert.SerializeObject( context.Message)}");
            return Task.CompletedTask;
        }
    }
}
