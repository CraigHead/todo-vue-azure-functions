using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Table;

namespace Todo.Vue.Functions.Models
{
    internal interface ITodoList<TItemsType>
    {
        string Id { get; set; }
        string Name { get; set; }
        TItemsType Items { get; set; }
    }

    internal class TodoList : ITodoList<IEnumerable<Item>>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Item> Items { get; set; }
    }

    internal class TodoListEntity : TableEntity, ITodoList<string>
    {
        public TodoListEntity(string key, string row)
        {
            this.PartitionKey = key;
            this.RowKey = row;
        }
        public TodoListEntity() { }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Items { get; set; }
    }
}
