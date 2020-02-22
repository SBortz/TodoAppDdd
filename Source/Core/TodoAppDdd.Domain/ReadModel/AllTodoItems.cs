using System;
using System.Collections.Generic;
using System.Text;

namespace TodoAppDdd.Domain.ReadModel
{
    public class AllTodoItems
    {
        public int Count { get; set; }
        public IEnumerable<string> TodoItems { get; set; }
    }
}
