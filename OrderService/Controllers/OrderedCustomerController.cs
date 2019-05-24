using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Interfaces;
using OrderService.Application.Request;
using OrderService.Application.Response;
using OrderService.Domain.Shared.ValueObject;

namespace OrderService.Controllers
{
    [Route("orderApi/[controller]")]
    [ApiController]
    public class OrderedCustomerController : ControllerBase
    {
        private ICustomerAppService _customerAppService;
        public OrderedCustomerController(ICustomerAppService CustomerAppService)
        {
            _customerAppService = CustomerAppService;
        }
        // GET: orderApi/Product
        [HttpGet(Name = "GetOrderedCustomer")]
        public async Task<IActionResult> GetOrderedCustomer(DateTime date)
        {
            var result = await _customerAppService.getListOrderedCustomer(date);
            return Ok(result);
        }
        
    }
}