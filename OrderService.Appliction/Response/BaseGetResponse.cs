using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Application.Response
{
    public abstract class BaseGetResponse<T>
    {
        public StatusResponse Status { get; set; }
        public int Total { get; set; }
        public IEnumerable<T> Data { get; set; }
        public BaseGetResponse() { }
        public BaseGetResponse(StatusResponse status, int total, IEnumerable<T> data)
        {
            Status = status;
            Total = total;
            Data = data;
        }
    }
    public class StatusResponse
    {
        public string Content { get; set; }
        public bool OK { get; set; }

        public StatusResponse(string content,bool ok)
        {
            Content = content;
            OK = ok;
        }
    }
}
