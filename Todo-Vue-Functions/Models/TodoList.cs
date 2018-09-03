using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Table;

namespace Todo.Vue.Functions.Models
{
    internal interface ITodoList
    {
        string Id { get; set; }
        string Name { get; set; }
        IEnumerable<Item> Items { get; set; }
    }

    internal class TodoListEntity : TableEntity, ITodoList
    {
        public TodoListEntity(string key, string row)
        {
            this.PartitionKey = key;
            this.RowKey = row;
        }
        public TodoListEntity() { }

        public string Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Item> Items { get; set; }
    }
}
