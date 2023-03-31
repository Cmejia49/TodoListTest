using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo_App.Domain.Entities;
public class TodoTag: BaseAuditableEntity
{

    public string? Name { get; set; }   

    public IList<TodoItem> Items { get; private set; } = new List<TodoItem>();
}
