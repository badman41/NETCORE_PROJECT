using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Application.Request
{
    public class GetAllProductByCustomerRequest
    {
        public int Id { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }

        public GetAllProductByCustomerRequest() { }
        public GetAllProductByCustomerRequest(int id, int page, int pageSize = 10)
        {
            Id = id;
            Page = page;
            PageSize = pageSize;
        }
    }
}
