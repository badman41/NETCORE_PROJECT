using OrderService.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Application.Response
{
    public class GetAllProductRequestResponse
    {
        public IEnumerable<ProductRequestModel> Data { get; set; }
        public int Total { get; set; }
        public GetAllProductRequestResponse() { }
    }
}
