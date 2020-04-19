using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace AzureFunctions
{
    public static class DeletePersonQueue
    {
        [FunctionName("DeletePersonQueue")]
        public static void Run(
            [QueueTrigger("DeletePerson", Connection = "AzureWebJobsStorage")]string queueItem,
            [Table("Person")]CloudTable cloudTable,
            ILogger log)
        {
            log.LogInformation($"DeletePersonQueue trigger function started.");

            var data = JsonConvert.DeserializeObject<Person>(queueItem);

            var person = new DynamicTableEntity(data?.PartitionKey, data?.RowKey);
            person.ETag = "*";

            var tableOperation = TableOperation.Delete(person);
            cloudTable.ExecuteAsync(tableOperation);

            log.LogInformation($"DeletePersonQueue trigger function finished.");
        }
    }
}
