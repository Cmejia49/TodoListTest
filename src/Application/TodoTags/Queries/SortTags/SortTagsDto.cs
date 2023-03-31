using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo_App.Application.Common.Mappings;
using Todo_App.Domain.Entities;

namespace Todo_App.Application.TodoTags.Queries.SortTags;
public class SortTagsDto : IMapFrom<TodoTag>
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public IList<TodoItem> Items { get; private set; } = new List<TodoItem>();
}
