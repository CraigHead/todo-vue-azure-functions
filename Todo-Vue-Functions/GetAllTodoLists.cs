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
    public static class GetAllTodoLists
    {
        [FunctionName("GetAllTodoLists")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            TodoListTableStorage storage = new TodoListTableStorage();
            
            // Get All Lists
            TableQuery<TodoListEntity> query = new TableQuery<TodoListEntity>();
            var results = storage.GetCloudTableReference().ExecuteQuery(query);

            List<ITodoList<IEnumerable<Item>>> items = new List<ITodoList<IEnumerable<Item>>>();
            foreach (TodoListEntity todoListEntity in results)
            {
                TodoList resultItem = new TodoList();
                resultItem.Id = todoListEntity.Id;
                resultItem.Name = todoListEntity.Name;
                resultItem.Items = JsonConvert.DeserializeObject<IEnumerable<Item>>(todoListEntity.Items);
                items.Add(resultItem);
            }

            var awaitedResult = await Task.FromResult(items);

            return req.CreateResponse(HttpStatusCode.OK, awaitedResult);
        }
    }
}
