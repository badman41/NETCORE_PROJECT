using MassTransit;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TransitionApp.Models.VehicleXXX;

namespace OrderService.Application.Implements
{
   public class AbcHandler : IConsumer<Abc>
    {
        public Task Consume(ConsumeContext<Abc> context)
        {
            Console.WriteLine($"Receive message value: {JsonConvert.SerializeObject(context.Message)}");
            return Task.CompletedTask;
        }
    }
}
