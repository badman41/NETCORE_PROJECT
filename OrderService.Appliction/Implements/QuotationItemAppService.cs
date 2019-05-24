using MassTransit;
using MediatR;
using OrderService.Application.Interfaces;
using OrderService.Application.Request;
using OrderService.Application.Response;
using OrderService.Application.Shared;
using OrderService.Domain.Bus;
using OrderService.Domain.Interface.Service;
using OrderService.Domain.Notifications;
using System.Threading.Tasks;
using OrderService.Domain.Commands.Quotation;

namespace OrderService.Application.Implements
{
    public class QuotationItemAppService : IQuotationItemAppService
    {
        private readonly IQuotationService _service;
        private readonly IQuotationItemService _quotationItemService;
        private readonly IMediatorHandler Bus;
        private readonly DomainNotificationHandler _notifications;

        public QuotationItemAppService(IQuotationService QuotationService, IQuotationItemService QuotationItemService, IMediatorHandler bus
                                    , INotificationHandler<DomainNotification> notifications)
        {
            _service = QuotationService;
            _quotationItemService = QuotationItemService;
            Bus = bus;
            _notifications = (DomainNotificationHandler)notifications;

        }

        public Task<bool> addNewQuotation(AddNewQuotationItemRequest request)
        {
            AddNewQuotationItemCommand command = new AddNewQuotationItemCommand
            (
                request.QuotationID,
                request.UnitID,
                request.ProductCode,
                request.Price
            );
            Task<object> Quotation = (Task<object>)Bus.SendCommand(command);

            return Task.FromResult((bool)Quotation.Result);
        }
        public Task<bool> updateQuotation(UpdateQuotationItemRequest request)
        {
            UpdateQuotationItemCommand command = new UpdateQuotationItemCommand
            (
                request.QuotationID,
                request.UnitID,
                request.ProductCode,
                request.Price
            );
            Task<object> Quotation = (Task<object>)Bus.SendCommand(command);

            return Task.FromResult((bool)Quotation.Result);
        }
        
    }
}
