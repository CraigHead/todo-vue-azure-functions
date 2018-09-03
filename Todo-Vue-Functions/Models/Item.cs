using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.Vue.Functions.Models
{
    internal class Item
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool Completed { get; set; }
    }
}
