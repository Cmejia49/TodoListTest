using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo_App.Application.Common.Mappings;
using Todo_App.Domain.Entities;

namespace Todo_App.Application.TodoTags.Queries.SortTags;
public class SortTagTodoItem : IMapFrom<TodoItem>
{
    public int Id { get; set; }
    public int? TagsId { get; set; }
    public string Title { get; set; }
}
