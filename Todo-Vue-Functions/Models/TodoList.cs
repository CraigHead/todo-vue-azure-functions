using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.Vue.Functions.Models
{
    internal class TodoList
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Item> Items { get; set; }
    }
}
