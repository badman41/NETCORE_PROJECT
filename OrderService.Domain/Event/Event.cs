using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.Event
{
    public abstract class Event :  INotification
    {
        public DateTime Timestamp { get; private set; }

        protected Event()
        {
            Timestamp = DateTime.Now;
        }
    }
}
