using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace AzureFunctions
{
    public static class CreatePersonQueue
    {
        [FunctionName("CreatePersonQueue")]
        public static void Run(
            [QueueTrigger("CreatePerson", Connection = "AzureWebJobsStorage")]string queueItem, 
            [Table("Person")]CloudTable cloudTable, 
            ILogger log)
        {
            log.LogInformation($"CreatePersonQueue trigger function started.");

            var data = JsonConvert.DeserializeObject<Person>(queueItem);
            data.PartitionKey = "Person";
            data.RowKey = Guid.NewGuid().ToString();

            var tableOperation = TableOperation.Insert(data);
            cloudTable.ExecuteAsync(tableOperation);

            log.LogInformation($"CreatePersonQueue trigger function finished.");
        }
    }
}
