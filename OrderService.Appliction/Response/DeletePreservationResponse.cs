using OrderService.Domain.Entities;
using OrderService.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Application.Response
{
    public class DeletePreservationResponse : BaseResponse
    {
        public int Content { get; set; }
        public bool OK { get; set; }
    }
}
