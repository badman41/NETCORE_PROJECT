using MassTransit;
using Newtonsoft.Json;
using OrderService.Domain.Commands.Invoices;
using OrderService.Domain.Interface.Respository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TransitionApp.Domain.ReadModel.Invoice;

namespace OrderService.Domain.CommandHandlers
{
   public class UpdateInvoiceConsumer : IConsumer<InvoiceReadModel>
    {
        private readonly IInvoiceRepository _invoiceRepository;
        public UpdateInvoiceConsumer(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }
        public Task Consume(ConsumeContext<InvoiceReadModel> context)
        {
            Console.WriteLine($"Receive message value: {JsonConvert.SerializeObject(context.Message)}");
            _invoiceRepository.UpdateStatus(context.Message.Code, context.Message.Status, context.Message.Served);
            return Task.CompletedTask;
        }
    }
}
