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

            List<ITodoList> items = new List<ITodoList>();
            foreach (TodoListEntity todoListEntity in results)
            {
                items.Add(todoListEntity);
            }

            var awaitedResult = await Task.FromResult(items);

            return req.CreateResponse(HttpStatusCode.OK, awaitedResult);
        }
    }
}
