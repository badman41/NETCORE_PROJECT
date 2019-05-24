using OrderService.Application.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Application.Response
{
    public abstract class BaseResponse
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }

        public BaseResponse()
        {
            Status = (int)StatusCode.Error;
            Message = "Error";
        }
    }
}
