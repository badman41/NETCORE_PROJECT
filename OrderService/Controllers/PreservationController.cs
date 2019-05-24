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
    public class PreservationController : ControllerBase
    {
        private IPreservationAppService _PreservationAppService;
        public PreservationController(IPreservationAppService PreservationAppService)
        {
            _PreservationAppService = PreservationAppService;
        }
        // GET: orderApi/Preservation
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result =  await _PreservationAppService.getAllPreservation();
            return Ok(result);
        }
        

        // POST: orrderApi/Preservation
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AddNewPreservationRequest request)
        {
            AddNewPreservationResponse result = await _PreservationAppService.addNewPreservation(request);
            if (result.Success)
            {
                return Ok(result);
            }
            return  NotFound(result);
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            DeletePreservationRequest request = new DeletePreservationRequest(id);
            DeletePreservationResponse result = await _PreservationAppService.deletePreservation(request);
            if (result.Success)
            {
                return Ok(result);
            }
            return NotFound(result);
        }
    }
}
