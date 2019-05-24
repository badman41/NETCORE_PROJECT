using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Application.Request
{
    public class SearchQuotationRequest
    {
        public string Code { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }

        public SearchQuotationRequest() { }
        public SearchQuotationRequest(string code, int page, int pageSize = 1)
        {
            Code = code;
            Page = page;
            PageSize = pageSize;
        }
    }
}
