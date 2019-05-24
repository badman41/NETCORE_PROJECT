using OrderService.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Application.Response
{
    public class GetProductResponse
    {
        public string Message { get; set; }
        public bool Success { get; set; }
        public DataProductResponse Data { get; set; }
        public GetProductResponse() { }
    }
}
