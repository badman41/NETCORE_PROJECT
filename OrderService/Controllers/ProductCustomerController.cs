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
    public class ProductCustomerController : ControllerBase
    {
        private IProductAppService _ProductAppService;
        public ProductCustomerController(IProductAppService ProductAppService)
        {
            _ProductAppService = ProductAppService;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var accesToken = Request.Headers["Authorization"];
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(accesToken) as JwtSecurityToken;
            var id = Int32.Parse(jsonToken.Claims.First().Value);

            GetAllProductByCustomerRequest request = new GetAllProductByCustomerRequest(id, 0, 0);
            var result = await _ProductAppService.getAllProductByCustomer(request);
            return Ok(result);
        }
        // GET: orderApi/Product
        [HttpGet("{id}", Name = "GetProductByCustomer")]
        public async Task<IActionResult> GetProductByCustomer(int id)
        {
            GetAllProductByCustomerRequest request = new GetAllProductByCustomerRequest(id,0,0);
            var result = await _ProductAppService.getAllProductByCustomer(request);
            return Ok(result);
        }
        
    }
}