using OrderService.Application.Response;
using OrderService.Domain.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrderService.Application.Shared
{
    public enum StatusCode : int
    {
        Success = 1,
        Error = 2
    }
    public enum StatusProductRequest : int
    {
        DangXuLy = 0,
        DaXuLy = 1
    }
    public class Common<TResponse> where TResponse : BaseResponse, new()
    {
        public static TResponse checkHasNotification(DomainNotificationHandler _notifications, TResponse response)
        {
            if (_notifications.HasNotifications())
            {
                if (_notifications.GetNotifications().Any(x => x.ResultCode == NotificationCode.Error))
                {
                    foreach (var item in _notifications.GetNotifications())
                    {
                        response.Message += item.request + "; ";
                    }
                }

                _notifications.Dispose();
                response.Success = false;
                response.Status = (int)StatusCode.Error;
            }
            else
            {
                response.Status = (int)StatusCode.Success;
                response.Message = StatusCode.Success.ToString();
                response.Success = true;
            }
            return response;
        }
    }
}
