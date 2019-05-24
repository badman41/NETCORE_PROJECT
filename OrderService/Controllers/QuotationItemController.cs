using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Interfaces;
using OrderService.Application.Request;
using OrderService.Application.Response;
using OrderService.Domain.Interface.Service;
using OrderService.Domain.Shared.ValueObject;

namespace OrderService.Controllers
{
    [Route("orderApi/[controller]")]
    [ApiController]
    public class QuotationItemController : ControllerBase
    {
        private IQuotationItemAppService _QuotationItemAppService;
        public QuotationItemController(IQuotationItemAppService QuotationItemAppService)
        {
            _QuotationItemAppService = QuotationItemAppService;
        }
        

        //POST: orrderApi/Customer
        [HttpPost("{id}")]
        public async Task<IActionResult> Post(int id, [FromBody] AddNewQuotationItemRequest request)
        {
            request.QuotationID = id;
            bool result = await _QuotationItemAppService.addNewQuotation(request);
            if (result)
            {
                return Ok(result);
            }
            return NotFound(result);
        }

        //POST: orrderApi/Customer
        [HttpPatch("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateQuotationItemRequest request)
        {
            request.QuotationID = id;
            bool result = await _QuotationItemAppService.updateQuotation(request);
            if (result)
            {
                return Ok(result);
            }
            return NotFound(result);
        }
    }
}