using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
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

namespace Todo.Vue.Functions
{
    public static class GetAllTodoLists
    {
        [FunctionName("GetAllTodoLists")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            // Connect to the Storage account.
            StorageCredentials credentials = new StorageCredentials("todovue", "flvLRKfCXMSNE3r9NWmPdI1BJ6Obr4Mnd+HdKTrkBmjrETwrUsi5fg+Y+IFtf9hCzm6S1PoaX+PyDMU0R6g01A==");
            CloudStorageAccount storageAccount = new CloudStorageAccount(credentials, true);

            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            CloudTable table = tableClient.GetTableReference("todoLists");

            table.CreateIfNotExists();

            // Get All Lists
            TableQuery<TodoListEntity> query = new TableQuery<TodoListEntity>();
            var results = table.ExecuteQuery(query);

            List<TodoList> items = new List<TodoList>();
            foreach (TodoListEntity todoListEntity in results)
            {
                TodoList result = new TodoList();
                result.Id = todoListEntity.RowKey;
                result.Items = todoListEntity.Items_VC;
                result.Name = todoListEntity.Name_VC;

                items.Add(result);
            }

            var awaitedResult = await Task.FromResult(items);

            return req.CreateResponse(HttpStatusCode.OK, awaitedResult);
        }
        
        private class TodoListEntity : TableEntity
        {
            public TodoListEntity(string skey, string srow)
            {
                this.PartitionKey = skey;
                this.RowKey = srow;
            }

            public TodoListEntity() { }

            public Guid Id_VC { get; set; }
            public string Name_VC { get; set; }
            public IEnumerable<Item> Items_VC { get; set; }
        }
    }
}
