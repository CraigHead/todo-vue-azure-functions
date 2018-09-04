using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using Todo.Vue.Functions.Models;
using Todo.Vue.Functions.Storage;

namespace Todo.Vue.Functions
{
    // ReSharper disable once UnusedMember.Global
    public static class UpdateTodoList
    {
        [FunctionName("UpdateTodoList")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "PUT", Route = null)]HttpRequestMessage req, TraceWriter log)
        {

            log.Info("UpdateTodoList Invoked.");
            
            string body = await req.Content.ReadAsStringAsync();
            ITodoList<IEnumerable<Item>> inputList = JsonConvert.DeserializeObject<TodoList>(body);

            log.Info($"UpdateTodoList Id: {inputList.Name}.");


            TodoListEntity updateEntity = new TodoListEntity("todovue", inputList.Id);

            updateEntity.Items = JsonConvert.SerializeObject(inputList.Items);
            updateEntity.ETag = "*"; //By default, the SDK enforces optimistic concurrency via ETags. Set this value to '*' in order to force an overwrite to an entity as part of an update operation.

            TableOperation operation = TableOperation.Replace(updateEntity);

            TodoListTableStorage storage = new TodoListTableStorage();
            CloudTable table = storage.GetCloudTableReference();
            await table.ExecuteAsync(operation);

            return req.CreateResponse(HttpStatusCode.NoContent);
        }

    }
}
