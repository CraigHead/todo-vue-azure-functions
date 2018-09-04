using System;
using System.Collections.Generic;
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
    public static class CreateTodoList
    {
        [FunctionName("CreateTodoList")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("CreateTodoList Invoked.");

            string body = await req.Content.ReadAsStringAsync();
            ITodoList<IEnumerable<Item>> newList = JsonConvert.DeserializeObject<TodoList>(body);

            log.Info($"CreateTodoList Name: {newList.Name}.");

            // Define the row,
            string newItemGuid = Guid.NewGuid().ToString();

            // Create the Entity and set the partition to signup, 
            TodoListEntity listEntity = new TodoListEntity("todovue", newItemGuid);

            listEntity.Id = newItemGuid;
            listEntity.Name = newList.Name;
            listEntity.Items = JsonConvert.SerializeObject(new List<Item>());

            TodoListTableStorage storage = new TodoListTableStorage();
            CloudTable table = storage.GetCloudTableReference();

            TableOperation insertOperation = TableOperation.Insert(listEntity);

            await table.ExecuteAsync(insertOperation);
            newList.Id = newItemGuid;

            return req.CreateResponse(HttpStatusCode.OK, newList);
        }

    }
}
