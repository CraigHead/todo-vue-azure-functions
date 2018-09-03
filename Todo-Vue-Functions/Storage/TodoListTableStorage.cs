using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;

namespace Todo.Vue.Functions.Storage
{
    public class TodoListTableStorage
    {
        public CloudTable GetCloudTableReference()
        {
            CloudStorageAccount storageAccount = Connect();
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("todoLists");

            SatisfyTableCreation(table);

            return table;
        }

        private CloudStorageAccount Connect()
        {
            // Connect to the Storage account.
            StorageCredentials credentials = new StorageCredentials("todovue", "flvLRKfCXMSNE3r9NWmPdI1BJ6Obr4Mnd+HdKTrkBmjrETwrUsi5fg+Y+IFtf9hCzm6S1PoaX+PyDMU0R6g01A==");
            return new CloudStorageAccount(credentials, true);
        }

        private bool SatisfyTableCreation(CloudTable table)
        {
            return table.CreateIfNotExists();
        }
    }
}
