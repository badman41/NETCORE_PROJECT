using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using OrderService.Application.Interfaces;
using OrderService.Application.Request;
using OrderService.Application.Response;
using OrderService.Common;
using OrderService.Domain.Interface.Respository;
using OrderService.Domain.ReadModels;
using OrderService.Domain.Shared.ValueObject;

namespace OrderService.Controllers
{
    [Route("orderApi/[controller]")]
    [ApiController]
    public class ProductRequestController : ControllerBase
    {
        private IProductRequestAppService _ProductRequestAppService;
        public ProductRequestController(IProductRequestAppService ProductRequestAppService)
        {
            _ProductRequestAppService = ProductRequestAppService;
        }
        // GET: orderApi/ProductRequest
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] SearchProductRequestRequest request)
        {
            if(request.user_id >0)
            {
                GetAllProductRequestByCustomerRequest customerRequest = new GetAllProductRequestByCustomerRequest()
                {
                    user_id = request.user_id,
                    Page = 0,
                    PageSize = 0
                };
                var results = await _ProductRequestAppService.getAllByCustomer(customerRequest);
                return Ok(results);
            }
            var result = await _ProductRequestAppService.getAllProductRequest(request);
            return Ok(result);
        }
        // GET: orderApi/Product
        //[HttpGet(Name = "GetByCustomer")]
        //[Route("[action]")]
        //public async Task<IActionResult> GetByCustomer([FromQuery] GetAllProductRequestByCustomerRequest request)
        //{
            
        //}
        //POST: orrderApi/Customer
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AddNewProductRequestRequest request)
        {
            var accesToken = Request.Headers["Authorization"];
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(accesToken) as JwtSecurityToken;
            var id = Int32.Parse(jsonToken.Claims.First().Value);
            request.userId = id;
            AddNewProductRequestResponse result = await _ProductRequestAppService.addNewProductRequest(request);
            if (result.Success)
            {
                return Ok(result);
            }
            return NotFound(result);
        }

        //POST: orrderApi/Customer
        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, [FromBody] UpdateProductRequestRequest request)
        {
            request.Id = id;
            UpdateProductRequestResponse result = await _ProductRequestAppService.updateProductRequest(request);
            if (result.Success)
            {
                return Ok(result);
            }
            return NotFound(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            DeleteProductRequestResponse result = await _ProductRequestAppService.deleteProductRequest(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return NotFound(result);
        }
    }
}