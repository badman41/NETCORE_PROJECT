using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Domain.ReadModels
{
    public class PagedModel
    {
        public IEnumerable<object> Data { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int PageTotal { get; set; }
    }
}
