using MediatR;
using OrderService.Domain.Bus;
using OrderService.Domain.Commands;
using OrderService.Domain.Notifications;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.CommandHandlers
{
    public class BaseCommandHandler
    {
        private readonly IMediatorHandler _bus;
        private readonly DomainNotificationHandler _notifications;

        public BaseCommandHandler(IMediatorHandler bus, INotificationHandler<DomainNotification> notifications)
        {
            _notifications = (DomainNotificationHandler)notifications;
            _bus = bus;
        }

        protected void NotifyValidationErrors(BaseCommand message)
        {
            foreach (var error in message.ValidationResult.Errors)
            {
                _bus.RaiseEvent(new DomainNotification("Command Error (" + error.ErrorCode + ") :", error.ErrorMessage, NotificationCode.Error));
            }
        }
    }
}
