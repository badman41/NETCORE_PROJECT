using OrderService.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Application.Response
{
    public class GetAllProductResponse
    {
        public IEnumerable<DataProductResponse> Data { get; set; }
        public Metadata Metadata { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
        public GetAllProductResponse() { }
    }
    public class Metadata
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
    }
    public class DataProductResponse
    {
        public DateTime CreatedAt { get; set; }
        public int ID { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ProductModel ProductInfo { get; set; }
    }
}
