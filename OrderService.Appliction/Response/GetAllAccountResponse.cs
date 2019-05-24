using OrderService.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Application.Response
{
    public class GetAllAccountResponse
    {
        public IEnumerable<AccountModel> Data { get; set; }
        public Metadata Metadata { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
        public GetAllAccountResponse() { }
    }
}
