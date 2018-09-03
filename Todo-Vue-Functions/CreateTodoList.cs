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
    public static class CreateTodoList
    {
        [FunctionName("CreateTodoList")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            string body = await req.Content.ReadAsStringAsync();
            TodoList newList = JsonConvert.DeserializeObject<TodoList>(body);

            // Define the row,
            Guid newItemGuid = Guid.NewGuid();

            // Create the Entity and set the partition to signup, 
            
            TodoListEntity listEntity = new TodoListEntity("todovue", newItemGuid.ToString());

            listEntity.Name_VC = newList.Name;
            

            // Connect to the Storage account.
            StorageCredentials credentials = new StorageCredentials("todovue", "flvLRKfCXMSNE3r9NWmPdI1BJ6Obr4Mnd+HdKTrkBmjrETwrUsi5fg+Y+IFtf9hCzm6S1PoaX+PyDMU0R6g01A==");
            CloudStorageAccount storageAccount = new CloudStorageAccount(credentials, true);

            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            CloudTable table = tableClient.GetTableReference("todoLists");

            table.CreateIfNotExists();

            TableOperation insertOperation = TableOperation.Insert(listEntity);

            table.Execute(insertOperation);
            newList.Id = newItemGuid;

            return req.CreateResponse(HttpStatusCode.OK, newList);
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
