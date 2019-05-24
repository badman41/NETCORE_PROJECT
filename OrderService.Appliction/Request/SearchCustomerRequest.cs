using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Application.Request
{
    public class SearchCustomerRequest
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string PhoneNumber { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }

        public SearchCustomerRequest() { }
        public SearchCustomerRequest(string name, string code, string phoneNumber, int page, int pageSize = 10)
        {
            Name = name;
            Code = code;
            PhoneNumber = phoneNumber;
            Page = page;
            PageSize = pageSize;
        }
    }
}
