using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Application.Request
{
    public class GetCustomerByGroupRequest
    {
        public int CustomerGroupId { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }

        public GetCustomerByGroupRequest() { }
        public GetCustomerByGroupRequest(int customerGroupId, int page, int pageSize = 10)
        {
            CustomerGroupId = customerGroupId;
            Page = page;
            PageSize = pageSize;
        }
    }
}
