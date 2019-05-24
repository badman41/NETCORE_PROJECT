using OrderService.Domain.Entities;
using OrderService.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Application.Response
{
    public class AddNewCustomerResponse : BaseResponse
    {
        public int Data { get; set; }
    }
}
