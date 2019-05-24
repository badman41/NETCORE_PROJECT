using OrderService.Domain.ReadModels;
using System.ComponentModel.DataAnnotations;
using System;

namespace OrderService.Application.Request
{
    public class AddNewQuotationByGroupRequest
    {
        public int group_id { get; set; }
        public DateTime date { get; set; }
    }
}
