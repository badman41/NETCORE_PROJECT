using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Application.Request
{
    public class SearchAccountRequest
    {
        public string Name { get; set; }
        public int? Role { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }

        public SearchAccountRequest() { }
    }
}
