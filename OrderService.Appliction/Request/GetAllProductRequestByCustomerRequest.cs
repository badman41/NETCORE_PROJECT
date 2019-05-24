using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Application.Request
{
    public class GetAllProductRequestByCustomerRequest
    {
        public int user_id { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }

        public GetAllProductRequestByCustomerRequest() { }
        public GetAllProductRequestByCustomerRequest(int userId, int page, int pageSize = 10)
        {
            user_id = userId;
            Page = page;
            PageSize = pageSize;
        }
    }
}
