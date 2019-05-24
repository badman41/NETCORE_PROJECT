using System;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OrderService.Model.Schemas;

namespace OrderService.Controllers
{
    [Route("graphql")]
    //[Route("orderApi/[controller]")]
    [ApiController]
    public class GraphController : ControllerBase
    {
        private readonly IDocumentExecuter _documentExecuter;
        private readonly ISchema _schema;
        private readonly ILogger _logger;

        public GraphController(IDocumentExecuter documentExecuter, ISchema schema, ILogger<GraphController> logger)
        {
            _documentExecuter = documentExecuter;
            _schema = schema;
            _logger = logger;
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GraphQLQuery query)
        {
            if (query == null) { throw new ArgumentNullException(nameof(query)); }

            var inputs = query.Variables.ToInputs();
            var queryToExecute = query.Query;


            var executionOptions = new ExecutionOptions { Schema = _schema, Query = queryToExecute, Inputs = inputs, OperationName = query.OperationName };

            var result = await _documentExecuter.ExecuteAsync(executionOptions).ConfigureAwait(false);


            if (result.Errors?.Count > 0)
            {
                _logger.LogError("GraphQL errors: {0}", result.Errors);
                return BadRequest(result);
            }
            _logger.LogDebug("GraphQL execution result: {result}", JsonConvert.SerializeObject(result.Data));
            return Ok(result);
        }
    }
}