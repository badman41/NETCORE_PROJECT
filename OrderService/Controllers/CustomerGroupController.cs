using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Interfaces;
using OrderService.Application.Request;
using OrderService.Application.Response;

namespace OrderService.Controllers
{
    [Route("orderApi/[controller]")]
    [ApiController]
    public class CustomerGroupController : ControllerBase
    {
        private ICustomerGroupAppService _CustomerGroupAppService;
        public CustomerGroupController(ICustomerGroupAppService CustomerGroupAppService)
        {
            _CustomerGroupAppService = CustomerGroupAppService;
        }
        // GET: orderApi/CustomerGroup
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _CustomerGroupAppService.getAllCustomerGroup();
            return Ok(result);
        }

        // GET: orrderApi/CustomerGroup/5
        [HttpGet("{id}", Name = "GetCustomerGroup")]
        public async Task<IActionResult> Get(int id, [FromQuery] GetCustomerByGroupRequest request)
        {
            request.CustomerGroupId = id;
            var result = await _CustomerGroupAppService.getCustomerGroupById(request);
            return Ok(result);
        }

        // POST: orrderApi/CustomerGroup
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AddNewCustomerGroupRequest request)
        {
            AddNewCustomerGroupResponse result = await _CustomerGroupAppService.addNewCustomerGroup(request);
            if (result.Success)
            {
                return Ok(result);
            }
            return NotFound(result);
        }

        // POST: orrderApi/CustomerGroup
        [HttpPost]
        [Route("[action]/{id}")]
        public async Task<IActionResult> AddCustomerToGroup(int id, [FromBody] AddCustomerToGroupRequest request)
        {
            request.customer_group_id = id;
            AddCustomerToGroupResponse result = await _CustomerGroupAppService.addCustomerToGroup(request);
            if (result.Success)
            {
                return Ok(result);
            }
            return NotFound(result);
        }
        // PUT: api/CustomerGroup/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            DeleteCustomerGroupRequest request = new DeleteCustomerGroupRequest(id);
            DeleteCustomerGroupResponse result = await _CustomerGroupAppService.deleteCustomerGroup(request);
            if (result.Success)
            {
                return Ok(result);
            }
            return NotFound(result);
        }
    }
}
