using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Application.Request
{
    public class SearchProductRequestRequest
    {
        public int? status { get; set; }
        public int user_id { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }

        public SearchProductRequestRequest() { }
        public SearchProductRequestRequest(int? _status, int page, int pageSize = 10)
        {
            status = _status;
            Page = page;
            PageSize = pageSize;
        }
    }
}
