using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
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
            log.Info("C# HTTP trigger function processed a request.");

            string body = await req.Content.ReadAsStringAsync();
            ITodoList newList = JsonConvert.DeserializeObject<TodoList>(body);

            // Define the row,
            string newItemGuid = Guid.NewGuid().ToString();

            // Create the Entity and set the partition to signup, 
            TodoListEntity listEntity = new TodoListEntity("todovue", newItemGuid);

            listEntity.Id = newItemGuid;
            listEntity.Name = newList.Name;
            listEntity.Items = new List<Item>();

            TodoListTableStorage storage = new TodoListTableStorage();
            CloudTable table = storage.GetCloudTableReference();

            TableOperation insertOperation = TableOperation.Insert(listEntity);

            table.Execute(insertOperation);
            newList.Id = newItemGuid;

            return req.CreateResponse(HttpStatusCode.OK, newList);
        }

    }
}
