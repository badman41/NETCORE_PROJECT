using OrderService.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Application.Response
{
    public class GetInvoiceResponse
    {
        public InvoiceModel Data { get; set; }
        public int Total { get; set; }
    }
}
