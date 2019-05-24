using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Application.Request
{
    public class BaseGetRequest
    {
        public int Page { get; set; }
        public int PageSize { get; set; }

        public BaseGetRequest(int page, int pageSize = 10)
        {
            Page = page;
            PageSize = pageSize;
        }
    }
}
