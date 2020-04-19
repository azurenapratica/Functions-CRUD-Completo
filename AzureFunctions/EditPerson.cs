using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace AzureFunctions
{
    public static class EditPerson
    {
        [FunctionName("EditPerson")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = null)]HttpRequest req,
            [Queue("EditPerson", Connection = "AzureWebJobsStorage")]IAsyncCollector<string> queueItem,
            ILogger log)
        {
            log.LogInformation("EditPerson function started a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            await queueItem.AddAsync(requestBody);

            log.LogInformation("EditPerson function finished a request.");

            return new OkObjectResult($"Obrigado! Seu registro já esta sendo processado.");
        }
    }
}
