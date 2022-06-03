using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using AzureFunctionsTech.Api.Core.TimeEntries;
using MediatR;
using AzureFunctionsTech.Api.Models;
using AzureFunctionsTech.Api.Extensions;
using AzureFunctionsTech.Api.Core.Command;
using Newtonsoft.Json;

namespace AzureFunctionsTech.Api.Functions
{
    public class TimeEntriesFunction
    {
        private readonly IMediator _mediator;

        public TimeEntriesFunction(IMediator mediator)
        {
            _mediator = mediator;
        }

        [FunctionName("TimeEntriesFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            CommandResult<int> result;
            try
            {
                var model = await req.Body.DeserializeToObjectAsync<TimeEntriesRequestModel>();
                result = await _mediator.Send(new CreateTimeEntriesCommand(model.StartOn, model.EndOn));
            }
            catch (JsonReaderException ex)
            {
                return new BadRequestObjectResult($"Invalid {ex.Path} value");
            }

            if (result.IsSuccess)
            {
                return new OkObjectResult(result.Value);
            }

            return new BadRequestObjectResult(result.Description);
        }
    }
}
