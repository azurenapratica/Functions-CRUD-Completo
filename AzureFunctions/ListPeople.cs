using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;

namespace AzureFunctions
{
    public static class ListPeople
    {
        [FunctionName("ListPeople")]
        public static async Task<IEnumerable<Person>> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            [Table("Person")]CloudTable cloudTable,
            ILogger log)
        {
            log.LogInformation("ListPeople function started a request.");

            var tableQuery = new TableQuery<Person>();
            TableContinuationToken continuationToken = null;

            TableQuerySegment<Person> tableQueryResult;
            do
            {
                tableQueryResult = await cloudTable.ExecuteQuerySegmentedAsync(tableQuery, continuationToken);
                continuationToken = tableQueryResult.ContinuationToken;
            } while (continuationToken != null);

            log.LogInformation("ListPeople function finished a request.");

            return tableQueryResult.Results;
        }
    }
}
