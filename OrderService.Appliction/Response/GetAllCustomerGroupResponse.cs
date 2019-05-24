using OrderService.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Application.Response
{
    public class GetAllCustomerGroupResponse
    {
        public IEnumerable<CustomerGroupModel> Data { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
        public int Total { get; set; }
    }
}
