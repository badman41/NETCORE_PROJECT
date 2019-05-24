using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Application.Request
{
    public class SearchInvoiceRequest
    {
        public int id { get; set; }
        public string customer_code { get; set; }
        public int customer_id { get; set; }
        public DateTime? from_time { get; set; }
        public int page { get; set; }
        public int page_size { get; set; }

        public SearchInvoiceRequest() { }
        public SearchInvoiceRequest(string code,int id, DateTime? date, int page, int pageSize = 1)
        {
            customer_code = code;
            customer_id = id;
            from_time = date;
            page = page;
            page_size = pageSize;
        }
    }
}
