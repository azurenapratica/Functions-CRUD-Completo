using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace AzureFunctions
{
    public static class EditPersonQueue
    {
        [FunctionName("EditPersonQueue")]
        public static void Run(
            [QueueTrigger("EditPerson", Connection = "AzureWebJobsStorage")]string queueItem,
            [Table("Person")]CloudTable cloudTable,
            ILogger log)
        {
            log.LogInformation($"EditPersonQueue trigger function started.");

            var data = JsonConvert.DeserializeObject<Person>(queueItem);

            var tableOperation = TableOperation.InsertOrReplace(data);
            cloudTable.ExecuteAsync(tableOperation);

            log.LogInformation($"EditPersonQueue trigger function finished.");
        }
    }
}
