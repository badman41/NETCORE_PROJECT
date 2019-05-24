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
    public class UnitController : ControllerBase
    {
        private IUnitAppService _unitAppService;
        public UnitController(IUnitAppService unitAppService)
        {
            _unitAppService = unitAppService;
        }
        // GET: orderApi/Unit
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result =  await _unitAppService.getAllUnit();
            return Ok(result);
        }

        // GET: orrderApi/Unit/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: orrderApi/Unit
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AddNewUnitRequest request)
        {
            AddNewUnitResponse result = await _unitAppService.addNewUnit(request);
            if (result.Success)
            {
                return Ok(result);
            }
            return  NotFound(result);
        }

        // PUT: api/Unit/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            DeleteUnitRequest request = new DeleteUnitRequest(id);
            DeleteUnitResponse result = await _unitAppService.deleteUnit(request);
            if (result.Success)
            {
                return Ok(result);
            }
            return NotFound(result);
        }
    }
}
