using OrderService.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Application.Response
{
    public class GetAllCustomerResponse : BaseGetResponse<CustomerModel>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public GetAllCustomerResponse() { }
        public GetAllCustomerResponse(StatusResponse status,int total, IEnumerable<CustomerModel> data) 
            : base(status, total, data) { }
    }
}
