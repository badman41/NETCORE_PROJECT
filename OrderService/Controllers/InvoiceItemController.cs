using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Interfaces;
using OrderService.Application.Request;
using OrderService.Application.Response;
using OrderService.Domain.Interface.Respository;
using OrderService.Domain.Interface.Service;
using OrderService.Domain.Shared.ValueObject;

namespace OrderService.Controllers
{
    [Route("orderApi/[controller]")]
    [ApiController]
    public class InvoiceItemController : ControllerBase
    {
        private IInvoiceAppService _InvoiceAppService;
        private IInvoiceAppService _invoiceAppService;
        public InvoiceItemController(IInvoiceAppService InvoiceItemAppService, IInvoiceAppService invoiceAppService)
        {
            _InvoiceAppService = InvoiceItemAppService;
            _invoiceAppService = invoiceAppService;
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> CancleInvoice(int id)
        {
            var result = await _invoiceAppService.cancleInvoice(id);
            return Ok(result);
        }
        ////POST: orrderApi/Customer
        //[HttpPost("{id}")]
        //public async Task<IActionResult> Post(int id, [FromBody] AddNewInvoiceItemRequest request)
        //{
        //    request.InvoiceID = id;
        //    bool result = await _InvoiceItemAppService.addNewInvoice(request);
        //    if (result)
        //    {
        //        return Ok(result);
        //    }
        //    return NotFound(result);
        //}

        ////POST: orrderApi/Customer
        //[HttpPatch("{id}")]
        //public async Task<IActionResult> Put(int id, [FromBody] UpdateInvoiceItemRequest request)
        //{
        //    request.InvoiceID = id;
        //    bool result = await _InvoiceItemAppService.updateInvoice(request);
        //    if (result)
        //    {
        //        return Ok(result);
        //    }
        //    return NotFound(result);
        //}
    }
}