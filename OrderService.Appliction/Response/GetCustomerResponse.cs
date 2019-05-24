using OrderService.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Application.Response
{
    public class GetCustomerResponse
    {
        public StatusResponse Status { get; set; }
        public CustomerModel Data { get; set; }
        public GetCustomerResponse() { }
        public GetCustomerResponse(StatusResponse status, CustomerModel data)
        {
            Status = status;
            Data = data;
        }
    }
}
