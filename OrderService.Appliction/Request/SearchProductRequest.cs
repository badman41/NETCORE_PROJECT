using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Application.Request
{
    public class SearchProductRequest
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int StoreCondition { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }

        public SearchProductRequest() { }
        public SearchProductRequest(string name, string code, int page, int pageSize = 10)
        {
            Name = name;
            Code = code;
            Page = page;
            PageSize = pageSize;
        }
    }
}
