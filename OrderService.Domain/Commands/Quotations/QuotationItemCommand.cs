using OrderService.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.Commands.Quotation
{
    public abstract class QuotationItemCommand: BaseCommand
    {
        public int Id { get; protected set; }
        public int QuotationId { get; protected set; }
        public int UnitId { get; protected set; }
        public string ProductCode { get; protected set; }
        public int Price { get; protected set; }
    }
}
