using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage.Table;
using Todo.Vue.Functions.Storage;

namespace Todo.Vue.Functions
{
    // ReSharper disable once UnusedMember.Global
    public static class DeleteTodoList
    {
        [FunctionName("DeleteTodoList")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "Delete", Route = null)]HttpRequestMessage req, TraceWriter log)
        {

            log.Info("DeleteTodoList Invoked.");


            string id = req.GetQueryNameValuePairs()
                .First(q => string.Compare(q.Key, "id", StringComparison.OrdinalIgnoreCase) == 0)
                .Value;


            log.Info($"DeleteTodoList Id: {id}.");

            TableEntity item = new TableEntity("todovue", id)
            {
                ETag = "*" //By default, the SDK enforces optimistic concurrency via ETags. Set this value to '*' in order to force an overwrite to an entity as part of an update operation.
            };

            TableOperation operation = TableOperation.Delete(item);

            TodoListTableStorage storage = new TodoListTableStorage();
            CloudTable table = storage.GetCloudTableReference();

            await table.ExecuteAsync(operation);

            return req.CreateResponse(HttpStatusCode.NoContent);
        }

    }
}
