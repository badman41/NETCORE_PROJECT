using System;
using OrderService.Domain.Event;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace OrderService.Domain.Notifications
{
    public class DomainNotification : OrderService.Domain.Event.Event
    {
        public Guid DomainNotificationId { get; private set; }
        public string Key { get; private set; }
        public Object request { get; private set; }
        public int Version { get; private set; }
        public NotificationCode ResultCode { get; private set; }

        public DomainNotification(string key, Object request, NotificationCode resultCode)
        {
            DomainNotificationId = Guid.NewGuid();
            Version = 1;
            Key = key;
            request = request;
            ResultCode = resultCode;
        }
    }
    public enum NotificationCode:int
    {
        [Display(Name="Successfully")]
        Success = 1,
        [Display(Name = "Error")]
        Error = 2,
    }
}
