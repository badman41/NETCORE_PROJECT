using OrderService.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Application.Response
{
    public class GetListOrderedCustomerResponse
    {
        public IEnumerable<OrderedCustomerModel> a;
    }
}
