using OrderService.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderService.Application.Response
{
    public class GetAllPreservationResponse
    {
        public IEnumerable<PreservationModel> Data { get; set; }
        public StatusResponse Status { get; set; }
        public int Total { get; set; }
    }
}
