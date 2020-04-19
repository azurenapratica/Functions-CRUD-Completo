using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureFunctions
{
    public static class GetPerson
    {
        [FunctionName("GetPerson")]
        public static async Task<Person> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            [Table("Person")]CloudTable cloudTable,
            ILogger log)
        {
            log.LogInformation("GetPerson function started a request.");

            var partitionKey = "Person";
            var rowKey = req.Query["id"];

            TableOperation person = TableOperation.Retrieve<Person>(partitionKey, rowKey);
            TableResult result = await cloudTable.ExecuteAsync(person);

            log.LogInformation("GetPerson function finished a request.");

            return (Person)result.Result;
        }
    }
}
