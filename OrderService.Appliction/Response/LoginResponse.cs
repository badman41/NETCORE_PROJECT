using OrderService.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Application.Response
{
    public class LoginResponse
    {
        public string Message { get; set; }
        public bool Success { get; set; }
        public DataLoginResponse Data { get; set; }
    }
    public class DataLoginResponse
    {
        public string Token { get; set; }
        public AccountModel User { get; set; }
    }
}
