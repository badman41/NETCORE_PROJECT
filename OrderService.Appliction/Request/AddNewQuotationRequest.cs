using OrderService.Domain.ReadModels;
using System.ComponentModel.DataAnnotations;
using System;

namespace OrderService.Application.Request
{
    public class AddNewQuotationRequest
    {
        public string customer_code { get; set; }
        public DateTime date { get; set; }
    }
}
