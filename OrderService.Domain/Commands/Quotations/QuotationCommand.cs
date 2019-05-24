using OrderService.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.Commands.Quotation
{
    public abstract class QuotationCommand: BaseCommand
    {
        public int Id { get; protected set; }
        public int CustomerId { get; protected set; }
        public int CustomerGroupId { get; protected set; }
        public string CustomerCode { get; protected set; }
        public DateTime StartDate { get; protected set; }
        public DateTime EndDate { get; protected set; }
    }
}
